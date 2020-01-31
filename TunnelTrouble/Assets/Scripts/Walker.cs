using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public float    DirectionChangeInterval    = 1.0f;
    public float    DirectionChangeDelta    = 1.0f;
    public float    Acceleration            = 1.0f;          

    Rigidbody       m_Rigidbody;
    float           m_CurAngle              = 0.0f;
    float           m_NextDirectionChange   = 0;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();    
    }

    void Update()
    {
        m_NextDirectionChange -= Time.deltaTime;

        if (m_NextDirectionChange <= 0)
        {
            m_CurAngle += Random.Range(-DirectionChangeDelta, DirectionChangeDelta);
            m_CurAngle = NormalizeAngle(m_CurAngle);
            m_NextDirectionChange = DirectionChangeInterval;
        }

        Vector3 directionVector = Quaternion.AngleAxis(m_CurAngle, Vector3.up) * Vector3.right;

        m_Rigidbody.AddForce(Acceleration * directionVector , ForceMode.VelocityChange);
    }

	public static float NormalizeAngle(float angle)
	{
		float result = angle - Mathf.CeilToInt(angle / 360f) * 360f;
		if (result < 0)
		{
			result += 360f;
		}
		return result;
	}
}
