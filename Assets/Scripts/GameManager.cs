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


}
