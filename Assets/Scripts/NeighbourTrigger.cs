using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class NeighbourTrigger : MonoBehaviour
{
    private Villager villager;

    private void Start()
    {
        villager = GetComponentInParent<Villager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Villager>())
        {
            Villager v = other.gameObject.GetComponent<Villager>();
            villager.AddNeighbour(v);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //If the leaving object is a villager
        if (other.GetComponentInParent<Villager>())
        {
            Villager v = other.gameObject.GetComponent<Villager>();
            villager.RemoveNeighbour(v);
        }

    }
}
