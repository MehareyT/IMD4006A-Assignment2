using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cameraTransform;
    // Update is called once per frame
    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }
    void LateUpdate()
    {
        transform.LookAt(transform.position + cameraTransform.forward);
    }
}
