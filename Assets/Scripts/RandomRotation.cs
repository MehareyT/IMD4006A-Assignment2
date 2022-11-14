using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float y = Random.Range(0f,360f);
        transform.rotation = Quaternion.Euler(new Vector3(0,y,0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
