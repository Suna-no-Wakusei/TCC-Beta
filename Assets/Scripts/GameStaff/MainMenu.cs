using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{

    public GameObject optionScreen;

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

    public void OpenOptions()
    {
        optionScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        optionScreen.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
