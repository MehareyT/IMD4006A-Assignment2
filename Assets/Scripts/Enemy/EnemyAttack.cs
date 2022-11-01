using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public Attack[] attacks;
    private Attack currentAttack;
    int index = -1;
    float[] cooldowns;
    // Start is called before the first frame update
    void Awake()
    {
        cooldowns = new float[attacks.Length];
        for(int i = 0; i < cooldowns.Length; i++){
            cooldowns[i] = 0f;
        }
    }

    // Update is called once per frame
    void Update(){
        for(int i = 0; i < cooldowns.Length; i++){
            if(cooldowns[i] > 0f){
                cooldowns[i] -= Time.deltaTime;
            }
            
        }
    }

    public void Attack(Animator anim)
    {
        for(int i = 0; i < attacks.Length; i++){
            if(cooldowns[i] <= 0f){
                if(currentAttack == null || currentAttack.priority < attacks[i].priority){
                    currentAttack = attacks[i];
                    index = i;
                }
            }
        }
        if(currentAttack != null){
            //perform attack animation
            anim.SetTrigger(currentAttack.animationName);
            //add attack cooldown
            cooldowns[index] = currentAttack.cooldown;
            //reset current attack to null
            currentAttack = null;
            index = -1;
        }
        
    }
}
