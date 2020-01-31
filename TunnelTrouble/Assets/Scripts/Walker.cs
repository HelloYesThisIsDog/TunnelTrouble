using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public float    DirectionChangeInterval     = 1.0f;
    public float    DirectionChangeDelta        = 1.0f;
    public float    Acceleration                = 1.0f;          
    public float    ReachTargetThreshold        = 0.2f;

    Rigidbody       m_Rigidbody;
    float           m_CurAngle              = 0.0f;
    float           m_NextDirectionChange   = 0;
    
    public int      m_CurrentTargetPointIndex = 0;

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

    void Update()
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
                return;
            }
        }

		/*m_NextDirectionChange -= Time.deltaTime;

		if (m_NextDirectionChange <= 0)
		{
			m_CurAngle += Random.Range(-DirectionChangeDelta, DirectionChangeDelta);
			m_CurAngle = NormalizeAngle(m_CurAngle);
			m_NextDirectionChange = DirectionChangeInterval;
		}

		Vector3 directionVector = Quaternion.AngleAxis(m_CurAngle, Vector3.up) * Vector3.right;*/
        
        Vector2 selfToTarget2D_Norm = selfToTarget2D;
        selfToTarget2D_Norm.Normalize();

        Vector3 directionVector = selfToTarget2D_Norm.To3D(0.0f);

        m_Rigidbody.AddForce(Acceleration * directionVector , ForceMode.VelocityChange);
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
