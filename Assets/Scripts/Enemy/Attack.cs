using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Attack", menuName = "Enemy Attack")]
public class Attack : ScriptableObject
{
    ///<summary> The time in seconds till the enemy can use this attack again. </summary>
    public float cooldown;

    ///<summary> The string of the attack trigger in the animator. </summary>
    public string animationName;

    ///<summary> The amount of damage the attack will deal to anything it hits. </summary>
    public int damage;

    ///<summary> The priority the enemy will use this attack. The higher number the more likely. </summary>
    public int priority;
    

}
