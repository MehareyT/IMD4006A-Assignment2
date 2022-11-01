using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    ///<summary> The target to move towards. </summary>
    public Transform target;

    ///<summary> The time between each navmesh path recalculation. </summary>
    public float updateSpeed = 0.1f; //how frequently to

    ///<summary> The enemies navmesh agent. </summary>
    private UnityEngine.AI.NavMeshAgent agent;

    public bool followingTarget;

    private IEnumerator followCoroutine;

    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        followCoroutine = FollowTarget();
    }

    // Update is called once per frame
    void Start()
    {
        if(followingTarget){
            StartCoroutine(followCoroutine);
        }
        
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(updateSpeed);

        while(enabled){
            if(target != null){
                agent.SetDestination(target.transform.position);
            }
            else{
                StopFollowing();
            }
            

            yield return wait;
        }


    }


    /// <summary>
    /// Changes the target the enemy is chasing.
    /// </summary>
    /// <param name="target">The target the enemy is chasing</param>
    public void SetTarget(Transform tar){
        target = tar;
    }

    /// <summary>
    /// Stops the enemy from following the target.
    /// </summary>
    public void StopFollowing(){
        StopCoroutine(followCoroutine);
        agent.ResetPath();
        followingTarget = false;
    }

    /// <summary>
    /// Starts the enemy following a target.
    /// </summary>
    public void StartFollowing(){
        StartCoroutine(followCoroutine);
        followingTarget = true;
    }

    
}
