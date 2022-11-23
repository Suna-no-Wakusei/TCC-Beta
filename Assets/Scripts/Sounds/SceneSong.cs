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
        LibrarySong,
        ForestSong
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
                music.forestSong.Stop();
                SFXManager.instance.ambient.Stop();
                break;
            case Song.ThemeSong:
                if (!music.themeSong.isPlaying)
                    music.themeSong.Play();
                music.villageSong.Stop();
                music.treantSong.Stop();
                music.librarySong.Stop();
                music.forestSong.Stop();
                SFXManager.instance.magicAmbient.Stop();
                break;
            case Song.VillageSong:
                music.themeSong.Stop();
                if (!music.villageSong.isPlaying)
                    music.villageSong.Play();
                music.treantSong.Stop();
                music.librarySong.Stop();
                music.forestSong.Stop();
                SFXManager.instance.magicAmbient.Stop();
                break;
            case Song.TreantSong:
                music.themeSong.Stop();
                music.villageSong.Stop();
                if (!music.treantSong.isPlaying)
                    music.treantSong.Play();
                music.librarySong.Stop();
                music.forestSong.Stop();
                if (SFXManager.instance.ambient.isPlaying)
                    SFXManager.instance.ambient.Stop();
                SFXManager.instance.magicAmbient.Stop();
                break;
            case Song.LibrarySong:
                music.themeSong.Stop();
                music.villageSong.Stop();
                music.treantSong.Stop();
                music.forestSong.Stop();
                if (!music.librarySong.isPlaying)
                    music.librarySong.Play();
                SFXManager.instance.magicAmbient.Stop();
                break;
            case Song.ForestSong:
                music.themeSong.Stop();
                music.villageSong.Stop();
                music.treantSong.Stop();
                music.librarySong.Stop();
                if (!music.forestSong.isPlaying)
                    music.forestSong.Play();
                if (!SFXManager.instance.ambient.isPlaying)
                    SFXManager.instance.ambient.Play();
                SFXManager.instance.magicAmbient.Stop();
                break;
        }
    }
}
