using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceCanvas : MonoBehaviour
{
	public GameObject	ScreenSpaceTextPrefab;
	public GameObject	InteractIconPrefab;
	public float		Height			= 6.0f;
	public float		RiseSpeed		= 2.0f;
	public float		KillAtHeight	= 8.0f;
	public int			MaxAllowedTexts = 2;

	private static WorldSpaceCanvas s_Instance;

	public static WorldSpaceCanvas Get()
	{
		if (!s_Instance)
		{
			s_Instance = GameObject.FindObjectOfType<WorldSpaceCanvas>();
			s_Instance.ReInit();
		}

		return s_Instance;
	}

	void ReInit()
	{
		for (int c = 0; c < transform.childCount; ++c)
		{
			GameObject.Destroy(transform.GetChild(c));
		}
	}

	///////////////////////////////////////////////////////////////////////////

	public GameObject CreateInteractIcon()
	{
		GameObject newObj = GameObject.Instantiate(InteractIconPrefab, transform);
		return newObj;
	}

	///////////////////////////////////////////////////////////////////////////

	public void AddText(string text, Vector3 position)
	{
		GameObject newText = GameObject.Instantiate(ScreenSpaceTextPrefab, transform);

		newText.transform.position = position;
		newText.transform.SetPositionY(Height);
		newText.GetComponent<TextMeshProUGUI>().text = text;
	}

	private void Update()
	{
		Camera camera = Camera.main;
		transform.LookAt(transform.position - camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.up);

		for (int c = 0; c < transform.childCount; ++c)
		{
			Transform childTransform = transform.GetChild(c).transform;
			childTransform.SetPositionY(childTransform.position.y + RiseSpeed * Time.deltaTime);

			if (childTransform.position.y > KillAtHeight)
			{
				GameObject.Destroy(childTransform.gameObject);
			}
		}

		int childrenToDelete = transform.childCount - MaxAllowedTexts + 1;
		for (int c = 0; c < childrenToDelete; ++c)
		{
			GameObject.Destroy(transform.GetChild(c).gameObject);
		}
	}

	///////////////////////////////////////////////////////////////////////////
}
