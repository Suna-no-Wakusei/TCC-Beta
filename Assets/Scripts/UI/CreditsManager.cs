using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    public GameObject creditsText;

    private void Start()
    {
        Time.timeScale = 1f;
        StopAllCoroutines();
        AudioListener.pause = true;
    }

    void Update()
    {
        if (!creditsText.GetComponent<TextMeshProUGUI>().IsActive())
        {
            SceneManager.LoadScene("StartMenu");
        }
    }

}
