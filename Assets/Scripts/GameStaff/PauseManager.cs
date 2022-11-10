using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public string mainMenu;

    // Start is called before the first frame update

    public void QuitToMenu()
    {
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1f;
    }

    public void SaveAndQuit()
    {
        SaveSystem.SaveState();
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1f;
    }
}
