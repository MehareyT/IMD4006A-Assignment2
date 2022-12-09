using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopulationManager : MonoBehaviour
{

    /// <summary> The max population minus the total amount of population removed </summary>
    public int population = 0;

    /// <summary> The maximum amount of population that can ever be spawned </summary>
    public int maxPopulation = 0;

    /// <summary> The total amount of population that has been killed in the last 35 milliseconds </summary>
    public int popRemoved = 0;

    /// <summary> The total amount of population actualy spawned on the map </summary>
    public int currentPop = 0;

    /// <summary> The position to instantiate our villager loss effect text </summary>
    public Transform spawnPoint;

    /// <summary> The UI text of the population </summary>
    public TMP_Text populationText;

    /// <summary> The prefab of the villager loss effect text </summary>
    public GameObject numberPrefab;

    /// <summary> The UI text animator of the population </summary>
    public Animator pop;

    /// <summary> The amount of time to count the amount of villagers removed </summary>
    float counting = 0f;

    bool warningOnce = false;

    DialogueSystem dialogueSystem;

    public Dialogue lowPop1;
    public Dialogue lowPop2;
    
    void Awake(){
        dialogueSystem = GameObject.Find("Dialogue").GetComponent<DialogueSystem>();
    }


    //The hurt function will be triggered multiple times over the course of a few milliseconds when a group of villagers is killed. 
    //To prevent the system from creating a bunch of -1 text effects we wait for .35 seconds and count all the kills during that time.

    public void Hurt(int x){
        if(counting <= 0){
            counting = 0.35f;
        }
        popRemoved += x;
    }

    void Update(){
        currentPop = GameObject.FindGameObjectsWithTag("Player").Length;
        populationText.text = population.ToString();
        if(counting > 0){
            counting -= Time.deltaTime;
        }
        else if(popRemoved > 0){
            pop.SetTrigger("Population");

            var obj = Instantiate(numberPrefab,spawnPoint.position,Quaternion.identity,spawnPoint);
            obj.GetComponent<TMP_Text>().text = "-"+popRemoved.ToString();
            Destroy(obj,1.5f);
            population -= popRemoved;
            popRemoved = 0;

        }

        if(population <= (maxPopulation * 0.75f) && !warningOnce){
            if(Random.Range(0,100) > 50){
                warningOnce = dialogueSystem.PlayDialogue(lowPop1);
            }
            else{
                warningOnce = dialogueSystem.PlayDialogue(lowPop2);
            }
        }
    }
}
