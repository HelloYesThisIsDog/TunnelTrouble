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
	public float	m_GameEndedRealtime	= 0.0f;

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

		bool doRestart = false;

		if (m_GameEnded && Time.realtimeSinceStartup > m_GameEndedRealtime + 1.0f)
		{
			doRestart  = Input.GetButtonDown("P1 Interact");
			doRestart |= Input.GetButtonDown("P2 Interact");
			doRestart |= Input.GetButtonDown("P3 Interact");
			doRestart |= Input.GetButtonDown("P4 Interact");
		}

		if (Input.GetKeyDown(KeyCode.F5) || 
			Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.Joystick2Button4) || Input.GetKeyDown(KeyCode.Joystick3Button4))
		{
			doRestart = true;
		}

		if (doRestart)
		{ 
			Time.timeScale = 1.0f;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);

			ToggleInfo.s_HackCloseOnceAfterRestart = true;
		}
	}

	///////////////////////////////////////////////////////////////////////////

	void EndGame()
	{
		Time.timeScale = 0.0f;
		m_GameEnded = true;
		m_GameEndedRealtime = Time.realtimeSinceStartup;
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
