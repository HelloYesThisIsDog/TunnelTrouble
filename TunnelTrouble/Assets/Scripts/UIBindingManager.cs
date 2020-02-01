using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBindingManager : MonoBehaviour
{
	public TextMeshProUGUI ToolTextField;

	private void Update()
	{
		PlayerController player1 = PlayerManager.Get().GetPlayer(PlayerSlot.Player1);
		
		if (player1.EquippedTool)
		{
			ToolTextField.text = player1.EquippedTool.name;
		}
		else
		{
			ToolTextField.text = "No Tool";
		}
	}
}
