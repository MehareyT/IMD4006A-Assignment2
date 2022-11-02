using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public Enemy enemy;
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            //damage
            enemy.population -= 1;
            Destroy(other.gameObject);
        }


    }
}
