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
    public Animator TrapAnim;
    public ToolType ToolToFix           = ToolType.Drill;

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
                TrapAnim.SetTrigger("Reset");
                TrapAnim.ResetTrigger("Activate");

                m_TimeUntilNextStateChange = Random.Range(AttackCooldownMin, AttackCooldownMax); break;
			case TrapState.Warning:
                TrapAnim.SetTrigger("Warn");
                TrapAnim.ResetTrigger("Reset");

                m_TimeUntilNextStateChange = AttackWarningDuration;                   			break;
			case TrapState.Broken_WaitForFix:
                TrapAnim.SetTrigger("Activate");
                TrapAnim.ResetTrigger("Warn");

                break;
			
                default: Debug.Assert(false); break;
		}

        m_State = forceSetState;
	}

    ///////////////////////////////////////////////////////////////////////////

    public bool IsWithinAttackRange(Vector2 referencePos)
    {
        BoxCollider boxCollider         = GetComponent<BoxCollider>();
        SphereCollider sphereCollider   = GetComponent<SphereCollider>();

        float   y       = 0.0f;
        Collider collider; 

        if (boxCollider)
        {
            y = boxCollider.transform.position.y + boxCollider.center.y;
            collider = boxCollider;
        }
        else if (sphereCollider)
        {
			y = sphereCollider.transform.position.y + sphereCollider.center.y;
            collider = sphereCollider;
		}
        else
        {
            Debug.LogWarning(name.AddBrackets() + " does not have attack collider");
            return false;
        }

        Vector3 referencePos3D = referencePos.To3D(y);

        Vector3 closestPoint = collider.ClosestPoint(referencePos3D);
        
        bool isInside = (referencePos3D == closestPoint);

        return isInside;
	}

    ///////////////////////////////////////////////////////////////////////////

    public bool CanBeInteractedBy(PlayerController player)
    {
        if (!player.EquippedTool || ToolToFix != player.EquippedTool._ToolType)
        {
            Debug.LogWarning("Cannot fix " + name.AddBrackets() + " with " + (player.EquippedTool ? player.EquippedTool.name.AddBrackets() : " nothing in hand"));
            return false;
        }

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
            if (GetComponent<BoxCollider>())
            {
                BoxCollider boxCollider = GetComponent<BoxCollider>();
                Gizmos.DrawCube(boxCollider.bounds.center, boxCollider.bounds.size);
            }
            else
            {
                SphereCollider sphereCollider = GetComponent<SphereCollider>();
                Gizmos.DrawWireSphere(sphereCollider.bounds.center, sphereCollider.radius);
            }
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
        List<Walker> walkersInRange = WalkerManager.Get().GetAllWalkers(transform.position.xz(), this);

        foreach (Walker walker in walkersInRange)
        {
            walker.Kill();
        }
    }

    ///////////////////////////////////////////////////////////////////////////
}
