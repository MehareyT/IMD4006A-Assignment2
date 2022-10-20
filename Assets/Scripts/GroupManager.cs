using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupManager : MonoBehaviour
{
    UnitGroup activeGroup;
    List<UnitGroup> unitGroups = new List<UnitGroup>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

}
