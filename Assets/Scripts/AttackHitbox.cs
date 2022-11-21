using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public GameObject deathEffect;
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            //damage
            Instantiate(deathEffect,other.transform.position,other.transform.rotation);
            Destroy(other.gameObject);
        }


    }
}
