using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusic : MonoBehaviour
{
    public AudioSource themeSong;
    public AudioSource librarySong;
    public AudioSource villageSong;
    public AudioSource treantSong;
    public AudioSource forestSong;

    public static MainMusic instance;

    void Awake()
    {
        themeSong.ignoreListenerPause = true;
        librarySong.ignoreListenerPause = true;
        villageSong.ignoreListenerPause = true;
        treantSong.ignoreListenerPause = true;
        forestSong.ignoreListenerPause = true;

        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

}
