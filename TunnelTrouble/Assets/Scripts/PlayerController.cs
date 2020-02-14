using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlayerSlot
{
	Player1,
	Player2,
	Player3,
	Player4,

	PlayerCount
}


public class PlayerController : MonoBehaviour
{
	public float		Speed				= 5f;
	public float		JumpHeight			= 2f;
	public float		GroundDistance		= 0.2f;
	public float		DashDistance		= 5f;
	public float		DashCooldown		= 0.5f;
	public AudioClip[]	DashSounds;
	public float		DashVolume			= 1.0f;
	public float		DashNoergelRadius	= 2.5f;
	public float		DashNoergelDot		= 0.2f;
	public PlayerSlot	Slot				= PlayerSlot.Player1;
	public LayerMask	GroundLayer;
	public AudioClip[]	FailSound;
    public Animator		CharAnim;

	public GameObject	HighlightedObject		= null;
	public GameObject	HighlightedObjectIcon	= null;

	[Header("Debug")]
	private Rigidbody m_Rigidbody;
	private Vector3 m_Inputs		= Vector3.zero;
	public Vector3 m_LastForward    = Vector3.right;
	private bool m_IsGrounded		= true;
	public Tool	EquippedTool		= null;
	public float m_LastDash			= 0.0f;

    public Transform[] ToolVisuals = null ;


	public float MegaphoneCooldown			= 1.0f;
	private float m_LastMegaphoneUsage		= 0.0f;
    public float MegaphoneImpactDistance	= 2.0f;
	public float MegaphoneInnerRadius		= 5.0f;
	public float MegaphoneOuterRadius		= 10.0f;

	public ParticleSystem dash_ps;

    ///////////////////////////////////////////////////////////////////////////

    public static Color GetPlayerColor(PlayerSlot slot)
	{
		switch(slot)
		{
			case PlayerSlot.Player1:	return new Color(1,0,0);
			case PlayerSlot.Player2:	return new Color(0,1,0);
			case PlayerSlot.Player3:	return new Color(1,1,0);
			case PlayerSlot.Player4:	return new Color(0,1,1);
		}

		return Color.white;
	}

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
			Vector3 forward = m_Inputs;
			forward.Normalize();
			transform.forward = forward;
			m_LastForward = forward;
		}
		else
		{
			transform.forward = m_LastForward;
		}

		/*if (Input.GetButtonDown("Jump") && m_IsGrounded)
		{
			m_Rigidbody.AddForce(Vector3.up * JumpHeight , ForceMode.VelocityChange);
		}*/
		if (Input.GetButtonDown(GetInputPrefix() + "Dash"))
		{
			if (Time.time - m_LastDash > DashCooldown)
			{
				Vector3 dashVelocity = transform.forward * DashDistance;
				m_Rigidbody.AddForce(dashVelocity, ForceMode.VelocityChange);
				m_LastDash = Time.time;

				AudioManager.Get().PlayRandomOneShot(gameObject, DashSounds, DashVolume);

				PlayNoergelSound();
				dash_ps.Play();
			}
		}

		UpdateHighlightedObject();

		TryInteraction();
	}

	///////////////////////////////////////////////////////////////////////////

	public void UpdateHighlightedObject()
	{
		GameObject newHighlightObject = null;

		Vector2 selfPos = transform.position.xz();
		Vector2 lookDir = transform.forward.xz();
		lookDir.Normalize();
		Trap nearestTrap = TrapManager.Get().GetNearestTrap(true, EquippedTool, selfPos, true, true, lookDir);

		if (nearestTrap)
		{
			newHighlightObject = nearestTrap.gameObject;
		}
		else
		{
			Tool nearestTool = ToolTrolley.Get().GetNearestTool(selfPos, true, lookDir);
			if (nearestTool)
			{
				newHighlightObject = nearestTool.gameObject;
			}
		}

		if (newHighlightObject != HighlightedObject)
		{
			if (HighlightedObjectIcon)
			{
				GameObject.Destroy(HighlightedObjectIcon);
			}

			if (newHighlightObject)
			{
				// highlight new
				HighlightedObjectIcon = WorldSpaceCanvas.Get().CreateInteractIcon(GetPlayerColor(Slot));
			}

			HighlightedObject = newHighlightObject;
		}
		
		if (HighlightedObject && HighlightedObjectIcon)
		{
			if (!EquippedTool || HighlightedObject != EquippedTool.gameObject)
			{
				float offset = HighlightedObject.gameObject.GetComponent<Tool>() ? 0.8f : 2.0f;
				HighlightedObjectIcon.transform.position = HighlightedObject.transform.position + Vector3.up * offset;

				Vector3 offsetTowardsCamera = (Camera.main.transform.position - HighlightedObject.transform.position);
				offsetTowardsCamera.Normalize();

				HighlightedObjectIcon.transform.position += offsetTowardsCamera * 2.0f;
			}
		}
	}


	///////////////////////////////////////////////////////////////////////////

	void PlayNoergelSound()
	{
		Walker nearestWalker = WalkerManager.Get().GetNearestWalker(transform.position.xz(), DashNoergelRadius, transform.forward.xz(), DashNoergelDot);

		if (!nearestWalker)
		{
			return;
		}

		AudioManager.Get().PlayRandomOneShot(nearestWalker.gameObject, nearestWalker.NoergelSound, nearestWalker.NoergelSoundVolume);
	}

	///////////////////////////////////////////////////////////////////////////

	void TryInteraction()
	{
		bool doInteract = Input.GetButtonDown(GetInputPrefix() + "Interact");

		if (doInteract)
		{
            CharAnim.SetTrigger("Work");
			Vector2 selfPos = transform.position.xz();
			Vector2 selfLookDir = transform.forward.xz();
			selfLookDir.Normalize();

			// 1) Traps
			if (EquippedTool && EquippedTool._ToolType != ToolType.Megaphone)
			{
				Trap nearestTrap = TrapManager.Get().GetNearestTrap(true, EquippedTool, selfPos, true, true, selfLookDir);

				if (nearestTrap)
				{
					if (EquippedTool)
					{
						AudioManager.Get().PlayRandomOneShot(gameObject, EquippedTool.RepairSound, EquippedTool.RepairSoundVolume);
					}

					nearestTrap.Interact();
					return;
				}
			
				if (TrapManager.Get().GetNearestTrap(false, EquippedTool, selfPos, true, true, selfLookDir))
				{
					PlayFailSound();
					WorldSpaceCanvas.Get().AddText("Wrong Tool", transform.position);
					return;
				}
			}

			// 2) Trolley
			Tool nearestTool = ToolTrolley.Get().GetNearestTool(selfPos, true, selfLookDir);

			if (nearestTool)
			{
				ToolTrolley.Get().SwitchTool(ref EquippedTool, nearestTool);
                if ( EquippedTool._ToolType == ToolType.Drill)
                {
                    ToolVisuals[0].gameObject.SetActive(true);
                    ToolVisuals[1].gameObject.SetActive(false);

                    ToolVisuals[2].gameObject.SetActive(false);
                    ToolVisuals[3].gameObject.SetActive(false);

                }
                else if (EquippedTool._ToolType == ToolType.Hammer)
                {
                    ToolVisuals[0].gameObject.SetActive(false);
                    ToolVisuals[1].gameObject.SetActive(true);

                    ToolVisuals[2].gameObject.SetActive(false);
                    ToolVisuals[3].gameObject.SetActive(false);

                }
                else if (EquippedTool._ToolType == ToolType.Megaphone)
                {
                    ToolVisuals[0].gameObject.SetActive(false);
                    ToolVisuals[1].gameObject.SetActive(false);

                    ToolVisuals[2].gameObject.SetActive(true);
                    ToolVisuals[3].gameObject.SetActive(false);

                }
                else if (EquippedTool._ToolType == ToolType.Mop)
                {
                    ToolVisuals[0].gameObject.SetActive(false);
                    ToolVisuals[1].gameObject.SetActive(false);

                    ToolVisuals[2].gameObject.SetActive(false);
                    ToolVisuals[3].gameObject.SetActive(true);
                }
                return;
			}

			// 3) Megaphone
			if (EquippedTool && EquippedTool._ToolType == ToolType.Megaphone)
			{
				if (Time.time - m_LastMegaphoneUsage > MegaphoneCooldown)
				{
					AudioManager.Get().PlayRandomOneShot(EquippedTool.gameObject, EquippedTool.RepairSound, EquippedTool.RepairSoundVolume);
					m_LastMegaphoneUsage = Time.time;

					Vector2 MegaphoneImpactCenter = transform.position.xz() + transform.forward.xz() * MegaphoneImpactDistance;

					List<Walker> walkers = WalkerManager.Get().GetAllWalkers(Vector2.zero, null);

					foreach (Walker walker in walkers)
					{
						float dist = Vector2.Distance(walker.transform.position.xz(), MegaphoneImpactCenter);

						float impactAmount = 1.0f - Mathf.InverseLerp(MegaphoneInnerRadius, MegaphoneOuterRadius, dist);
						impactAmount = Mathf.Clamp01(impactAmount);

						if (impactAmount <= 0.0f)
						{
							continue;
						}

						float debugHeight = transform.position.y + 0.1f;
						Debug.DrawLine(MegaphoneImpactCenter.To3D(debugHeight), walker.transform.position.xz().To3D(debugHeight));

						Vector2 pushDir = walker.transform.position.xz() - MegaphoneImpactCenter;
						
						walker.MegaphoneForceAmountNorm = impactAmount;
						walker.MegaphoneForceDirection = pushDir.normalized;
					}
				}

				return;
			}

			// 4) Error
			PlayFailSound();
		}
	}

	void PlayFailSound()
	{
		AudioManager.Get().PlayRandomOneShot(gameObject, FailSound, 1.0f);
	}

	void FixedUpdate()
	{
		if (m_Inputs.magnitude > 0.3f)
		{
            CharAnim.SetTrigger("Walk");
			m_Rigidbody.MovePosition(m_Rigidbody.position + m_Inputs * Speed * Time.fixedDeltaTime);
		}
	}

}