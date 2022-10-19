using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupManager : MonoBehaviour
{

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
}
