using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{
	public ParticleSystem drops;
	public Trap trap;

	void Start()
	{
		drops.Play();
	}

	void Update()
	{
		if (trap.m_State == TrapState.Broken_WaitForFix)
		{
			drops.Stop();
		}

		if (trap.m_State == TrapState.WaitingForAttack)
		{
			drops.Play();
		}
	}
}
