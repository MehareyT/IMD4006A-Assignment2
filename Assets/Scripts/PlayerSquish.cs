using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSquish : MonoBehaviour
{

    public Animator anim;

   void OnCollisionEnter(Collision collision)
    {
        anim.SetTrigger("Squish");
    }
}
