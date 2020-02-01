using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WalkerManager))]
public class WalkerManagerEditor : Editor
{
	///////////////////////////////////////////////////////////////////////////

	public override void OnInspectorGUI()
	{
		WalkerManager walkerManager = (WalkerManager)target;

		DrawDefaultInspector();

		// We only want to modify Scene data during play mode
		GUI.enabled = Application.isPlaying;

		if (GUILayout.Button("KillAll"))
		{
			walkerManager.KillAllWalkers();
		}

		if (GUILayout.Button("Spawn Small"))
		{
			WalkerSpawner spawner = GameObject.FindObjectOfType<WalkerSpawner>();
			WalkerSpawnerEditor.SpawnSingle(spawner, false);
		}

		if (GUILayout.Button("Spawn Big"))
		{
			WalkerSpawner spawner = GameObject.FindObjectOfType<WalkerSpawner>();
			WalkerSpawnerEditor.SpawnSingle(spawner, true);
		}

		if (GUILayout.Button("Spawn Multiple"))
		{
			WalkerSpawner[] spawners = GameObject.FindObjectsOfType<WalkerSpawner>();

			for (int s = 0; s < spawners.Length; ++s)
			{
				WalkerSpawnerEditor.SpawnMulti(spawners[s]);
			}
		}

		if (GUILayout.Button(walkerManager.enabled ? "Stop Spawning" : "Resume Spawning"))
		{
			WalkerSpawner[] spawners = GameObject.FindObjectsOfType<WalkerSpawner>();

			for (int s = 0; s < spawners.Length; ++s)
			{
				spawners[s].enabled = !spawners[s].enabled;
			}	
		}

		GUI.enabled = true;
	}

	///////////////////////////////////////////////////////////////////////////

	

	///////////////////////////////////////////////////////////////////////////
}
