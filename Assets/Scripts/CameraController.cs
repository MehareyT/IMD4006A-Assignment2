using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameManager gameManager;
    public GameObject vcam1; //overview 
    public GameObject vcam2; //in control
    [SerializeField] GameObject hiddenClone;
    public GameObject cameraHolder;
    private PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsOverview())
        {
            if (vcam2.activeInHierarchy == true)
            {
                vcam2.SetActive(false);
            }
            vcam2.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = cameraHolder.transform;

        }

        if (gameManager.IsInControl())
        {
            if (vcam2.activeInHierarchy == false)
            {
                vcam2.SetActive(true);
            }
            else
            {
                Debug.Log("PlayerInput passed a null hiddenClone to camera controller");
            }

        }
    }

}

