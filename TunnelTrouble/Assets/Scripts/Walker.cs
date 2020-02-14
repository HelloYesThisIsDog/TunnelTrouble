using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public float    DirectionChangeInterval     = 1.0f;
    public float    DirectionChangeDelta        = 1.0f;
    public float    Speed                       = 1.0f;          
    public float    ReachTargetThreshold        = 0.8f;
    public float    ClampVelocity               = 1.0f;
    public int      ID                          = -1;
    public float    TimeBonusWhenRescued        = 1.0f;
    public float    StuckThreshold              = 0.7f;
    public float    DieAfterStuck               = 20.0f;
    public float    PositionSnapshotInterval    = 2.0f;
	public bool		IsKilled					= false;

    public AudioClip[]  NoergelSound;
    public float        NoergelSoundVolume;

    public ParticleSystem stuckPS;
	public ParticleSystem bloodPS;
	public GameObject model;

    Rigidbody       m_Rigidbody;
    
    [Header("Debug")]
    public int      m_CurrentTargetPointIndex = 0;
    Vector3         m_Direction                 = Vector3.right;
    public Vector2  LastPositionSnapshot        = Vector2.zero;
    float           LastPositionSnapshotTime    = 0.0f;
    float?          IsStuckSince                = null;

	[Header("Megaphone")]
	public Vector2  MegaphoneForceDirection		= Vector2.one;
	public float	MegaphoneForceAmountNorm	= 0.0f;
	public float	MegaphoneForceImpact		= 24.0f;
	public float	MegaphoneMaxVelocityImpact	= 10.0f;
	public float	MegaphoneFalloff			= 0.9f;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();    
        LastPositionSnapshotTime        = Time.time;
        LastPositionSnapshot            = transform.position.xz() + new Vector2(100, 100);
    }

	///////////////////////////////////////////////////////////////////////////

    public void Kill()
    {
		if (IsKilled)
		{
			return;
		}

		IsKilled = true;
        AudioManager.Get().PlayRandomOneShot(WalkerManager.Get().gameObject, WalkerManager.Get().KillSound, WalkerManager.Get().KillSoundVolume);
		bloodPS.Play();
		Speed = 0;
		model.SetActive(false);
        GameObject.Destroy(gameObject, 2f);
    }

    ///////////////////////////////////////////////////////////////////////////

	Vector3 GetTargetPoint()
    {
		int pathPointCount = WalkerPath.Get().PathLines.Length;

		if (pathPointCount == 0 || m_CurrentTargetPointIndex >= pathPointCount)
        {
            Debug.Assert(false);
            return Vector3.zero;
        }

        float fixedRnd = VectorExtensions.GetFixedRandom(ID);
        Vector3 pathPoint = WalkerPath.Get().PathLines[m_CurrentTargetPointIndex].GetLerped(fixedRnd);
        return pathPoint;
    }

    ///////////////////////////////////////////////////////////////////////////

    private void OnDrawGizmosSelected()
    {
        Vector3 targetPos = GetTargetPoint();

        Color oldColor = Gizmos.color;

        Debug.DrawLine(transform.position, targetPos, Color.cyan);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(targetPos, ReachTargetThreshold);

        Gizmos.color = oldColor;
    }

    ///////////////////////////////////////////////////////////////////////////

    bool HandleWaypointChange()
    {
		Vector3 targetPoint = GetTargetPoint();
		Vector2 selfToTarget2D = (targetPoint - transform.position).xz();

		float distanceToTarget = selfToTarget2D.magnitude;

		if (distanceToTarget < ReachTargetThreshold)
		{
			int pathPointCount = WalkerPath.Get().PathLines.Length;
			m_CurrentTargetPointIndex++;

			if (m_CurrentTargetPointIndex >= pathPointCount)
			{
                GameManager.Get().m_RescuedWalkers++;
                GameManager.Get().AddTimeBonus(TimeBonusWhenRescued);
                AudioManager.Get().PlayRandomOneShot(WalkerManager.Get().gameObject, WalkerManager.Get().RescueSound, WalkerManager.Get().RescueSoundVolume);
				m_CurrentTargetPointIndex--;
				GameObject.Destroy(gameObject);
				return false;
			}
		}

        return true;
	}

    ///////////////////////////////////////////////////////////////////////////

    private void FixedUpdate()
    {
		MegaphoneForceAmountNorm *= MegaphoneFalloff;

		if (MegaphoneForceAmountNorm < 0.01f)
		{
			MegaphoneForceAmountNorm = 0.0f;
		}

		Vector2 forceVector = Speed * m_Direction;
		float clampVelocity = ClampVelocity;

		if (MegaphoneForceAmountNorm != 0.0f)
		{
			forceVector = MegaphoneForceAmountNorm * MegaphoneForceDirection * MegaphoneForceImpact;
			clampVelocity = Mathf.Lerp(1.0f, MegaphoneMaxVelocityImpact, MegaphoneForceAmountNorm);
		}


		m_Rigidbody.AddForce(forceVector, ForceMode.VelocityChange);

		m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, clampVelocity);
	}

    ///////////////////////////////////////////////////////////////////////////
    
    void SetStuckParticlesEnabled(bool enable)
    {
		if (stuckPS)
		{
            if (enable)
            {
			    stuckPS.Play();
            }
            else
            {
                stuckPS.Stop();
            }
        }
        else
        {
            WorldSpaceCanvas.Get().AddText("I'm Angry!", transform.position);
        }
	}


    ///////////////////////////////////////////////////////////////////////////

    void TickStuckCheck()
    {
		if (Time.time - LastPositionSnapshotTime > PositionSnapshotInterval)
		{
            Vector2 lastSnapshot = LastPositionSnapshot;

			LastPositionSnapshotTime    = Time.time;
		    LastPositionSnapshot        = transform.position.xz();

            bool stuckThisTick = (lastSnapshot - transform.position.xz()).magnitude < StuckThreshold;
            
            if (stuckThisTick)
            {
                if (IsStuckSince.HasValue)
                {
                    // still stuck
                    if (Time.time - IsStuckSince.Value > DieAfterStuck)
                    {
                        Kill();
                        return;
                    }
                }
                else
                {
                    // become stuck
                    SetStuckParticlesEnabled(true);
                    IsStuckSince = Time.time;
                }
            }
            else
            {
                if (IsStuckSince.HasValue)
                {
                    SetStuckParticlesEnabled(false);
                    IsStuckSince = null;
                }
                else
                {
                    // stay unstucked
                }
            }
        }
	}

    ///////////////////////////////////////////////////////////////////////////

    void Update()
    {
        if (!HandleWaypointChange())
        {
            return;
        }

		Vector3 targetPoint = GetTargetPoint();
		Vector2 selfToTarget2D = (targetPoint - transform.position).xz();

		Vector2 selfToTarget2D_Norm = selfToTarget2D;
        selfToTarget2D_Norm.Normalize();

        Vector3 directionVector = selfToTarget2D_Norm.To3D(0.0f);
        m_Direction = directionVector;

        /*Vector3 smoothForwardDir = Vector3.Lerp(transform.forward, directionVector, 0.2f);
        smoothForwardDir.Normalize();*/
        transform.forward = directionVector;

        TickStuckCheck();
    }

    ///////////////////////////////////////////////////////////////////////////

	public static float NormalizeAngle(float angle)
	{
		float result = angle - Mathf.CeilToInt(angle / 360f) * 360f;
		if (result < 0)
		{
			result += 360f;
		}
		return result;
	}

    ///////////////////////////////////////////////////////////////////////////
}
