using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class ShipCreator : MonoBehaviour
{
    [SerializeField] protected Transform[] SpawnPoints;
    [SerializeField] protected ShipAbstract[] Ships;

    private DateTime lastSpawnedAt;

    private bool inited;
    private float spawnRate;

    public void Init(float spawnRate)
    {
        inited = true;
        this.spawnRate = spawnRate;
        
        lastSpawnedAt = DateTime.UtcNow.AddSeconds(spawnRate);
    }

    private void Update()
    {
        if(!inited || lastSpawnedAt.AddSeconds(spawnRate) > DateTime.UtcNow)
            return;
        
        SpawnShip();
    }

    protected virtual void SpawnShip()
    {
        lastSpawnedAt = DateTime.UtcNow;
        
        var chosenPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        var chosenShip = Ships[Random.Range(0, Ships.Length)];

        var ship = Instantiate(chosenShip, chosenPoint.position, Quaternion.identity).GetComponent<ShipAbstract>();

        ship.Init(.001f, 10, chosenPoint.rotation);
    }

    public virtual void Deinit()
    {
        inited = false;
    }
    
}
