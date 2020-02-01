using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapState
{
    WaitingForAttack,
    Warning,
    Broken_WaitForFix,
}

public class Trap : MonoBehaviour
{
    public float AttackCooldownMin      = 10.0f;
    public float AttackCooldownMax      = 15.0f;
    public float AttackWarningDuration  = 10.0f;
    public bool  CanAttack              = true;

    float       m_NextStateChange       = 0.0f;

    public TrapState   m_State = TrapState.WaitingForAttack;
    
    private void Awake()
    {
        
    }

    private void Start()
    {
        ChangeToState(TrapState.WaitingForAttack);
    }

    private void ChangeToState(TrapState forceSetState)
    {
		switch (m_State)
		{
			case TrapState.WaitingForAttack:
				m_NextStateChange = Random.Range(AttackCooldownMin, AttackCooldownMax); break;
			case TrapState.Warning:
                m_NextStateChange = AttackWarningDuration;                   			break;
			case TrapState.Broken_WaitForFix:                                   		break;
			
                default: Debug.Assert(false); break;
		}
	}

    private void Update()
    {
        float timeUntilNextStateChange = Time.time - m_NextStateChange;

        switch (m_State)
        {
            case TrapState.WaitingForAttack:
                if (timeUntilNextStateChange <= 0)
                {
                    ChangeToState(TrapState.Warning);
                }
                break;

            case TrapState.Warning:
                if (timeUntilNextStateChange <= 0)
                {
                    if (CanAttack)
                    {
                        ChangeToState(TrapState.Broken_WaitForFix);
                    }
                }
                break;

            case TrapState.Broken_WaitForFix:
                break;
            default: Debug.Assert(false);                break;
        }
    }
}
