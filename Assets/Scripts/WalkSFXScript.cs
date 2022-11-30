using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSFXScript : MonoBehaviour
{
    [SerializeField] Villager villager;

    public AudioSource footSound;

    [SerializeField] Rigidbody rb;

    bool cr_running = false;

    /// <summary> The unit group that this unit belongs to </summary>
    private UnitGroup unitGroup;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (villager.IsLeading())
        {
            unitGroup = villager.GetUnitGroup();

            if (villager.moving > 0.5f)
            {
                if (!cr_running)
                {
                    StartCoroutine(VillagerWalk());
                }

                footSound.enabled = true;
                footSound.volume = (0.5f / Mathf.Min(unitGroup.units.Count, 5)) + 0.5f;
                footSound.pitch = Random.Range(0.9f, 1.2f);

            }
            else
            {
                footSound.enabled = false;
            }
        }

    }
    IEnumerator VillagerWalk()
    {
        cr_running = true;
        for (int i = 1; i < Mathf.Min(unitGroup.units.Count, 5); i++)
        {
            Debug.Log("walk");
            StartCoroutine(PlayWalkSound());
        }
        yield return new WaitForSeconds(0.672f);
        cr_running = false;
        yield return null;
    }

    IEnumerator PlayWalkSound()
    {
        yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
        footSound.PlayOneShot(footSound.clip);
        yield return null;
    }
}
