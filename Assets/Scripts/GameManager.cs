using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject overviewCanvas;
    public GameObject incontrolCanvas;

    public GameObject gameOverScreen;
    public GameObject victoryScreen;
    public GameObject gameMusic;

    public static GameManager Instance;
    public enum GameState
    {
        Menu,
        Pause,
        GameOver,
        Victory,
        Overview,
        InControl
    }
    [SerializeField] public GameState gameState;

    GameObject enemy;
    public float tutorialTimer = 30;
    bool tutorialOnce = false;
    DialogueSystem dialogueSystem;
    public Dialogue tutorial1;
    public Dialogue tutorial2;

    public Dialogue win1;
    public Dialogue win2;

    public Dialogue lose1;
    public Dialogue lose2;

    public Animator tutorial;

    bool lose = true;
    bool win = true;

    private void Awake()
    {
        
        dialogueSystem = GameObject.Find("Dialogue").GetComponent<DialogueSystem>();
        if(Instance == null)
        {
            //DontDestroyOnLoad(gameObject);
            Instance = this;
            var parameters = new LoadSceneParameters(LoadSceneMode.Additive);
            enemy = GameObject.FindGameObjectsWithTag("Enemy")[0]; 
            //SceneManager.LoadScene(1, parameters);
            SceneManager.LoadScene(2, parameters);
            SceneManager.LoadScene(3, parameters);
            SceneManager.LoadScene(4, parameters);
            SceneManager.LoadScene(5, parameters);
            SceneManager.LoadScene(6, parameters);
            SceneManager.LoadScene(7, parameters);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(!lose){
            if(Random.Range(0,100) > 50){
                lose = dialogueSystem.PlayDialogue(lose1);
            }
            else{
                lose = dialogueSystem.PlayDialogue(lose2);
            }
        }
        if(!win){
            if(Random.Range(0,100) > 50){
                win = dialogueSystem.PlayDialogue(win1);
            }
            else{
                win =dialogueSystem.PlayDialogue(win2);
            }

        }
        overviewCanvas.SetActive(IsOverview());
        incontrolCanvas.SetActive(IsInControl());
        if(gameState == GameState.Overview){
            if(tutorialTimer >= 0){
                tutorialTimer -= Time.deltaTime;
            }
            else if(!tutorialOnce){
                if(Random.Range(0,100) > 50){
                    tutorialOnce = dialogueSystem.PlayDialogue(tutorial1);
                }
                else{
                    tutorialOnce = dialogueSystem.PlayDialogue(tutorial2);
                }
                tutorial.SetTrigger("Trigger");
            }
        }
        else if(!tutorialOnce){
            tutorialOnce = true;
        }
        switch(gameState){
            case GameState.Overview:
            case GameState.InControl:
                if(GetComponent<PopulationManager>().population <= GetComponent<PopulationManager>().maxPopulation * 0.40){
                    SetToGameOver();
                    Destroy(GameObject.Find("Enemy"),2f);
                    gameOverScreen.SetActive(true);
                    gameMusic.SetActive(false);
                    if(Random.Range(0,100) > 50){
                       lose = dialogueSystem.PlayDialogue(lose1);
                    }
                    else{
                       lose = dialogueSystem.PlayDialogue(lose2);
                    }
                }
                else if(enemy.GetComponent<Enemy>().health <= 0){
                    SetToVictory();
                    GameObject.Find("Enemy").SetActive(false);
                    victoryScreen.SetActive(true);
                    gameMusic.SetActive(false);
                    if(Random.Range(0,100) > 50){
                        win = dialogueSystem.PlayDialogue(win1);
                    }
                    else{
                        win =dialogueSystem.PlayDialogue(win2);
                    }
                }
                break;
        }
        //UnitGroup unitGroup = new UnitGroup(transform);
    }

    public void SetToMenu()
    {
        gameState = GameState.Menu;
    }

    public void SetToPause()
    {
        gameState = GameState.Pause;
    }

    public void SetToGameOver()
    {
        gameState = GameState.GameOver;
    }

    public void SetToVictory()
    {
        gameState = GameState.Victory;
    }

    public void SetToOverview()
    {
        gameState = GameState.Overview;

    }

    public void SetToInControl()
    {
        gameState = GameState.InControl;
    }

    /// <returns>Returns true if the game state == Menu </returns>
    public bool IsMenu()
    {
        return gameState == GameState.Menu ? true : false;
    }

    /// <returns>Returns true if the game state == Pause </returns>
    public bool IsPaused()
    {
        return gameState == GameState.Pause ? true : false;
    }

    /// <returns> Returns true if the game state == GameOver </returns>
    public bool IsGameOver()
    {
        return gameState == GameState.GameOver ? true : false;
    }

    /// <returns>Returns true if the game state == Victory </returns>
    public bool IsVictory()
    {
        return gameState == GameState.Victory ? true : false;
    }

    /// <returns>Returns true if the game state == OverView </returns>
    public bool IsOverview()
    {
        return gameState == GameState.Overview ? true : false;
    }

    /// <returns>Returns true if the game state == InControl </returns>
    public bool IsInControl()
    {
        return gameState == GameState.InControl ? true : false; 
    }


}
