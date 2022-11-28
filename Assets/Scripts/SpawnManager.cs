using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnManager : MonoBehaviour
{



    /// <summary> List of spawner nodes </summary>
    public List<GameObject> locations = new();

    /// <summary> The villager prefab to spawn </summary>
    public GameObject spawnVillager;

    /// <summary> The manager of the population numbers </summary>
    private PopulationManager popManager;


    void Start()
    {
        locations = GameObject.FindGameObjectsWithTag("VillagerBase").ToList();
        popManager = GetComponent<PopulationManager>();
        for(int i = 0; i < 23; i++)
        {
            StartCoroutine(SpawnVillagers(locations[i].GetComponent<SpawnLocation>()));
        }
    }

    private IEnumerator SpawnVillagers(SpawnLocation location)
    {
        WaitForSeconds wait = new WaitForSeconds(location.spawnRate * Random.Range(0.5f, 1.5f));

        List<GameObject> baseVillagers = new();

        while (enabled)
        {
            yield return wait;
            if (location.totalSpawnNum < location.maxSpawnNum && Random.Range(0, 100) <= location.spawnChance && popManager.currentPop < popManager.maxPopulation)
            {

                float radius = location.spawnRadius;
                //Instantiate in a ring around the baseTransform
                baseVillagers.Add(GameObject.Instantiate(spawnVillager, location.transform.position + new Vector3(Random.onUnitSphere.x * radius, 1.25f,Random.onUnitSphere.z * radius), Quaternion.Euler(Vector3.zero)));

                location.totalSpawnNum += 1;
                
            }

        }
    }


}
