using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerSpawner : MonoBehaviour
{
    public Walker   WalkerSpawnPrefab;
    public float    SpawnBurstInterval;
    public int      SpawnsPerBurst;

    private float   m_LastSpawnTime = float.NegativeInfinity;

    private void Update()
    {
        if (Time.time - m_LastSpawnTime > SpawnBurstInterval)
        {
            m_LastSpawnTime = Time.time;

            for (int i = 0; i < SpawnsPerBurst; ++i)
            {
                SpawnWalker();
            }
        }
    }

    public void SpawnWalker()
    {
        GameObject.Instantiate(WalkerSpawnPrefab, transform);
    }
}
