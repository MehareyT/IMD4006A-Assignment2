using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenLeader : MonoBehaviour
{
    public GameObject unitLeader;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = unitLeader.transform.position;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));
    }
}
