using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

	[SerializeField] private float m_Acceleration;

	private Rigidbody m_Rigidbody;

	void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		float moxeH = Input.GetAxis("Horizontal");
		float moveV = Input.GetAxis("Vertical");

		Vector3 moveDir = new Vector3(moxeH, 0.0f, moveV);

		m_Rigidbody.AddForce(moveDir * m_Acceleration);
	}
}