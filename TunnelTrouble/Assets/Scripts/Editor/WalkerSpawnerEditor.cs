using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WalkerSpawner))]
public class WalkerSpawnerEditor : Editor
{
	///////////////////////////////////////////////////////////////////////////

	public override void OnInspectorGUI()
	{
		WalkerSpawner walkerSpawner = (WalkerSpawner)target;

		DrawDefaultInspector();

		// We only want to modify Scene data during play mode
		GUI.enabled = Application.isPlaying;

		if (GUILayout.Button("KillAll"))
		{
			walkerSpawner.KillAllWalkers();
		}

		if (GUILayout.Button("Spawn Single"))
		{
			walkerSpawner.SpawnWalker(0.0f);
		}

		if (GUILayout.Button(walkerSpawner.enabled ? "Stop Spawning" : "Resume Spawning"))
		{
			walkerSpawner.enabled = !walkerSpawner.enabled;
		}

		GUI.enabled = true;
	}

	///////////////////////////////////////////////////////////////////////////
}
