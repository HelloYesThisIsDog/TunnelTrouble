using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////////////////////////////////////////////////////////////////

public class PlayerManager : MonoBehaviour
{

	///////////////////////////////////////////////////////////////////////////

	private static PlayerManager s_Instance;

	public PlayerController PlayerPrefab;

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

	private void Update()
	{
		if (!m_Players.ContainsKey(PlayerSlot.Player1) && Input.GetButtonDown("P1 Interact")) { AddPlayer(PlayerSlot.Player1); }
		if (!m_Players.ContainsKey(PlayerSlot.Player2) && Input.GetButtonDown("P2 Interact")) { AddPlayer(PlayerSlot.Player2); }
		if (!m_Players.ContainsKey(PlayerSlot.Player3) && Input.GetButtonDown("P3 Interact")) { AddPlayer(PlayerSlot.Player3); }
		if (!m_Players.ContainsKey(PlayerSlot.Player4) && Input.GetButtonDown("P4 Interact")) { AddPlayer(PlayerSlot.Player4); }
	}

	///////////////////////////////////////////////////////////////////////////

	private void AddPlayer(PlayerSlot playerSlot)
	{
		GameObject newObject = GameObject.Instantiate(PlayerPrefab.gameObject, transform);
		newObject.transform.position = transform.position;
		newObject.name = "Player " + ((int)playerSlot + 1);

		PlayerController controller = newObject.GetComponent<PlayerController>();

		m_Players.Add(playerSlot, controller);

		controller.Slot = playerSlot;
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
