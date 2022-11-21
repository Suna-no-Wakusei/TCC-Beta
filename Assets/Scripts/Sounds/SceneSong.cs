using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSong : MonoBehaviour
{
    private MainMusic music;

    public enum Song
    {
        Null,
        ThemeSong,
        VillageSong,
        TreantSong,
        LibrarySong
    }

    public Song song;

    void Start()
    {
        music = MainMusic.instance;

        switch (song)
        {
            case Song.Null:
                if (!SFXManager.instance.magicAmbient.isPlaying)
                    SFXManager.instance.magicAmbient.Play();
                music.themeSong.Stop();
                music.villageSong.Stop();
                music.treantSong.Stop();
                music.librarySong.Stop();
                break;
            case Song.ThemeSong:
                if (!music.themeSong.isPlaying)
                    music.themeSong.Play();
                music.villageSong.Stop();
                music.treantSong.Stop();
                music.librarySong.Stop();
                SFXManager.instance.magicAmbient.Stop();
                break;
            case Song.VillageSong:
                music.themeSong.Stop();
                if (!music.villageSong.isPlaying)
                    music.villageSong.Play();
                music.treantSong.Stop();
                music.librarySong.Stop();
                SFXManager.instance.magicAmbient.Stop();
                break;
            case Song.TreantSong:
                music.themeSong.Stop();
                music.villageSong.Stop();
                if (!music.treantSong.isPlaying)
                    music.treantSong.Play();
                music.librarySong.Stop();
                SFXManager.instance.magicAmbient.Stop();
                break;
            case Song.LibrarySong:
                music.themeSong.Stop();
                music.villageSong.Stop();
                music.treantSong.Stop();
                if (!music.librarySong.isPlaying)
                    music.librarySong.Play();
                SFXManager.instance.magicAmbient.Stop();
                break;
        }
    }
}
