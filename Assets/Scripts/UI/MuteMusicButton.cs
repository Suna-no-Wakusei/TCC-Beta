using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MuteMusicButton : MonoBehaviour
{
    public Toggle musicVolTog;
    public AudioMixer mixer;
    public Sprite mutedAudio, defaultAudio;

    private void Start()
    {
        mixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        mixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));

        float j;
        mixer.GetFloat(("MusicVol"), out j);

        if(j == -80)
        {
            musicVolTog.isOn = true;
            musicVolTog.GetComponent<Image>().sprite = mutedAudio;
        }
        else
        {
            musicVolTog.isOn = false;
            musicVolTog.GetComponent<Image>().sprite = defaultAudio;
        }
    }

    void LateUpdate()
    {
        float j;
        mixer.GetFloat(("MusicVol"), out j);

        if (musicVolTog.isOn)
        {
            mixer.SetFloat("MusicVol", -80);
            musicVolTog.GetComponent<Image>().sprite = mutedAudio;
            PlayerPrefs.SetFloat("MusicVol", -80);
        }
        else
        {
            mixer.SetFloat("MusicVol", 0);
            PlayerPrefs.SetFloat("MusicVol", 0);
            musicVolTog.GetComponent<Image>().sprite = defaultAudio;
        }

        if (j == -80)
            musicVolTog.GetComponent<Image>().sprite = mutedAudio;
        else
            musicVolTog.GetComponent<Image>().sprite = defaultAudio;

    }
}
