using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        Time.timeScale = 1f;
        StopAllCoroutines();
    }

    public void NewGame()
    {
        if (File.Exists(Application.persistentDataPath + "/JSONData.sus"))
        {
            File.Delete(Application.persistentDataPath + "/JSONData.sus");
        }
        
        SceneManager.LoadScene("Casa");
    }

    public void Continue()
    {
        try
        {
            if (File.Exists(Application.persistentDataPath + "/JSONData.sus"))
            {
                SaveSystem.LoadState();
                SaveSystem.LoadSavedScene();
            }
            else
            {
                NewGame();
            }
        }
        catch
        {
            SceneManager.LoadScene("Casa");
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
