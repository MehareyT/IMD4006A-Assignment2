using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Villager : MonoBehaviour
{
    /// <summary> The unit group that this unit belongs to </summary>
    private UnitGroup unitGroup;
    public PlayerInput playerInput;
    NavMeshAgent agent;



    public List<Villager> neighbours = new List<Villager>();
    /// <summary>
    /// The states which a villager could be in.
    /// </summary>
    public enum State
    {
        idling,
        patroling,
        leading,
        following,
        attacking,
        dead
    }
    
    ///<summary> The state that the villager is in. </summary>
    [SerializeField] private State state = State.idling;



    // Start is called before the first frame update
    void Start()
    {
        playerInput = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerInput>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.idling:
                break;
            case State.patroling:
                break;
            case State.leading:
                break;
            case State.following:

                Vector4 cumulative = new Vector4();
                Quaternion avg = Quaternion.identity;
                if (neighbours.Count == 1)
                {
                    avg = Quaternion.Slerp(gameObject.transform.rotation, neighbours[0].transform.rotation, 0.5f);
                }
                else if (neighbours.Count > 1)
                {
                    foreach (Villager item in neighbours)
                    {
                        avg = MyMath.AverageQuaternion(ref cumulative, item.transform.rotation, gameObject.transform.rotation, neighbours.Count);
                    }
                    gameObject.transform.rotation = avg;
                }

                break;
            case State.attacking:
                break;
            case State.dead:
                break;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        //If it was triggered by a villager
        if (other.GetComponentInParent<Villager>())
        {
            Villager v = other.GetComponentInParent<Villager>();
            AddNeighbour(v);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //If the leaving object is a villager
        if (other.GetComponentInParent<Villager>())
        {
            Villager v = other.GetComponentInParent<Villager>();
            RemoveNeighbour(v);
        }

    }
    /// <summary>
    /// Sets the group that the villager belongs to. 
    /// </summary>
    /// <param name="unitGroup">The group the villager belongs to. </param>
    public void SetUnitGroup(UnitGroup unitGroup)
    {
        this.unitGroup = unitGroup;
    }

    /// <returns> The group that the unit belongs to.</returns>
    public UnitGroup GetUnitGroup()
    {
        return unitGroup;
    }

    /// <summary>
    /// Adds a villager to the villager's list of neighbours if it is not already on the list.
    /// </summary>
    /// <param name="toAdd">the new villager to add to the list of neighbours</param>
    public void AddNeighbour(Villager toAdd)
    {
        if (!neighbours.Contains(toAdd))
        {
            neighbours.Add(toAdd);
        }
    }
    /// <summary>
    /// Removes a villager from the villager's list of neighbours if it is on the list
    /// </summary>
    /// <param name="toRemove">The villager that is no longer a neighbour</param>
    public void RemoveNeighbour(Villager toRemove)
    {
        if (neighbours.Contains(toRemove))
        {
            neighbours.Remove(toRemove);
        }
    }



    /// <summary>
    /// Checks to see if the villager is leading and then removes itself from the playerInput class leader
    /// </summary>
    private void CheckAndRemoveLeadership()
    {
        if (state == State.leading)
        {
            playerInput.RemoveLeaderIfPresent(gameObject);
        }
    }

    public void SetToIdle()
    {
        CheckAndRemoveLeadership();
        state = State.idling;
    }

    public void SetToPatrol()
    {
        CheckAndRemoveLeadership();
        state = State.patroling;
    }

    public void SetToLeader()
    {
        state = State.leading;
        playerInput.SetLeader(gameObject);
    }

    public void SetToFollow()
    {
        CheckAndRemoveLeadership();
        state = State.following;
    }

    public void SetToDead()
    {
        CheckAndRemoveLeadership();
        state = State.dead;
    }

    public bool IsLeading()
    {
        return state == State.leading ? true : false;
    }

    public bool IsFollowing()
    {
        return state == State.following ? true : false;
    }

    public bool IsDead()
    {
        return state == State.dead ? true : false;
    }

    public bool IsAttacking()
    {
        return state == State.attacking ? true : false;
    }

    public bool IsIdling()
    {
        return state == State.idling ? true : false;
    }

    public bool IsPatroling()
    {
        return state == State.patroling ? true : false;
    }
}
