using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    public Transform target;
    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.position += (target.position - transform.position).normalized * speed * Time.deltaTime;
        transform.LookAt(target);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy"){
            //damage
            Destroy(gameObject, 0.3f);
        }

    }
}
