using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{

    public Animator animator;
    public TMP_Text text;
    public AudioSource audio;
    public float cooldown;
    float tempCooldown;
    // Start is called before the first frame update
    public bool PlayDialogue(Dialogue dialogue)
    {
        if(tempCooldown <= 0){
            audio.PlayOneShot(dialogue.audio);
            text.text = dialogue.text;
            animator.SetBool("Show",true);
            tempCooldown = cooldown;
            return true;
        }
        return false;      

    }

    void Update(){
        if(tempCooldown > 0){
            tempCooldown -= Time.deltaTime;
        }
        else if(animator.GetBool("Show")){
            animator.SetBool("Show",false);
        }
    }
}
