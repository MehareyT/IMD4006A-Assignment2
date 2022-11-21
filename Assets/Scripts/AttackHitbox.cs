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
            Instantiate(deathEffect,other.transform.position,other.transform.rotation);
            Destroy(other.gameObject);
        }


    }
}
