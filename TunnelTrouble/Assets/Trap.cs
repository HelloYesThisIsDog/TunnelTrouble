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
    public float AttackCooldownMin      = 10.0f;
    public float AttackCooldownMax      = 15.0f;
    public float AttackWarningDuration  = 10.0f;

    TrapState   m_State = TrapState.WaitingForAttack;

 
}
