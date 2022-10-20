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

    LayerMask unitMask = 6;

    private void Awake()
    {   
        if (leader == null)
        {
            Debug.Log("Leader is null");
        }
        agent = leader.GetComponent<NavMeshAgent>();
        leaderActionMap = inputActions.FindActionMap("Leader");
        movement = leaderActionMap.FindAction("Move");
        movement.started += HandleMovementAction;
        movement.performed += HandleMovementAction;
        movement.canceled += HandleMovementAction;
        movement.Enable();

        mousePositionAction = leaderActionMap.FindAction("MousePositon");
        leaderActionMap.Enable();
        inputActions.Enable();
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
                    GetMouseCollision();
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

    void GetMouseCollision()
    {
        Vector3 mousePosition = new Vector3(0, 0, float.NegativeInfinity);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitMask))
        {
            Debug.Log($"{raycastHit.collider.gameObject.name}");
        }

    }



}