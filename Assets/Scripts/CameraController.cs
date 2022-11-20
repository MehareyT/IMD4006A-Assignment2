using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
    public GameObject groupTarget;
    public float targetRadius = 0.5f;
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
            overviewCameraHolder.transform.position = groupTarget.transform.position;

            List<CinemachineTargetGroup.Target> targList = new List<CinemachineTargetGroup.Target>();
            CinemachineTargetGroup.Target temp;
            temp.target = groupManager.activeGroup.hiddenLeader;
            temp.radius = targetRadius;
            temp.weight = 3;
            targList.Add(temp);
            foreach (GameObject item in groupManager.activeGroup.units)
            {
                CinemachineTargetGroup.Target t;
                t.radius = targetRadius;
                t.target = item.transform;
                t.weight = 1; 

                targList.Add(t);
            }
          

            CinemachineTargetGroup.Target[] targets = targList.ToArray();  

            groupTarget.GetComponent<CinemachineTargetGroup>().m_Targets = targets;

            //inControlCamerHolder.transform.position = groupManager.GetActiveGroup().hiddenLeader.position;
        }
    }

}

