using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGroup : MonoBehaviour
{
    public GameObject leader;
    public List<GameObject> units = new List<GameObject>();
    public UnitGroup(GameObject leader) {
        this.leader = leader;
        units.Add(leader);
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
