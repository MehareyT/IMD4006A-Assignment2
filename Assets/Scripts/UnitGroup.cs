using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitGroup : MonoBehaviour
{
    /// <summary> The leader is the transform the group will follow. </summary>
    public Transform hiddenLeader;
    
    /// <summary> playerUnit is the Unit that the player is in control of - this isn't always in the UnitGroup</summary>
    public GameObject unitLeader;
    
    /// <summary> List of units in the group </summary>
    public List<GameObject> units = new List<GameObject>();
    
    /// <summary>
    /// Create a new unit group class with a hiddenLeader. 
    /// </summary>
    /// <param name="hiddenLeader">The hiddenLeader is the transorm the group will follow.</param>
    /// <param name="unitLeader">The unit that is designated as Leader</param>
    public UnitGroup(Transform hiddenLeader, GameObject unitLeader) {
        this.hiddenLeader = hiddenLeader;
        this.unitLeader = unitLeader;
        AddUnit(unitLeader);
    }
    
    ~UnitGroup()
    {
        units.Clear();
    }

    /// <summary>
    /// Adds a unit to the UnitGroup if it is not already in the group.
    /// </summary>
    /// <param name="toAdd">The unit you are adding to the group</param>
    public void AddUnit(GameObject toAdd)
    {
        if (!units.Contains(toAdd))
        {
            units.Add(toAdd);
        }
    }
    /// <summary>
    /// Removes a unit to the UnitGroup if it is in the group.
    /// </summary>
    /// <param name="toRemove">The unit you are removing from the group</param>
    public void RemoveUnit(GameObject toRemove)
    {
        if (units.Contains(toRemove))
        {
            units.Remove(toRemove);
        }
    }

    void Update()
    {
        if (unitLeader)
        {
            UpdateLeaderPosition();
            UpdateUnitDestinations();
        }
         
    }

    void UpdateUnitDestinations()
    {
        foreach(GameObject item in units)
        {
            if (item != unitLeader)
            {
                item.GetComponent<NavMeshAgent>().destination = hiddenLeader.position;
            }
            
        }
    }

    void UpdateLeaderPosition()
    {
        hiddenLeader.position = unitLeader.transform.position;
    }

    


}
