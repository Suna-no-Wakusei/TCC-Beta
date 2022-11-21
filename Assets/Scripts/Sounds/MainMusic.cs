using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusic : MonoBehaviour
{
    public AudioSource themeSong;
    public AudioSource librarySong;
    public AudioSource villageSong;
    public AudioSource treantSong;

    public static MainMusic instance;

    void Awake()
    {
        themeSong.ignoreListenerPause = true;
        librarySong.ignoreListenerPause = true;
        villageSong.ignoreListenerPause = true;
        treantSong.ignoreListenerPause = true;
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
