﻿using UnityEngine;
using System.Collections;

public enum PlayerSlot
{
	Player1,
	Player2,
	Player3,
	Player4
}



public class PlayerController : MonoBehaviour
{
	public float		Speed				= 5f;
	public float		JumpHeight			= 2f;
	public float		GroundDistance		= 0.2f;
	public float		DashDistance		= 5f;
	public PlayerSlot	Slot				= PlayerSlot.Player1;
	public LayerMask	GroundLayer;

	[Header("Debug")]
	private Rigidbody m_Rigidbody;
	private Vector3 m_Inputs		= Vector3.zero;
	private bool m_IsGrounded		= true;
	public Tool	EquippedTool			= null;

	///////////////////////////////////////////////////////////////////////////

	void Start()
	{
		m_Rigidbody		= GetComponent<Rigidbody>();
	}

	///////////////////////////////////////////////////////////////////////////

	string GetInputPrefix()
	{
		return "P" + (((int)Slot ) + 1).ToString("0") + " ";
	}

	///////////////////////////////////////////////////////////////////////////

	void Update()
	{
		m_IsGrounded = Physics.CheckSphere(transform.position, GroundDistance, GroundLayer, QueryTriggerInteraction.Ignore);

		m_Inputs = Vector3.zero;
		m_Inputs.x =   Input.GetAxis(GetInputPrefix() + "Horizontal");
		m_Inputs.z = - Input.GetAxis(GetInputPrefix() + "Vertical");

		if (Mathf.Abs(m_Inputs.x) < 0.1f)
		{
			m_Inputs.x = 0.0f;
		}
		if (Mathf.Abs(m_Inputs.y) < 0.1f)
		{
			m_Inputs.y = 0.0f;
		}

		if (m_Inputs != Vector3.zero)
		{
			transform.forward = m_Inputs;
		}

		/*if (Input.GetButtonDown("Jump") && m_IsGrounded)
		{
			m_Rigidbody.AddForce(Vector3.up * JumpHeight , ForceMode.VelocityChange);
		}*/
		if (Input.GetButtonDown(GetInputPrefix() + "Dash"))
		{
			Vector3 dashVelocity = transform.forward * DashDistance;
			m_Rigidbody.AddForce(dashVelocity, ForceMode.VelocityChange);
		}

		TryInteraction();
	}

	///////////////////////////////////////////////////////////////////////////

	void TryInteraction()
	{
		bool doInteract = Input.GetButtonDown(GetInputPrefix() + "Interact");

		if (doInteract)
		{
			Vector2 selfPos = transform.position.xz();
			Vector2 selfLookDir = transform.forward.xz();
			selfLookDir.Normalize();

			// 1) Traps
			Trap nearestTrap = TrapManager.Get().GetNearestTrap(true, EquippedTool, selfPos, true, true, selfLookDir);

			if (nearestTrap)
			{
				nearestTrap.Interact();
				return;
			}
			
			if (TrapManager.Get().GetNearestTrap(false, EquippedTool, selfPos, true, true, selfLookDir))
			{
				WorldSpaceCanvas.Get().AddText("Wrong Tool", transform.position);
				return;
			}

			Tool nearestTool = ToolTrolley.Get().GetNearestTool(selfPos, true, selfLookDir);

			if (nearestTool)
			{
				ToolTrolley.Get().SwitchTool(ref EquippedTool, nearestTool);
				return;
			}
		}
	}

	void FixedUpdate()
	{
		if (m_Inputs.magnitude > 0.1f)
		{
			m_Rigidbody.MovePosition(m_Rigidbody.position + m_Inputs * Speed * Time.fixedDeltaTime);
		}
	}

}