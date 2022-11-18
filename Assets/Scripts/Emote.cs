using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Emote", menuName = "Emote")]
public class Emote : ScriptableObject
{

    ///<summary> The name of this emote. </summary>
    public new string name;

    ///<summary> The emoticon image. </summary>
    public Sprite emotion;

    ///<summary> The background image. </summary>
    public Sprite background;

    ///<summary> The chances this emote has of being shown. </summary>
    public float chance;

    ///<summary> The time until this emote can be shown again. </summary>
    public float cooldown;
    

}
