using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapspawner : MonoBehaviour
{
    public Transform TrapsContainer;
    public Transform[] SpawnableTraps;

    public Transform[] SpawnPoints;

    public float SpawnIntervallTime=2;
    private TrapManager TM;
    public float TimeToIncreaseSpawnrate =30;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("spawnATrap",SpawnIntervallTime, SpawnIntervallTime);
        TM = TrapManager.Get();
        InvokeRepeating("invokeMoreSpawns", TimeToIncreaseSpawnrate, TimeToIncreaseSpawnrate);

    }

    public void spawnATrap()
    {
        
        int PointID =  Random.Range(0, SpawnPoints.Length);
        int TrapID = Random.Range(0,SpawnableTraps.Length);
        TM.m_Traps.Add(Instantiate(SpawnableTraps[TrapID], SpawnPoints[PointID].position, Quaternion.identity, TrapsContainer).GetComponent<Trap>());
       
    }
    public void invokeMoreSpawns()
    {
        InvokeRepeating("spawnATrap", Random.Range(0,3), SpawnIntervallTime+ Random.Range(0, 2));


    }
}
