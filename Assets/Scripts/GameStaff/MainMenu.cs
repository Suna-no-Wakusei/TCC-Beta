using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("Casa");
    }

    public void Continue()
    {
        if (GameManager.instance.actualScene == null)
            NewGame();
        else
            SceneManager.LoadScene(GameManager.instance.actualScene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
