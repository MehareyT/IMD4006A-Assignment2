using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnManager : MonoBehaviour
{



    /// <summary> List of map locations </summary>
    public List<GameObject> locations = new();

    public GameObject spawnVillager;

    public int totalPop;

    /// <summary> Total Max Popluation of villager at any given time </summary>
    [SerializeField] private float maxTotalPop;


    void Start()
    {
        locations = GameObject.FindGameObjectsWithTag("VillagerBase").ToList();
        
        for(int i = 0; i < 23; i++)
        {
            StartCoroutine(SpawnVillagers(locations[i].GetComponent<SpawnLocation>()));
        }
    }

    // Update is called once per frame
    void Update()
    {
        totalPop = GameObject.FindGameObjectsWithTag("Player").Length;

    }

    private IEnumerator SpawnVillagers(SpawnLocation location)
    {
        WaitForSeconds wait = new WaitForSeconds(location.spawnRate * Random.Range(0.5f, 1.5f));

        List<GameObject> baseVillagers = new();

        while (enabled)
        {
            yield return wait;
            if (location.totalSpawnNum < location.maxSpawnNum && Random.Range(0, 100) <= location.spawnChance && totalPop < maxTotalPop)
            {

                float radius = location.spawnRadius;
                //Instantiate in a ring around the baseTransform
                baseVillagers.Add(GameObject.Instantiate(spawnVillager, location.transform.position + new Vector3(Random.onUnitSphere.x * radius, 1.25f,Random.onUnitSphere.z * radius), Quaternion.Euler(Vector3.zero)));

                location.totalSpawnNum += 1;
                
            }

        }
    }


}
