using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBindingManager : MonoBehaviour
{
	public TextMeshProUGUI	ToolTextField;
	public TextMeshProUGUI	ScoreTextField;
	public TextMeshProUGUI	CountDownTextfield;
	public GameObject		GameOverScreen;
	public TextMeshProUGUI	GameOverTextfield;

	private void Update()
	{
		ToolTextField.text = "";

		for (int i = 0; i < (int)PlayerSlot.PlayerCount; ++i)
		{
			PlayerController player = PlayerManager.Get().GetPlayer((PlayerSlot)i);

			if (!player)
			{
				continue;
			}

			if (ToolTextField.text != "")
			{
				ToolTextField.text += "\n";
			}

			if (player.EquippedTool)
			{
				ToolTextField.text += "P" + (i + 1) + ": " + player.EquippedTool.name;
			}
			else
			{
				ToolTextField.text += "P" + (i + 1) + ": " + "No Tool";
			}
		}
		
		int rescuedWalkers = GameManager.Get().m_RescuedWalkers;

		ScoreTextField.text = "Rescued: " + rescuedWalkers;
		GameOverTextfield.text = "Rescued:\n" + rescuedWalkers;

		float runningTime	= GameManager.Get().m_RunningTime;
		float gameDuration	= GameManager.Get().m_GameDuration;

		float timeLeft = Mathf.Max(gameDuration - runningTime, 0);
		
		int minutes = ((int)timeLeft) / 60;
		int seconds = ((int)timeLeft) - minutes * 60;

		CountDownTextfield.text = minutes + ":" + seconds.ToString("00");

		GameOverScreen.SetActive(GameManager.Get().m_GameEnded);
	}
}
