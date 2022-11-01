using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffscreenEnemyAI : MonoBehaviour
{

    /// <summary> Collider in which the enemy detects things (villagers, locations.) </summary>
    SphereCollider detectRing;

    Transform enemyTransform;

    private float huntStart;
    private float huntLength;

    int newTarget;

    //For Debugging
    public TMPro.TextMeshProUGUI textMesh;

    /// <summary> List of map locations </summary>
    public List<GameObject> locations = new List<GameObject>();

    /// <summary>
    /// The states which a enemy could be in.
    /// </summary>
    public enum State
    {
        idling,
        hunting,
        fleeing,
        attacking,
        dead
    }

    public bool enemyOnscreen;

    ///<summary> The state that the enemy is in. </summary>
    [SerializeField] private State state = State.idling;

    // Start is called before the first frame update
    void Start()
    {
        detectRing = gameObject.GetComponent<SphereCollider>();
        enemyTransform = gameObject.GetComponent<Transform>();
    }



    void startHunt()
    {
        huntStart = Time.time;
        huntLength = Random.Range(1.0f, 5.0f);
        Debug.Log(huntStart);
        state = State.hunting;
        newTarget = -1;
    }

    int selectTarget()
    {
        
        if (Time.time - huntStart > huntLength)
        {
            return 0;  //Random.Range(0,locations.Count);
        }
        Debug.Log("Time Till Move: " + (huntLength - Time.time + huntStart));
        return -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (textMesh != null)
        {
            //textMesh.text = agent.destination.ToString();

            if (state == State.hunting)
            {
                textMesh.text = "hunting";
            }
            else if (state == State.attacking)
            {
                textMesh.text = "attacking";
            }
            else
            {
                textMesh.text = "idle";
            }
        }

        if (enemyOnscreen)
        {
        }
        else
        {
            switch (state)
            {
                case State.idling:
                    startHunt();
                    break;
                case State.hunting:
                    if (newTarget > -1)
                    {
                        enemyTransform.position = locations[newTarget].GetComponent<Transform>().position;
                    }
                    else
                    {
                        newTarget = selectTarget();
                    }
                    break;
                case State.fleeing:
                    break;
                case State.attacking:
                    break;
                case State.dead:
                    break;



            }
        }
    }
}
