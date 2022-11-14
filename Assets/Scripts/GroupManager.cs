using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroupManager : MonoBehaviour
{
    [SerializeField]
    public UnitGroup activeGroup;
    
    [SerializeField]
    List<UnitGroup> unitGroups = new List<UnitGroup>();

    bool isModified = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //while(isMod == false)
        //{

        UnitGroup[] temp = unitGroups.ToArray();
        foreach (UnitGroup item in temp)
        {
            if (isModified == false)
            {
                item.Update();
            }
            else
            {
                break;
            }


        }
        //}
        isModified = false;

    }

    /// <summary>
    /// Adds a group to the list of groups if it isn't already in the list
    /// </summary>
    /// <param name="toAdd"> the group that is being added</param>
    public void AddGroup(UnitGroup toAdd)
    {
        if (!unitGroups.Contains(toAdd))
            unitGroups.Add(toAdd);
    }

    /// <summary>
    /// Removes a group from the list of groups if it is in the list
    /// </summary>
    /// <param name="toRemove">The group to remove from the list</param>
    public void RemoveGroup(UnitGroup toRemove)
    {
        if (unitGroups.Contains(toRemove))
            unitGroups.Remove(toRemove);
    }

    public UnitGroup GetActiveGroup()
    {
        return activeGroup;
    }

    /// <summary>
    /// Sets the active group for the group manager
    /// </summary>
    /// <param name="unitGroup"> group to set as the active group</param>
    public void SetActiveGroup(UnitGroup unitGroup)
    {
        activeGroup = unitGroup;
    }

    /// <summary>
    /// Sets null as the active group
    /// </summary>
    public void RemoveActiveGroup()
    {
        activeGroup = null;
    }

    /// <summary>
    /// Combines two unit groups together to create a third one. 
    /// </summary>
    /// <param name="groupA"></param>
    /// <param name="groupB"></param>
    /// <param name="unitLeader"></param>
    /// <param name="hiddenLeader"></param>
    /// <returns></returns>
    public UnitGroup CombineGroups(UnitGroup groupA, UnitGroup groupB, GameObject unitLeader, Transform hiddenLeader)
    {
        
        List<GameObject> l = new List<GameObject>();
        foreach(GameObject item in groupA.units)
        {
            if(!l.Contains(item))
                l.Add(item);
        }
        foreach (GameObject item in groupB.units)
        {
            if (!l.Contains(item))
                l.Add(item);
        }
        UnitGroup newGroup = new UnitGroup(hiddenLeader, unitLeader, this, l);
        foreach(GameObject item in l)
        {
            item.GetComponent<Villager>().SetUnitGroup(newGroup);
        }
        if(groupA.hiddenLeader != hiddenLeader)
        {
            Destroy(groupA.hiddenLeader.gameObject);
        }
        if(groupB.hiddenLeader != hiddenLeader)
        {
            Destroy(groupB.hiddenLeader.gameObject);
        }
        groupA.units.Clear();
        groupB.units.Clear();
        
        foreach(GameObject item in newGroup.units)
        {
            item.GetComponent<NavMeshAgent>().ResetPath();
            if(item!= newGroup.unitLeader)
            {
                if (!item.GetComponent<Villager>().IsFollowing())
                {
                    item.GetComponent<Villager>().SetToFollow();
                }
            }
            
        }
        isModified = true;
        unitGroups.Add(newGroup);
        return newGroup;
    }


}
