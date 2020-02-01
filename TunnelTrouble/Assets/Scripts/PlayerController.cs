using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

	public float Speed				= 5f;
	public float JumpHeight			= 2f;
	public float GroundDistance		= 0.2f;
	public float DashDistance		= 5f;
	public LayerMask GroundLayer;

	private Rigidbody m_Rigidbody;
	private Vector3 m_Inputs		= Vector3.zero;
	private bool m_IsGrounded		= true;

	///////////////////////////////////////////////////////////////////////////

	void Start()
	{
		m_Rigidbody		= GetComponent<Rigidbody>();
	}

	///////////////////////////////////////////////////////////////////////////

	void Update()
	{
		m_IsGrounded = Physics.CheckSphere(transform.position, GroundDistance, GroundLayer, QueryTriggerInteraction.Ignore);

		m_Inputs = Vector3.zero;
		m_Inputs.x = Input.GetAxis("Horizontal");
		m_Inputs.z = Input.GetAxis("Vertical");
		if (m_Inputs != Vector3.zero)
		{
			transform.forward = m_Inputs;
		}

		if (Input.GetButtonDown("Jump") && m_IsGrounded)
		{
			m_Rigidbody.AddForce(Vector3.up * JumpHeight , ForceMode.VelocityChange);
		}
		if (Input.GetButtonDown("Dash"))
		{
			Vector3 dashVelocity = transform.forward * DashDistance;
			m_Rigidbody.AddForce(dashVelocity, ForceMode.VelocityChange);
		}

		TryInteraction();
	}

	///////////////////////////////////////////////////////////////////////////

	void TryInteraction()
	{
		bool doInteract = Input.GetButtonDown("Interact");

		if (doInteract)
		{
			Vector2 selfPos = transform.position.xz();
			Trap nearestTrap = TrapManager.Get().GetNearestTrap(selfPos, true);

			if (nearestTrap && nearestTrap.CanBeInteractedWith())
			{
				nearestTrap.Interact();
			}
			else
			{
				if (nearestTrap)
				{
					Debug.Log("Cannot interact with " + nearestTrap.name.AddBrackets());
				}
			}
		}
	}

	void FixedUpdate()
	{
		m_Rigidbody.MovePosition(m_Rigidbody.position + m_Inputs * Speed * Time.fixedDeltaTime);
	}

}