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
			WalkerManager.Get().KillAllWalkers();
		}

		if (GUILayout.Button("Spawn Smal"))
		{
			SpawnSingle(walkerSpawner, false);
		}

		if (GUILayout.Button("Spawn Big"))
		{
			SpawnSingle(walkerSpawner, true);
		}

		if (GUILayout.Button("Spawn Multiple"))
		{
			for (int i = 0; i < 20; ++i)
			{
				SpawnMulti(walkerSpawner);
			}
		}

		if (GUILayout.Button(walkerSpawner.enabled ? "Stop Spawning" : "Resume Spawning"))
		{
			walkerSpawner.enabled = !walkerSpawner.enabled;
		}

		GUI.enabled = true;
	}

	///////////////////////////////////////////////////////////////////////////

	public static void SpawnSingle(WalkerSpawner spawner, bool isBig)
	{
		spawner.SpawnWalker(0.0f, isBig);
	}

	///////////////////////////////////////////////////////////////////////////

	public static void SpawnMulti(WalkerSpawner spawner)
	{
		for (int i = 0; i < 20; ++i)
		{
			spawner.SpawnWalker(i / 20.0f, (i == 0));
		}
	}

	///////////////////////////////////////////////////////////////////////////
}
