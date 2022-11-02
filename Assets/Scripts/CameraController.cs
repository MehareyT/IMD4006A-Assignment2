using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GroupManager groupManager;
    GameManager gameManager;
    public GameObject vcam1; //overview 
    public GameObject vcam2; //in control
    [SerializeField] GameObject hiddenClone;
    public GameObject overviewCameraHolder;
    public GameObject inControlCamerHolder;
    private PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        groupManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GroupManager>();
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
            inControlCamerHolder.transform.position = overviewCameraHolder.transform.position;
        }

        if (gameManager.IsInControl())
        {
            if (vcam2.activeInHierarchy == false)
            {
                vcam2.SetActive(true);
            }
            else
            {
                //Debug.Log("PlayerInput passed a null hiddenClone to camera controller");
            }
            overviewCameraHolder.transform.position = inControlCamerHolder.transform.position;
            inControlCamerHolder.transform.position = groupManager.GetActiveGroup().hiddenLeader.position;
        }
    }

}

