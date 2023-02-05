using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void Quitame()
    {
        Debug.Log("Quit action");
        Application.Quit();
    }


    public void MainScreen()
    {
        SceneManager.LoadScene(0);
    }

    public void WinGame()
    {
        SceneManager.LoadScene(3);
    }

    public void CreditsScene()
    {
        SceneManager.LoadScene(4);
    }
}
