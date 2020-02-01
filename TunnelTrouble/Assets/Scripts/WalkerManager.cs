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
			GameObject.Destroy(transform.GetChild(c).gameObject);
		}
	}

	///////////////////////////////////////////////////////////////////////////


}
