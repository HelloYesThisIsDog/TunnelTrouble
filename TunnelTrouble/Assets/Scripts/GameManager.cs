using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	static GameManager s_Instance;

	public float	m_GameDuration		= 120.0f;

	[Header("Debug")]
	public int		m_RescuedWalkers	= 0;
	public float	m_RunningTime		= 0.0f;

	public static GameManager Get()
	{
		if (!s_Instance)
		{
			s_Instance = GameObject.FindObjectOfType<GameManager>();
			s_Instance.ReInit();
		}

		return s_Instance;
	}

	///////////////////////////////////////////////////////////////////////////

	private void Update()
	{
		m_RunningTime += Time.deltaTime;

		if (m_RunningTime >= m_GameDuration)
		{
			EndGame();
		}
	}

	///////////////////////////////////////////////////////////////////////////

	void EndGame()
	{
		Time.timeScale = 0.0f;
		WorldSpaceCanvas.Get().AddText("GAME OVER", Vector3.zero);
	}

	///////////////////////////////////////////////////////////////////////////

	void ReInit()
	{
	}

	///////////////////////////////////////////////////////////////////////////
}
