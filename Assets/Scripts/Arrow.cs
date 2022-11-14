using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform enemy;

    public Transform player;

    public GroupManager groupManager;



    void Update()
    {
        if(groupManager.activeGroup.hiddenLeader == null){
            
        }
        else{
            player = groupManager.activeGroup.hiddenLeader;
            Vector3 relativePos =  enemy.position -  player.position;

            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y,rotation.eulerAngles.y);
        }
        
    }
}
