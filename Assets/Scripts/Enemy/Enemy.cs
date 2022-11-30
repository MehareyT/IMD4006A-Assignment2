using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    /// <summary>
    /// The states which the enemy could be in.
    /// </summary>
    public enum State
    {
        chasing,
        attacking,
        fleeing,
        idling,
        dead
    }

    ///<summary> The state that the enemy is in. </summary>
    [SerializeField] private State state = State.idling;

    ///<summary> The player gameobject the enemy references for distance. </summary>
    [SerializeField] private Transform player;

    ///<summary> The range the player needs to be from the enemy for it to start chasing. </summary>
    [SerializeField] private float movementRange;

    ///<summary> The range the player needs to be from the enemy for it to start attacking. </summary>
    [SerializeField] private float attackRange;

    ///<summary> The enemies animator. </summary>
    [SerializeField] private Animator enemyAnimator;

    ///<summary> The enemies movement script. </summary>
    private EnemyMovement enemyMovement;

    ///<summary> The enemies attack script. </summary>
    private EnemyAttack enemyAttack;

    ///<summary> The map locations script. </summary>
    private SpawnManager mapLocations;

    public int health = 3;

    public Animator hurtAnimator;

    public Healthbar healthbar;

    public AudioSource thud;


    ///<summary> The range the player needs to be from the enemy to begin onscreen AI. </summary>
    [SerializeField] private float detectRange;

    ///<summary> The range the enemy can detect nearby locations. </summary>
    [SerializeField] private float locationTargetRange;

    ///<summary> Gameobject the enemy is currently hunting that is not the player. </summary>
    private Transform nonPlayerTarget;

    ///<summary> Int ID of enemy's current location </summary>
    private int currentBase;

    void Awake(){
        thud = GetComponent<AudioSource>();
        player = FindClosestPlayer().transform; 
        enemyMovement = GetComponent<EnemyMovement>();
        enemyMovement.SetTarget(player);
        healthbar.current = health;
        healthbar.max = health;
        enemyAttack = GetComponent<EnemyAttack>();
        mapLocations = GameObject.Find("GameManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.current = health;
        if (enemyMovement.target == null && state != State.attacking){
            state = State.idling;
        }
        player = FindClosestPlayer().transform; 
        enemyAnimator.SetBool("Run", enemyMovement.followingTarget);
        switch (state)
        {
            case State.idling:
                //calculate how much to eat
                if (Vector3.Distance(transform.position, player.transform.position) <= movementRange)
                {
                    enemyMovement.SetTarget(player);
                    enemyMovement.StartFollowing();
                    state = State.chasing;
                }
                break;
            case State.chasing:
                if (Vector3.Distance(transform.position, player.transform.position) <= movementRange)
                {
                    if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
                    {
                        enemyMovement.StopFollowing();
                        enemyAttack.Attack(enemyAnimator);
                        state = State.attacking;
                    }
                    else
                    {

                        //move towards player
                    }
                }
                else
                {
                    enemyMovement.StopFollowing();
                    state = State.idling;
                }
                break;
            case State.attacking:
                if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("EnemyIdle") || enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("EnemyRun"))
                {
                    if (Vector3.Distance(transform.position, player.transform.position) <= movementRange)
                    {
                        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
                        {
                            enemyAttack.Attack(enemyAnimator);
                        }
                        else
                        {
                            enemyMovement.StartFollowing();
                            state = State.chasing;
                        }
                    }
                    else
                    {
                        state = State.idling;
                    }
                }
                break;
            case State.fleeing:
                // if player is in range
                //move away from player
                //else switch state to idling
                break;
            case State.dead:
                break;
        }
        
    }

    public GameObject FindClosestPlayer()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, movementRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, locationTargetRange);
    }

    /// <summary>
    /// Makes the enemy take a specific amount of damage
    /// </summary>
    /// <param name="damage">The amount of damage the enemy will take. </param>
    public void Hurt(int damage){
        health -= damage;
        thud.Play(0);
        hurtAnimator.SetTrigger("Hurt");
        if(health <= 0){
            state = State.dead;
            enemyMovement.StopFollowing();
            enemyAnimator.SetTrigger("Die");
        }

    }


    /// <summary>
    /// Selects a nearby location for monster to target. If none are nearby it selects a random location. 
    /// </summary>
    private Transform selectNewLocation()
    {
        List<Transform> nearby = new();
        foreach(GameObject newTarget  in mapLocations.locations)
        {
            if (Vector3.Distance(transform.position, newTarget.transform.position) <= locationTargetRange)
            {
                nearby.Add(newTarget.transform);
            }
        }
        if (nearby.Count > 0)
        {
            return nearby[Random.Range(0, nearby.Count)];
        }
        else
        {
            return mapLocations.locations[Random.Range(0, mapLocations.locations.Count)].transform;
        }
    }

}
