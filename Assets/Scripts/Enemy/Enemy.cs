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

    void Awake(){
        enemyMovement = GetComponent<EnemyMovement>();
        enemyMovement.SetTarget(player);

        enemyAttack = GetComponent<EnemyAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyAnimator.SetBool("Run", enemyMovement.followingTarget);
        switch (state)
        {
            case State.idling:
                //calculate how much to eat
                if(Vector3.Distance(transform.position,player.transform.position) <= movementRange){
                    enemyMovement.StartFollowing();
                    state = State.chasing;
                }
                break;
            case State.chasing:
                if(Vector3.Distance(transform.position,player.transform.position) <= movementRange){
                    if(Vector3.Distance(transform.position,player.transform.position) <= attackRange){
                        enemyMovement.StopFollowing();
                        enemyAttack.Attack(enemyAnimator);
                        state = State.attacking;
                    }
                    else{
                        
                        //move towards player
                    }
                }
                else{
                    enemyMovement.StopFollowing();
                    state = State.idling;
                }
                break;
            case State.attacking:
                if(enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("EnemyIdle")){
                    if(Vector3.Distance(transform.position,player.transform.position) <= movementRange){
                        if(Vector3.Distance(transform.position,player.transform.position) <= attackRange){
                            enemyAttack.Attack(enemyAnimator);
                        }
                        else{
                            enemyMovement.StartFollowing();
                            state = State.chasing;
                        }
                    }
                    else{
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

    /// <summary>
    /// Sets the state to chasing. 
    /// </summary>
    public void SetToChasing(UnitGroup unitGroup)
    {
        state = State.chasing;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, movementRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
