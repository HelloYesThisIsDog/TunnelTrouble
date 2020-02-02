using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	static GameManager s_Instance;

	public float	m_GameDuration		= 120.0f;

	[Header("Debug")]
	public int		m_RescuedWalkers	= 0;
	public float	m_RunningTime		= 0.0f;
	public bool		m_GameEnded			= false;

	public float	LastGameDurationBonusTimestamp  = 0.0f;
	public float	LastGameDurationBonus			= 0.0f;

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

		if (!m_GameEnded && m_RunningTime >= m_GameDuration)
		{
			EndGame();
		}

		if (m_GameEnded && m_RunningTime > m_GameDuration + 1.0f)
		{
			bool needrestart  = Input.GetButtonDown("P1 Interact");
				 needrestart |= Input.GetButtonDown("P2 Interact");
				 needrestart |= Input.GetButtonDown("P3 Interact");
				 needrestart |= Input.GetButtonDown("P4 Interact");

			if (needrestart)
			{
				Time.timeScale = 1.0f;
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}

		if (Input.GetKeyDown(KeyCode.F5))
		{
			Time.timeScale = 1.0f;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	///////////////////////////////////////////////////////////////////////////

	void EndGame()
	{
		Time.timeScale = 0.0f;
		m_GameEnded = true;
		AudioManager.Get().PlayGameOverMusic();
	}

	///////////////////////////////////////////////////////////////////////////

	void ReInit()
	{
	}

	///////////////////////////////////////////////////////////////////////////
	public static float BONUS_COLLECTING_TIME = 0.5f;

	public void AddTimeBonus(float time)
	{
		m_GameDuration += time;

		if (Time.time - LastGameDurationBonusTimestamp > BONUS_COLLECTING_TIME)
		{
			LastGameDurationBonus  = 0.0f;
		}

		LastGameDurationBonus += time;

		LastGameDurationBonusTimestamp = Time.time;

	}

	///////////////////////////////////////////////////////////////////////////
}
