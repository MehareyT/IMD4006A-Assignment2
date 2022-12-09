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

    float counting = 0;

    float countingToo = 30;

    float cooldown = 0;
    float cooldown2 = 0;
    bool doTheThing = false;
    bool doTheThing2 = false;

    DialogueSystem dialogueSystem;

    public Dialogue farMonster1;
    public Dialogue farMonster2;

    public Dialogue tooScared1;
    public Dialogue tooScared2;

    public Dialogue perfectGroup1;
    public Dialogue perfectGroup2;

    void Awake(){
        dialogueSystem = GameObject.Find("Dialogue").GetComponent<DialogueSystem>();
    }



    void Update()
    {
        if(cooldown > 0){
            cooldown -= Time.deltaTime;
        }
        if(groupManager.activeGroup.hiddenLeader == null){
            distance = 0f;
            head.SetBool("Anger", false);
        }
        else{
            player = groupManager.activeGroup.hiddenLeader;
            Vector3 relativePos =  enemy.position -  player.position;
            distance = 1.5f - (Vector3.Distance(enemy.position,player.position)/100f);
            if(Mathf.Abs(Vector3.Distance(enemy.position,player.position)) > 100f){
                counting += Time.deltaTime;
                if(counting >= countingToo){
                    counting = 0;
                    countingToo = countingToo *2;
                    if(Random.Range(0,100) > 50){
                        var f = dialogueSystem.PlayDialogue(farMonster1);
                    }
                    else{
                        var f = dialogueSystem.PlayDialogue(farMonster2);
                    }
                }
            }
            if(distance > hideArrowRange){
                distance = 0.0f;
                head.SetBool("Anger", true);
                if(groupManager.activeGroup.units.Count < 5){
                    if(cooldown <= 0 && !doTheThing){
                        if(Random.Range(0,100) > 50){
                            doTheThing = dialogueSystem.PlayDialogue(tooScared1);
                        }
                        else{
                            doTheThing = dialogueSystem.PlayDialogue(tooScared2);
                        }
                    }
                    if(doTheThing){
                        cooldown = 60;
                    }
                }
                counting = 0;
                if(groupManager.activeGroup.units.Count >= 10 && cooldown2 <= 0){
                    cooldown2 = 30;
                }
            }
            else{
                head.SetBool("Anger", false);
                if(groupManager.activeGroup.units.Count >= 10){
                    if(cooldown2 <= 0 && !doTheThing2){
                        if(Random.Range(0,100) > 50){
                            doTheThing2 = dialogueSystem.PlayDialogue(perfectGroup1);
                        }
                        else{
                            doTheThing2 = dialogueSystem.PlayDialogue(perfectGroup2);
                        }
                    }
                    if(doTheThing2){
                        cooldown2 = 60;
                    }
                }

            }
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y,rotation.eulerAngles.y);
        }
        scalePivot.localScale = new Vector3(distance,distance,distance);
    }
}
