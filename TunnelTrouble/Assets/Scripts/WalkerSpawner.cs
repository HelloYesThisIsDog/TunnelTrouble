using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerSpawner : MonoBehaviour
{
    public Walker   WalkerSpawnPrefab;
    public float    SpawnBurstInterval;
    public int      SpawnsPerBurst;
    public int      MaxSpawnCount   = int.MaxValue;
    public float    SpawnRadius     = 1.0f;

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

                float relativeSpawnOfBurst = i / (float)SpawnsPerBurst;

                SpawnWalker(relativeSpawnOfBurst);
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////

    public void SpawnWalker(float relativeSpawnOfBurst)
    {
        Walker walker = GameObject.Instantiate(WalkerSpawnPrefab, WalkerManager.Get().transform);
        walker.ID = m_SpawnedCount;
        walker.gameObject.name = "Walker " + m_SpawnedCount;

        Vector2 offset = Vector2.zero;
        offset.x = Mathf.Cos(relativeSpawnOfBurst * Mathf.PI * 2.0f);
        offset.y = Mathf.Sin(relativeSpawnOfBurst * Mathf.PI * 2.0f);

        walker.transform.position += offset.To3D(0.0f);

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
