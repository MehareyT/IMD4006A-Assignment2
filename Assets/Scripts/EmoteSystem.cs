using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmoteSystem : MonoBehaviour
{

    public Image background;
    public Image emote;
    public Animator emoteAnimator;
    public Emote[] emotes;
    public float[] cooldowns;
    int index = -1;

    public void Awake(){
        cooldowns = new float[emotes.Length];
        for(int i = 0; i < cooldowns.Length; i++){
            cooldowns[i] = 0f;
        }
        
    }

    void Update(){
        for(int i = 0; i < cooldowns.Length; i++){
            if(cooldowns[i] > 0f){
                cooldowns[i] -= Time.deltaTime;
            }
            
        }
    }
    

    public void Emote(string str){
        Emote emt = null;
        for(int i = 0; i < emotes.Length; i++){
            if(emotes[i].name == str){
                emt = emotes[i];
                index = i;
            }
        }
        if(emt == null){
            Debug.Log("Warning: There is no emote with the name "+str+".");
        }
        else if( Random.Range(0,100) <= emt.chance && cooldowns[index] <= 0.0f){
            emote.sprite = emt.emotion;
            background.sprite = emt.background;
            emoteAnimator.SetTrigger("Emote");
        }
        
    }


}
