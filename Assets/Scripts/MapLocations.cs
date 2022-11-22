using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocations : MonoBehaviour
{

    /// <summary> Information about villager base locations </summary>
    struct VillagerBase
    {
        public int baseID;
        public int currentPop;
        public int maxPop;
        public bool isDestoryed;
        public bool isSpawning;

        public Transform baseLocation;

        public VillagerBase(Transform newBase, int ID)
        {
            this.baseID = ID;
            this.baseLocation = newBase;
            this.isDestoryed = false;
            this.isSpawning = false;
            this.currentPop = 0;
            maxPop = 3;
        }

    }

    /// <summary> The enemy the units will use to check distance </summary>
    public Transform enemy;

    /// <summary> List of map locations </summary>
    public List<Transform> locations = new();

    /// <summary> Array of map locations </summary>
    private VillagerBase[] villagerBases;

    /// <summary> Spawn rate of villagers at bases </summary>
    [SerializeField] private float spawnRate;

    public GameObject spawnVillager;

    public int totalPop;

    /// <summary> Total Max Popluation of villager at any given time </summary>
    [SerializeField] private float maxTotalPop;

    private void Awake()
    {
        enemy = GameObject.FindGameObjectsWithTag("Enemy")[0].transform;
    }

    void Start()
    {
        villagerBases = new VillagerBase[locations.Count];

        for(int i = 0; i < locations.Count; i++)
        {
            villagerBases[i] = new VillagerBase(locations[i], i);
            StartCoroutine(SpawnVillagers(villagerBases[i]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        totalPop = GameObject.FindGameObjectsWithTag("Player").Length;
        //Debug.Log("Current total population: " + totalPop);

    }

    private IEnumerator SpawnVillagers(VillagerBase spawnFrom)
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate * Random.Range(0.5f, 1.5f));

        List<GameObject> baseVillagers = new();

        while (enabled)
        {
            yield return wait;
            if (totalPop < maxTotalPop)
            {

                //Debug.Log("Spawn Villager");

                //Instantiate in a ring around the baseTransform
                baseVillagers.Add(GameObject.Instantiate(spawnVillager, locations[spawnFrom.baseID].position + new Vector3(Random.onUnitSphere.x, 1.25f,Random.onUnitSphere.z), Quaternion.Euler(Vector3.zero)));

                spawnFrom.currentPop += 1;
                villagerBases[spawnFrom.baseID].currentPop = spawnFrom.currentPop;
            }
            else
            {
                //Debug.Log("No Villager Spawned");
            }

            //Debug.Log("Current total population: " + GameObject.FindGameObjectsWithTag("Player").Length);

            foreach (GameObject vilCheck in baseVillagers)
            {
                if(Vector3.Distance(locations[spawnFrom.baseID].position, vilCheck.transform.position) > 10.0f)
                {
                    //villagerBases[spawnFrom.baseID].maxPop = spawnFrom.maxPop;
                    //Debug.Log("Villager Left :(");
                }
            }



        }
    }


}
