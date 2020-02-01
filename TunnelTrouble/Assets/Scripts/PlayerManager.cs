using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////////////////////////////////////////////////////////////////

public class PlayerManager : MonoBehaviour
{

	///////////////////////////////////////////////////////////////////////////

	private static PlayerManager s_Instance;

	private Dictionary<PlayerSlot, PlayerController> m_Players = new Dictionary<PlayerSlot, PlayerController>();

	public static PlayerManager Get()
	{
		if (!s_Instance)
		{
			s_Instance = GameObject.FindObjectOfType<PlayerManager>();
			s_Instance.ReInit();
		}

		return s_Instance;
	}

	///////////////////////////////////////////////////////////////////////////

	void ReInit()
	{
		PlayerController[] controllers = GameObject.FindObjectsOfType<PlayerController>();

		foreach (var controller in controllers)
		{
			m_Players.Add(controller.Slot, controller);
		}

	}

	///////////////////////////////////////////////////////////////////////////
	
	public PlayerController GetPlayer(PlayerSlot slot)
	{
		if (!m_Players.ContainsKey(slot))
		{
			return null;
		}

		return m_Players[slot];
	}

	///////////////////////////////////////////////////////////////////////////
}
