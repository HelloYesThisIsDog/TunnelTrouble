using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTrolley : MonoBehaviour
{
	private static ToolTrolley s_Instance;

	///////////////////////////////////////////////////////////////////////////

	public static ToolTrolley Get()
	{
		if (!s_Instance)
		{
			s_Instance = GameObject.FindObjectOfType<ToolTrolley>();
			s_Instance.ReInit();
		}

		return s_Instance;
	}

	///////////////////////////////////////////////////////////////////////////

	void ReInit()
	{
		
	}

	///////////////////////////////////////////////////////////////////////////
}
