using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    GameManager gameManager;
    GroupManager groupManager;
    [SerializeField]
    private InputActionAsset inputActions;
    private InputActionMap leaderActionMap;
    private InputAction movement;
    private InputAction mousePositionAction;
    private InputAction denyAction;
    private InputAction contextAction;


    [SerializeField]
    private GameObject cam;
    private NavMeshAgent agent;
    [SerializeField]
    [Range(0, 0.99f)]
    private float smoothing = 0.25f;

    private Vector3 targetDirection;
    private float lerpTime = 0;
    [SerializeField]
    [Range(0, 100)]
    private float targetLerpSpeed = 1;
    private Vector3 LastDirection;
    private Vector3 movementVector;
    public GameObject unit;
    public GameObject hiddenLeader;
    public LayerMask unitMask;

    Vector2 mousePos = new Vector2();



    private void Awake()
    {
        if (unit)
        {
            agent = unit.GetComponent<NavMeshAgent>();
        }

        
        leaderActionMap = inputActions.FindActionMap("Leader");
        movement = leaderActionMap.FindAction("Move");
        movement.started += HandleMovementAction;
        movement.performed += HandleMovementAction;
        movement.canceled += HandleMovementAction;
        movement.Enable();


        mousePositionAction = leaderActionMap.FindAction("MousePosition");
        mousePositionAction.started += HandleMousePosition;
        mousePositionAction.performed += HandleMousePosition;
        mousePositionAction.canceled += HandleMousePosition;
        mousePositionAction.Enable();

        denyAction = leaderActionMap.FindAction("Deny");
        denyAction.started += HandleDenyAction;
        denyAction.performed += HandleDenyAction;
        denyAction.canceled += HandleDenyAction;

        contextAction = leaderActionMap.FindAction("ContextAction");
        contextAction.started += HandleContextAction;
        contextAction.performed += HandleContextAction;
        contextAction.canceled += HandleContextAction;


        leaderActionMap.Enable();
        inputActions.Enable();
    }

    private void HandleContextAction(InputAction.CallbackContext obj)
    {
        if (gameManager.IsInControl())
        {
            unit.GetComponent<Villager>().Rally();
        }
    }

    private void HandleDenyAction(InputAction.CallbackContext obj)
    {
        Deny();
    }

    private void HandleMousePosition(InputAction.CallbackContext obj)
    {
        mousePos = obj.ReadValue<Vector2>();
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        groupManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GroupManager>();
        Debug.Assert(gameManager);
        Debug.Assert(groupManager);
    }
    private void HandleMovementAction(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        movementVector = new Vector3(input.x, 0, input.y);
    }



    private void Update()
    {
        if (unit)
        {
            agent = unit.GetComponent<NavMeshAgent>();
        }
        if (gameManager)
        {
            if (gameManager.IsInControl())
            {
                agent.avoidancePriority = 1;
                VectorUpdate();
                MovementUpdate();
            }
            else if(gameManager.IsOverview())
            {
                if (Input.GetMouseButton(0))
                {
                    if (GetMouseCollision())
                    {
                        GameObject col = GetMouseCollision();

                        if (col.GetComponent<Villager>())
                        {
                            //the way i have it set up to set the leader is going to cause an issue when it comes to having multiple groups
                            col.GetComponent<Villager>().SetToLeader();
                            gameManager.SetToInControl();
                           
                            
                            //If the viller that the player clicked on has a unit group already then set that group to the active group in the group manager;
                            if (col.GetComponent<Villager>().GetUnitGroup() != null)
                            {
                                groupManager.SetActiveGroup(col.GetComponent<Villager>().GetUnitGroup());

                                //Change the leader in the unitgroup class
                                //this is super important because without it, the units will all be told by the unit group to follow the old leader.
                                col.GetComponent<Villager>().GetUnitGroup().ChangeUnitLeader(col);
                                col.GetComponent<Villager>().GetUnitGroup().StopDestinations();
                                foreach (GameObject item in groupManager.GetActiveGroup().units)
                                {
                                    if(item != col)
                                    {
                                        if (item.GetComponent<Villager>().IsFollowing() != true)
                                        {
                                            item.GetComponent<Villager>().SetToFollow();
                                        }
                                    }
                                }
                            }
                            else
                            {

                                GameObject hl = Instantiate(hiddenLeader, col.gameObject.transform.position, Quaternion.identity);
                                hl.name = "Hidden Leader";
                                hl.GetComponent<HiddenLeader>().unitLeader = col;
                                UnitGroup unitGroup = new UnitGroup(hl.transform, col, groupManager);
                                groupManager.AddGroup(unitGroup);
                                groupManager.SetActiveGroup(unitGroup);
                                col.GetComponent<Villager>().SetUnitGroup(unitGroup);
                            }
                        }
                        
                    }
                }
            }


        }

        lerpTime += Time.deltaTime;
    }

    private void VectorUpdate()
    {
        var forward = cam.transform.forward;
        var right = cam.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        movementVector = (forward * movementVector.z + right * movementVector.x);
        movementVector.Normalize();
        if (movementVector != LastDirection)
        {
            lerpTime = 0;
        }
        LastDirection = movementVector;
        targetDirection = Vector3.Lerp(targetDirection, movementVector, Mathf.Clamp01(lerpTime * targetLerpSpeed * (1 - smoothing)));
    }
    private void MovementUpdate()
    {
        if (unit != null)
        {
            agent.Move(targetDirection * agent.speed * Time.deltaTime);
            Vector3 lookDirection = movementVector;
            if (lookDirection != Vector3.zero)
            {
                unit.transform.rotation = Quaternion.Lerp(unit.transform.rotation, Quaternion.LookRotation(lookDirection), Mathf.Clamp01(lerpTime * targetLerpSpeed * (1 - smoothing)));
            }
        }

    }

    public void SetUnit(GameObject newLeader)
    {
        if (unit != newLeader)
        {
            unit = newLeader;
        }
    }

    public void RemoveUnitIfPresent(GameObject obj)
    {
        if (unit == obj)
        {
            unit = null;
        }
    }

    /// <returns>returns the game object that the mouse collides with if it is on the unit layer</returns>
    GameObject GetMouseCollision()
    {
        Vector3 mousePosition = new Vector3(0, 0, float.NegativeInfinity);
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitMask))
        {
            return raycastHit.collider.gameObject;
        }
        else return null;

    }

    void Deny()
    {
        if (gameManager.IsInControl())
        {
            gameManager.SetToOverview();
        }
    }




}