using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerAttack : MonoBehaviour
{
    public GameObject attack;
    float curCooldown;
    public float cooldown;

    // Update is called once per frame
    void Update(){
        
        if(curCooldown > 0f){
            curCooldown -= Time.deltaTime;
        }
    }

    public void Attack(Transform target)
    {
        if(curCooldown <= 0f){
            Projectile.Spawn(attack,transform.position, Quaternion.identity, target);
            //var obj = Instantiate(attack, transform.position, Quaternion.identity);
            //obj.GetComponent<ProjectileAttack>().target = target;
            curCooldown = cooldown;
        }
        
    }
}
