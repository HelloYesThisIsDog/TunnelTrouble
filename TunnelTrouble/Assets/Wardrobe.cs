using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wardrobe : MonoBehaviour
{
	public ParticleSystem dust;
	public Trap trap;

	private bool playable = true;

	void Update()
	{
		if (trap.m_State == TrapState.Broken_WaitForFix && playable)
		{
			dust.Play();
			playable = false;
		}

		if (trap.m_State == TrapState.WaitingForAttack && !playable)
		{
			playable = true;
		}
	}
}
