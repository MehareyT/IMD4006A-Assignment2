using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputActions;
    private InputActionMap leaderActionMap;
    private InputAction movement;

    [SerializeField]
    private Camera cam;
    private NavMeshAgent agent;
    [SerializeField]
    [Range(0, 0.99f)]
    private float smoothing = 0.25f;

    private Vector3 targetDirection;
    private float lerpTime = 0;
    [SerializeField]
    [Range(0,100)]
    private float targetLerpSpeed = 1;
    private Vector3 LastDirection;
    private Vector3 movementVector;
    public GameObject controlledVillager;

    private void Awake()
    {
        if (controlledVillager == null)
        {
            Debug.Log("Leader is null");
        }
        agent = controlledVillager.GetComponent<NavMeshAgent>();
        leaderActionMap = inputActions.FindActionMap("Leader");
        movement = leaderActionMap.FindAction("Move");
        movement.started += HandleMovementAction;
        movement.performed += HandleMovementAction;
        movement.canceled += HandleMovementAction;
        movement.Enable();
        leaderActionMap.Enable();
        inputActions.Enable();
    }

    private void Start()
    {
        
    }

    private void HandleMovementAction(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        movementVector = new Vector3(input.x, 0, input.y);
    }
    private void Update()
    {
/*        if(agent == null)
        {

            if (controlledVillager)
            {
                agent = controlledVillager.GetComponent<NavMeshAgent>();
            }
        }*/

        agent.avoidancePriority = 1;
        VectorUpdate();
        MovementUpdate();
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
        Debug.Log($"MovementVector: {movementVector}");
        Debug.Log($"TargetDirection: {targetDirection}");
        Debug.Log($"LastDirection: {LastDirection}");
    }
    private void MovementUpdate()
    {
        if (controlledVillager != null)
        {
            Debug.Assert(agent);
            agent.Move(targetDirection * agent.speed * Time.deltaTime);
            Vector3 lookDirection = movementVector;
            if (lookDirection != Vector3.zero)
            {
                controlledVillager.transform.rotation = Quaternion.Lerp(controlledVillager.transform.rotation, Quaternion.LookRotation(lookDirection), Mathf.Clamp01(lerpTime * targetLerpSpeed * (1 - smoothing)));
            }
        }
    }

    public void SetControlledVillager(GameObject toControl)
    {
        if (controlledVillager != toControl)
        {
            controlledVillager = toControl;
        }
    }

    public void RemoveControlledVillager(GameObject obj)
    {
        if (controlledVillager == obj)
        {
            controlledVillager = null;
        }
    }

}
