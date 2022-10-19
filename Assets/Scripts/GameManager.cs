using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject Leader;
    public enum GameState
    {
        Menu,
        Pause,
        GameOver,
        Victory,
        OverView,
        InControl
    }
    public static GameState gameState;

    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //UnitGroup unitGroup = new UnitGroup(transform);
    }

    public void SetLeader()
    {
        
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
    public bool IsOverView()
    {
        return gameState == GameState.OverView ? true : false;
    }

    /// <returns>Returns true if the game state == InControl </returns>
    public bool IsInControl()
    {
        return gameState == GameState.InControl ? true : false; 
    }

}
