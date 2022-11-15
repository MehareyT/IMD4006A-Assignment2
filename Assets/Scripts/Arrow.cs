using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform enemy;

    public Transform player;

    public Transform scalePivot;

    public Animator head;

    public GroupManager groupManager;

    public float hideArrowRange = 1f;

    float distance = 0f;



    void Update()
    {
        if(groupManager.activeGroup.hiddenLeader == null){
            distance = 0f;
            head.SetBool("Anger", false);
        }
        else{
            player = groupManager.activeGroup.hiddenLeader;
            Vector3 relativePos =  enemy.position -  player.position;
            distance = 1.5f - (Vector3.Distance(enemy.position,player.position)/100f);
            if(distance > hideArrowRange){
                distance = 0.0f;
                head.SetBool("Anger", true);
            }
            else{
                head.SetBool("Anger", false);
            }
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y,rotation.eulerAngles.y);
        }
        scalePivot.localScale = new Vector3(distance,distance,distance);
    }
}
