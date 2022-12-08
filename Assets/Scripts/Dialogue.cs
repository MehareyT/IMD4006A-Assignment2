using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    ///<summary> The text that will show in the dialogue. </summary>
    public string text;

    ///<summary> The audio clip that will play during the dialogue. </summary>
    public AudioClip audio;
    

}
