using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
	public ParticleSystem steam;
	public Trap trap;

    void Start()
    {
    }

    void Update()
    {
		if (trap.m_State == TrapState.Broken_WaitForFix)
		{
			steam.Play();
		}

		if (trap.m_State == TrapState.WaitingForAttack)
		{
			steam.Stop();
		}
	}
}
