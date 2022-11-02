using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;


//This allows unity to put the class in the inspector among other things
[System.Serializable]
public class UnitGroup 
{
    GroupManager groupManager;
    /// <summary> The leader is the transform the group will follow. </summary>
    public Transform hiddenLeader;
    
    [SerializeField]
    /// <summary> playerUnit is the Unit that the player is in control of - this isn't always in the UnitGroup</summary>
    public GameObject unitLeader;
    
    /// <summary> List of units in the group </summary>
    public List<GameObject> units = new List<GameObject>();
    
    /// <summary>
    /// Create a new unit group class with a hiddenLeader. 
    /// </summary>
    /// <param name="hiddenLeader">The hiddenLeader is the transorm the group will follow.</param>
    /// <param name="unitLeader">The unit that is designated as Leader</param>
    public UnitGroup(Transform hiddenLeader, GameObject unitLeader, GroupManager groupManager) {
        this.hiddenLeader = hiddenLeader;
        this.unitLeader = unitLeader;
        this.groupManager = groupManager;
        AddUnit(unitLeader);
    }

    public UnitGroup(Transform hiddenLeader, GameObject unitLeader, GroupManager groupManager, List<GameObject> units)
    {
        this.hiddenLeader = hiddenLeader;
        this.unitLeader = unitLeader;
        this.groupManager = groupManager;
        AddUnit(unitLeader);
        foreach(GameObject item in units)
        {
            AddUnit(item);
        }
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

    //NEEDS TO BE CALLED EVEY FRAME (UPDATE) BY GROUPMANAGER
    public void Update()
    {
        units.RemoveAll(item => item == null);
        if (unitLeader!=null)
        {
            UpdateUnitDestinations();
            //units.RemoveAll(item => item == null);
        }
        else{
            foreach(GameObject item in units)
            {
                if(item != null){ 
                    item.GetComponent<Villager>().SetToIdle();
                }
                
            }
            units.Clear();

        }
        
        if (units.Count <= 0)
        {
            Debug.Log("Destroying Unit Group");
            Debug.Assert(groupManager);
            groupManager.RemoveGroup(this);
        }
         
    }

    void UpdateUnitDestinations()
    {
        foreach(GameObject item in units)
        {
            if(item != null){ item.GetComponent<NavMeshAgent>().destination = hiddenLeader.position; }
            
            
            
        }
    }

    public void ChangeUnitLeader(GameObject newLeader)
    {
        unitLeader = newLeader;
        hiddenLeader.GetComponent<HiddenLeader>().unitLeader = newLeader;
    }
    
    public void StopDestinations()
    {
        foreach(GameObject item in units)
        {
            item.GetComponent<NavMeshAgent>().ResetPath();

        }
    }

}
