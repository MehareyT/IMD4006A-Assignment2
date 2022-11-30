using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public GameObject deathEffect;
    public PopulationManager popManager;
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            //damage
            popManager.Hurt(1);
            var obj = Instantiate(deathEffect,other.transform.position,other.transform.rotation);
            Destroy(obj,50f);
            Destroy(other.gameObject);
        }


    }
}
