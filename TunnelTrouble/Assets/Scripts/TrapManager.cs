using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{

	private static TrapManager s_Instance;
	private List<Trap> m_Traps = new List<Trap>();

	///////////////////////////////////////////////////////////////////////////

	public static TrapManager Get()
	{
		if (!s_Instance)
		{
			s_Instance = GameObject.FindObjectOfType<TrapManager>();
			s_Instance.ReInit();
		}

		return s_Instance;
	}

	///////////////////////////////////////////////////////////////////////////

	void ReInit()
	{
		for (int c = 0; c < transform.childCount; ++c)
		{
			Transform child = transform.GetChild(c);
			Trap trap = child.gameObject.GetComponentInChildren<Trap>();

			if (trap)
			{
				m_Traps.Add(trap);
			}
		}
	}

	///////////////////////////////////////////////////////////////////////////

	private void Awake()
	{
		ReInit();
	}

	///////////////////////////////////////////////////////////////////////////

	public Trap GetNearestTrap(bool checkToolRequirement, Tool equippedTool, Vector2 referencePos, bool forceWithinRange, bool forceInteractable, Vector2? requireDirectionTowards)
	{
		Trap bestTrap = null;
		float bestDist = float.MaxValue;

		for (int c = 0; c < m_Traps.Count; ++c)
		{
			Trap curTrap = m_Traps[c];

			Vector2 trapPos = curTrap.transform.position.xz();
			
			float dist = (referencePos - trapPos).magnitude;

			if (forceWithinRange && dist > curTrap.InteractRadius)
			{
				continue;
			}

			if (dist > bestDist)
			{
				continue;
			}

			if (forceInteractable && !curTrap.CanBeInteractedBy(checkToolRequirement, equippedTool))
			{
				continue;
			}

			if (requireDirectionTowards.HasValue)
			{
				float dot = Vector2.Dot(trapPos - referencePos, requireDirectionTowards.Value);
				if (dot < 0)
				{
					continue;
				}
			}

			bestTrap = curTrap;
			bestDist = dist;
		}

		return bestTrap;
	}

	///////////////////////////////////////////////////////////////////////////

}
