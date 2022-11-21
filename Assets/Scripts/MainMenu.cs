using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenu_Buttons;


    // Start is called before the first frame update
    void Start()
    {
      
    }

    public void Play_Button() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
    }

    public void Quit_Button()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
