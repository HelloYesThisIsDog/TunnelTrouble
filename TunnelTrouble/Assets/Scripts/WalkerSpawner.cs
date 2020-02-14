using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerSpawner : MonoBehaviour
{
    public Walker   SmallWalkerSpawnPrefab;
    public Walker   BigWalkerSpawnPrefab;
    public float    SpawnBurstInterval;
    public int      SmallWalkersPerBurst;
    public int      BigWalkersPerBurst;
    public int      MaxSpawnCount   = int.MaxValue;
    public float    SpawnRadius     = 1.0f;
	public float	WaitBeforeFirstSpawn = 30.0f;

    private float   m_LastSpawnTime = float.NegativeInfinity;
    private int     m_SpawnedCount = 0;

    ///////////////////////////////////////////////////////////////////////////

    bool NeedsMoreWalkers()
    {
		bool enoughWalkersAlive     = transform.childCount >= WalkerManager.Get().MaxWalkerCount;
		bool enoughWalkersSpawned   = m_SpawnedCount >= MaxSpawnCount;

        return !enoughWalkersAlive && !enoughWalkersSpawned;
	}

    ///////////////////////////////////////////////////////////////////////////

    private void Update()
    {
		WaitBeforeFirstSpawn -= Time.deltaTime;
		WaitBeforeFirstSpawn = Mathf.Max(WaitBeforeFirstSpawn, 0);

		if (WaitBeforeFirstSpawn > 0)
		{
			return;
		}

        bool intervalFinished = Time.time - m_LastSpawnTime > SpawnBurstInterval;

        if (intervalFinished)
        {
            m_LastSpawnTime = Time.time;

            int totalSpawnCount = SmallWalkersPerBurst + BigWalkersPerBurst;

            for (int i = 0; i < totalSpawnCount; ++i)
            {
                if (!NeedsMoreWalkers())
                {
                    break;
                }

                bool spawnBigWalker = (i >= SmallWalkersPerBurst);

                float relativeSpawnOfBurst = i / (float)totalSpawnCount;

                SpawnWalker(relativeSpawnOfBurst, spawnBigWalker);
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////

    public void SpawnWalker(float relativeSpawnOfBurst, bool isBigWalker)
    {
        Walker walker = GameObject.Instantiate(isBigWalker ? BigWalkerSpawnPrefab : SmallWalkerSpawnPrefab, WalkerManager.Get().transform);
        walker.ID = m_SpawnedCount;
        walker.gameObject.name = "Walker " + m_SpawnedCount;

        Vector2 offset = Vector2.zero;
        offset.x = Mathf.Cos(relativeSpawnOfBurst * Mathf.PI * 2.0f);
        offset.y = Mathf.Sin(relativeSpawnOfBurst * Mathf.PI * 2.0f);

        walker.transform.position = transform.position + offset.To3D(0.0f);

        m_SpawnedCount++;
    }

    ///////////////////////////////////////////////////////////////////////////

	private void OnDrawGizmosSelected()
	{
		Color oldColor = Gizmos.color;
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(transform.position, SpawnRadius);

		Gizmos.color = oldColor;
	}

    ///////////////////////////////////////////////////////////////////////////

}
