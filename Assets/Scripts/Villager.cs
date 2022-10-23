using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Villager : MonoBehaviour
{
    /// <summary> The unit group that this unit belongs to </summary>
    private UnitGroup unitGroup;
    public PlayerInput playerInput;
    GroupManager groupManager;
    NavMeshAgent agent;


    //For Debugging
    public TMPro.TextMeshProUGUI textMesh;
    

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
        groupManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GroupManager>();
        agent = GetComponent<NavMeshAgent>();
        neighbours.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (textMesh != null)
        {
            //textMesh.text = agent.destination.ToString();
          
            if (state == State.leading)
            {
                textMesh.text = "leader";
            }else if (state == State.following)
            {
                textMesh.text = "follower";
            } else
            {
                textMesh.text = "idle";
            }
        }

        switch (state)
        {
            case State.idling:
                break;
            case State.patroling:
                break;
            case State.leading:
                if(agent.avoidancePriority!= 2)
                {
                    agent.avoidancePriority = 2;
                }
                break;
            case State.following:
                if (agent.avoidancePriority != 50)
                {
                    agent.avoidancePriority = 50;
                }
                Vector4 cumulative = new Vector4();
                Quaternion avg = Quaternion.identity;
                if (neighbours.Count == 1)
                {
                    avg = Quaternion.Slerp(gameObject.transform.rotation, neighbours[0].transform.rotation, 0.5f);
                }
                else if (neighbours.Count >= 1)
                {
                    foreach (Villager item in neighbours)
                    {
                        if(item!= null)
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
        //if (neighbours.Contains(toRemove))
        //{
        neighbours.Remove(toRemove);
        // }
    }



    /// <summary>
    /// Checks to see if the villager is leading and then removes itself from the playerInput class leader
    /// </summary>
    private void CheckAndRemoveLeadership()
    {
        if (state == State.leading)
        {
            playerInput.RemoveUnitIfPresent(gameObject);
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
        playerInput.SetUnit(gameObject);
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

    public void Rally()
    {
        Debug.Log("Rally!");
        if (IsLeading())
        {
            if (neighbours.Count > 0)
            {
                Debug.Assert(unitGroup != null);
                
                foreach (Villager item in neighbours)
                {
                    if (item != null)
                    {
                        if (item.GetUnitGroup() == unitGroup && unitGroup != null)
                        {
                            //neighbour is already in your group
                            Debug.Log("neighbour is already in your group");
                        }
                        else
                        {
                            if (item.GetUnitGroup() == null)
                            {
                                unitGroup.AddUnit(item.gameObject);
                                item.SetToFollow();
                                item.SetUnitGroup(unitGroup);
                            }
                            else if (item.GetUnitGroup() != null)
                            {
                                Debug.Log("will combine");
                                groupManager.CombineGroups(unitGroup, item.GetUnitGroup(), gameObject, unitGroup.hiddenLeader);
                            }
                        }
                    }
                }
            }
        }
    }
}

