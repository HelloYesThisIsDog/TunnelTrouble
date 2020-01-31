using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerSpawner : MonoBehaviour
{
    public Walker   WalkerSpawnPrefab;
    public float    SpawnBurstInterval;
    public int      SpawnsPerBurst;
    public int      MaxWalkerCount = 20;
    public int      MaxSpawnCount  = int.MaxValue;

    private float   m_LastSpawnTime = float.NegativeInfinity;
    private int     m_SpawnedCount = 0;

    bool NeedsMoreWalkers()
    {
		bool enoughWalkersAlive = transform.childCount >= MaxWalkerCount;
		bool enoughWalkersSpawned = m_SpawnedCount >= MaxSpawnCount;

        return !enoughWalkersAlive && !enoughWalkersSpawned;
	}

    private void Update()
    {
        bool intervalFinished = Time.time - m_LastSpawnTime > SpawnBurstInterval;

        if (intervalFinished)
        {
            m_LastSpawnTime = Time.time;

            for (int i = 0; i < SpawnsPerBurst; ++i)
            {
                if (!NeedsMoreWalkers())
                {
                    break;
                }
                SpawnWalker();
            }
        }
    }

    public void KillAllWalkers()
    {
		for (int c = 0; c < transform.childCount; ++c)
		{
			GameObject.Destroy(transform.GetChild(c).gameObject);
        }
    }

    public void SpawnWalker()
    {
        Walker walker = GameObject.Instantiate(WalkerSpawnPrefab, transform);
        walker.gameObject.name = "Walker " + m_SpawnedCount;

        m_SpawnedCount++;
    }
}
