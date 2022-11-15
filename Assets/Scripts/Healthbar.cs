using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public bool useXAxis = true;
    float x = 1f;
    public float current;
    public float max;
    // Update is called once per frame
    void Update()
    {
        x = current / max;
        //prevent reverse barr
        if(x <= 0){
            x = 0;
        }

        if(useXAxis){
            transform.localScale = new Vector3(x,1f,1f);
        }
        else{
            transform.localScale = new Vector3(1f,x,1f);
        }
        
    }
}
