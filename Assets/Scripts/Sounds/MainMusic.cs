using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusic : MonoBehaviour
{
    public AudioSource audioSource;

    public static MainMusic instance;

    void Start()
    {
        audioSource.Play();
        DontDestroyOnLoad(gameObject);

        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

}
