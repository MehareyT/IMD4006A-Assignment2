using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopulationManager : MonoBehaviour
{

    public int population = 0;

    public int maxPopulation = 0;

    public int popRemoved = 0;

    public Transform spawnPoint;

    public TMP_Text populationText;

    public GameObject numberPrefab;

    public Animator pop;

    float counting = 0f;

    public void Hurt(int x){
        if(counting <= 0){
            counting = 0.25f;
        }
        popRemoved += x;
    }

    void Update(){
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
    }
}
