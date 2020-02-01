using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerManager : MonoBehaviour
{
	public int MaxWalkerCount = 20;

	static WalkerManager s_Instance;

	public static WalkerManager Get()
	{
		if (!s_Instance)
		{
			s_Instance = GameObject.FindObjectOfType<WalkerManager>();
			s_Instance.ReInit();
		}

		return s_Instance;
	}

	///////////////////////////////////////////////////////////////////////////

	void ReInit()
	{
	}
	
	///////////////////////////////////////////////////////////////////////////

	int GetTotalWalkerCount()
	{
		return transform.childCount;
	}

	///////////////////////////////////////////////////////////////////////////

	private void Update()
	{
		
	}

	///////////////////////////////////////////////////////////////////////////

	public void KillAllWalkers()
	{
		for (int c = 0; c < transform.childCount; ++c)
		{
			transform.GetChild(c).gameObject.GetComponent<Walker>().Kill();
		}
	}

	///////////////////////////////////////////////////////////////////////////

	public Walker GetNearestWalker(Vector2 referencePos, float? forceWithinRange)
	{
		Walker bestWalker = null;
		float bestDist = float.MaxValue;

		for (int c = 0; c < transform.childCount; ++c)
		{
			Walker curWalker = transform.GetChild(c).gameObject.GetComponent<Walker>();

			Vector2 walkerPos = curWalker.transform.position.xz();

			float dist = (referencePos - walkerPos).magnitude;

			if (forceWithinRange.HasValue && dist > forceWithinRange.Value)
			{
				continue;
			}

			if (dist > bestDist)
			{
				continue;
			}

			bestWalker = curWalker;
			bestDist = dist;
		}

		return bestWalker;
	}

	///////////////////////////////////////////////////////////////////////////

	public List<Walker> GetAllWalkers(Vector2 referencePos, Trap withinAttackRange)
	{
		List<Walker> result = new List<Walker>();

		for (int c = 0; c < transform.childCount; ++c)
		{
			Walker curWalker = transform.GetChild(c).gameObject.GetComponent<Walker>();

			Vector2 walkerPos = curWalker.transform.position.xz();
			
			if (withinAttackRange)
			{
				if (!withinAttackRange.IsWithinAttackRange(walkerPos))
				{
					continue;
				}
			}

			result.Add(curWalker);
		}

		return result;
	}

	///////////////////////////////////////////////////////////////////////////


}
