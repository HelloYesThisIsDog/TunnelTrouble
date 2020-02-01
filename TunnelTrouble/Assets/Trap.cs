using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapState
{
    WaitingForAttack,
    Warning,
    Attacking_WaitForFix,
    Broken_WaitForFix,
}

public class Trap : MonoBehaviour
{
 /*   public float AttackCooldownMin      = 10.0f;
    public float AttackCooldownMax      = 15.0f;
    public float AttackWarningDuration  = 10.0f;

    float       m_NextStateChange       = 0.0f;

    TrapState   m_State = TrapState.WaitingForAttack;
    
    private void Awake()
    {
        
    }

    private void Start()
    {
        ChangeToState(TrapState.WaitingForAttack);
    }

    private void ChangeToState(TrapState forceSetState)
    {
        
    }

    private void Update()
    {
        float timeSinceLastStateChange = Time.time - m_LastStateChange;

        switch ( m_State )
        {
            case TrapState.WaitingForAttack:
                if (timeSinceLastStateChange > )
                break;
            case TrapState.Warning:
                break;
            case TrapState.Attacking_WaitForFix:
                break;
            case TrapState.Broken_WaitForFix:
                break;
            default: Debug.Assert(false);                break;

        }
    }*/
}
