using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GibSFXScript : MonoBehaviour
{

    public AudioSource gibSound;

    private void Awake()
    {
        gibSound.pitch = Random.Range(0.925f, 1.075f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
