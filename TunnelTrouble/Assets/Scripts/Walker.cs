using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public float    DirectionChangeInterval     = 1.0f;
    public float    DirectionChangeDelta        = 1.0f;
    public float    Speed                       = 1.0f;          
    public float    ReachTargetThreshold        = 0.2f;
    public float    ClampVelocity               = 1.0f;

    Rigidbody       m_Rigidbody;
    
    public int      m_CurrentTargetPointIndex = 0;
    Vector3         m_Direction = Vector3.right;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();    
    }

    ///////////////////////////////////////////////////////////////////////////

    Vector3 GetTargetPoint()
    {
        int pathPointCount = WalkerPath.Get().PathPoints.Length;

        if (pathPointCount == 0 || m_CurrentTargetPointIndex >= pathPointCount)
        {
            Debug.Assert(false);
            return Vector3.zero;
        }

        Vector3 pathPoint = WalkerPath.Get().PathPoints[m_CurrentTargetPointIndex].position;
        return pathPoint;
    }

    ///////////////////////////////////////////////////////////////////////////

    private void OnDrawGizmos()
    {
        Vector3 targetPos = GetTargetPoint();

        Debug.DrawLine(transform.position, targetPos);
    }

    ///////////////////////////////////////////////////////////////////////////

    bool HandleWaypointChange()
    {
		Vector3 targetPoint = GetTargetPoint();
		Vector2 selfToTarget2D = (targetPoint - transform.position).xz();

		float distanceToTarget = selfToTarget2D.magnitude;

		if (distanceToTarget < ReachTargetThreshold)
		{
			int pathPointCount = WalkerPath.Get().PathPoints.Length;
			m_CurrentTargetPointIndex++;

			if (m_CurrentTargetPointIndex >= pathPointCount)
			{
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
		m_Rigidbody.AddForce(Speed * m_Direction, ForceMode.VelocityChange);

		m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, ClampVelocity);
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

        Vector3 smoothForwardDir = Vector3.Lerp(transform.forward, directionVector, 0.2f);
        smoothForwardDir.Normalize();
        transform.forward = smoothForwardDir;
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
