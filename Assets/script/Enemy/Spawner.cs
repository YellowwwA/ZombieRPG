using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public PoolManager pool;
    public Transform[] spawnPoint;

    void Update()
    {
       if(GameObject.FindGameObjectsWithTag("enemy").Length <3)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = pool.Get(0);    //Random.Range(0,2);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        //pool.Get(1);
    }
}
