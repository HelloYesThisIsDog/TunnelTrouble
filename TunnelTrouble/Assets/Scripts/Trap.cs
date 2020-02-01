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
    public float InteractRadius         =  2.0f;
    public float AttackRadius           =  4.0f;

    [Header("Debug")]
    public float        m_TimeUntilNextStateChange       = 0.0f;

    public TrapState   m_State = TrapState.WaitingForAttack;
    
    ///////////////////////////////////////////////////////////////////////////
    
    private void Start()
    {
        ChangeToState(TrapState.WaitingForAttack);
    }

    ///////////////////////////////////////////////////////////////////////////

    private void ChangeToState(TrapState forceSetState)
    {
		switch (forceSetState)
		{
			case TrapState.WaitingForAttack:
				m_TimeUntilNextStateChange = Random.Range(AttackCooldownMin, AttackCooldownMax); break;
			case TrapState.Warning:
                m_TimeUntilNextStateChange = AttackWarningDuration;                   			break;
			case TrapState.Broken_WaitForFix:                                   		break;
			
                default: Debug.Assert(false); break;
		}

        m_State = forceSetState;
	}

    ///////////////////////////////////////////////////////////////////////////

    public bool CanBeInteractedWith()
    {
        return (m_State == TrapState.Broken_WaitForFix) || (m_State == TrapState.Warning);
    }

    ///////////////////////////////////////////////////////////////////////////

    public void OnDrawGizmosSelected()
    {
        Color oldColor = Gizmos.color;
        
        bool drawAttackRadius   = false;;
        bool drawInteractRadius = false;

        Color newColor = Color.black;

        if (Application.isPlaying)
        {
            switch (m_State)
            {
                case TrapState.WaitingForAttack:    newColor = Color.green;         drawInteractRadius = true; break;
                case TrapState.Warning:             newColor = Color.yellow;        drawInteractRadius = true; drawAttackRadius = true; break;
                case TrapState.Broken_WaitForFix:   newColor = Color.red;           drawInteractRadius = true; break;
                default:
                    break;

            }
        }
        else
        {
            newColor = Color.yellow;
            drawAttackRadius = true;
            drawInteractRadius = true;
        }

        Gizmos.color = newColor;

        if (drawInteractRadius)
        {
            Gizmos.DrawWireSphere(transform.position, InteractRadius);
        }
        if (drawAttackRadius)
        {
            Gizmos.DrawWireSphere(transform.position, AttackRadius);
        }

        Gizmos.color = oldColor;
    }

    ///////////////////////////////////////////////////////////////////////////

    public void Interact()
    {
        if (m_State == TrapState.Broken_WaitForFix || m_State == TrapState.Warning)
        {
            ChangeToState(TrapState.WaitingForAttack);
            Debug.Log("Fixed " + gameObject.name.AddBrackets());
        }
    }

    ///////////////////////////////////////////////////////////////////////////

    private void Update()
    {
        m_TimeUntilNextStateChange -= Time.deltaTime;       

        switch (m_State)
        {
            case TrapState.WaitingForAttack:
                if (m_TimeUntilNextStateChange <= 0)
                {
                    Debug.Log("Warn " + gameObject.name.AddBrackets());
                    ChangeToState(TrapState.Warning);
                }
                break;

            case TrapState.Warning:
                if (m_TimeUntilNextStateChange <= 0)
                {
                    Debug.Log("Attack " + gameObject.name.AddBrackets());
                    Attack();
                    ChangeToState(TrapState.Broken_WaitForFix);
                }
                break;

            case TrapState.Broken_WaitForFix:
                break;
            default: Debug.Assert(false);                break;
        }
    }

    ///////////////////////////////////////////////////////////////////////////
    
    void Attack()
    {
        List<Walker> walkersInRange = WalkerManager.Get().GetAllWalkers(transform.position.xz(), AttackRadius);

        foreach (Walker walker in walkersInRange)
        {
            walker.Kill();
        }
    }

    ///////////////////////////////////////////////////////////////////////////
}
