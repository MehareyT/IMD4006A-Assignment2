using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField]
    private InputActionAsset inputActions;
    private InputActionMap leaderActionMap;
    private InputAction movement;
    private InputAction mousePositionAction;
    private InputAction denyAction;

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
    public GameObject leader;

    public LayerMask unitMask;

    Vector2 mousePos = new Vector2();



    private void Awake()
    {
        if (leader)
        {
            agent = leader.GetComponent<NavMeshAgent>();
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




        leaderActionMap.Enable();
        inputActions.Enable();
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
    }
    private void HandleMovementAction(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        movementVector = new Vector3(input.x, 0, input.y);
    }



    private void Update()
    {
        if (leader)
        {
            agent = leader.GetComponent<NavMeshAgent>();
        }
        if (gameManager)
        {
            if (gameManager.IsInControl())
            {
                agent.avoidancePriority = 1;
                VectorUpdate();
                MovementUpdate();
            }
            else if(gameManager.IsOverView())
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
        if (leader != null)
        {
            agent.Move(targetDirection * agent.speed * Time.deltaTime);
            Vector3 lookDirection = movementVector;
            if (lookDirection != Vector3.zero)
            {
                leader.transform.rotation = Quaternion.Lerp(leader.transform.rotation, Quaternion.LookRotation(lookDirection), Mathf.Clamp01(lerpTime * targetLerpSpeed * (1 - smoothing)));
            }
        }

    }

    public void SetLeader(GameObject newLeader)
    {
        if (leader != newLeader)
        {
            leader = newLeader;
        }
    }

    public void RemoveLeaderIfPresent(GameObject obj)
    {
        if (leader == obj)
        {
            leader = null;
        }
    }

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
            gameManager.SetToOverView();
        }
    }

}