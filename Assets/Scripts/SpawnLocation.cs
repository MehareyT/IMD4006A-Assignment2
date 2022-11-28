using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{

    public int maximumPoplation;
    public int currentlySpawned;
    public float spawnRadius;
    public float spawnRate;
    public float spawnChance;
    public int maxSpawnNum;
    [HideInInspector]public int totalSpawnNum;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

}
