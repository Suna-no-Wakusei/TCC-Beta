using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class OptionsScreen : MonoBehaviour
{
    public Toggle fullscreenTog, vsyncTog, mastVolTog, musicVolTog, sfxVolTog;

    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedResolution;

    public TMP_Text resolutionLabel;

    public AudioMixer mixer;
    public Sprite mutedAudio, defaultAudio;

    public TMP_Text mastLabel, musicLabel, sfxLabel;
    public Slider mastSlider, musicSlider, sfxSlider;

    public GameObject waitingForInputObject;
    public GameObject[] rebindingButtons;

    // Start is called before the first frame update
    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        }
        else
        {
            vsyncTog.isOn = true;
        }

        bool foundRes = false;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;

                selectedResolution = i;

                UpdateResLabel();
            }
        }

        if (!foundRes)
        {
            selectedResolution = 0;

            UpdateResLabel();
        }

        float vol = 0f;
        mixer.GetFloat(("MasterVol"), out vol);
        mastSlider.value = vol;
        mixer.GetFloat(("MusicVol"), out vol);
        musicSlider.value = vol;
        mixer.GetFloat(("SFXVol"), out vol);
        sfxSlider.value = vol;

        mastLabel.SetText(Mathf.RoundToInt((mastSlider.value + 20) * 5).ToString());
        musicLabel.SetText(Mathf.RoundToInt((musicSlider.value + 20) * 5).ToString());
        sfxLabel.SetText(Mathf.RoundToInt((sfxSlider.value + 20) * 5).ToString());

        //Rebinding System Loading
        for (int i = 0; i < rebindingButtons.Length; i++)
        {
                int bindingIndex = rebindingButtons[i].GetComponent<RebindingButtons>().input.action.GetBindingIndexForControl(rebindingButtons[i].GetComponent<RebindingButtons>().input.action.controls[0]);
                rebindingButtons[i].GetComponentInChildren<TMP_Text>().text = InputControlPath.ToHumanReadableString(rebindingButtons[i].GetComponent<RebindingButtons>().input.action.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);

                if (rebindingButtons[i].GetComponent<RebindingButtons>().input.action.bindings[rebindingButtons[i].GetComponent<RebindingButtons>().inputBindingIndex].isPartOfComposite)
                {
                    rebindingButtons[i].GetComponentInChildren<TMP_Text>().text = InputControlPath.ToHumanReadableString(rebindingButtons[i].GetComponent<RebindingButtons>().input.action.bindings[rebindingButtons[i].GetComponent<RebindingButtons>().inputBindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
                }
            }
    }

    public void ResLeft()
    {
        selectedResolution--;
        if (selectedResolution < 0)
        {
            selectedResolution = 0;
        }

        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedResolution++;
        if (selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = resolutions.Count - 1;
        }

        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " X " + resolutions[selectedResolution].vertical.ToString();
    }

    public void ApplyGraphics()
    {
        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenTog.isOn);
    }


    public void SetMasterVol()
    {
        mastLabel.SetText(Mathf.RoundToInt((mastSlider.value + 20) * 5).ToString());

        if (mastSlider.value > -20)
        {
            mixer.SetFloat("MasterVol", mastSlider.value);
        }
        else if (mastSlider.value == -20)
        {
            mixer.SetFloat("MasterVol", -80);
        }

        PlayerPrefs.SetFloat("MasterVol", mastSlider.value);

    }

    public void SetMusicVol()
    {
        musicLabel.SetText(Mathf.RoundToInt((musicSlider.value + 20) * 5).ToString());

        if (musicSlider.value > -20)
        {
            mixer.SetFloat("MusicVol", musicSlider.value);
        }
        else if (musicSlider.value == -20)
        {
            mixer.SetFloat("MusicVol", -80);
        }

        PlayerPrefs.SetFloat("MusicVol", musicSlider.value);
    }

    public void SetSFXVol()
    {
        sfxLabel.SetText(Mathf.RoundToInt((sfxSlider.value + 20) * 5).ToString());

        if (sfxSlider.value > -20)
        {
            mixer.SetFloat("SFXVol", sfxSlider.value);
        }
        else if (sfxSlider.value == -20)
        {
            mixer.SetFloat("SFXVol", -80);
        }

        PlayerPrefs.SetFloat("SFXVol", sfxSlider.value);
    }

    void Update()
    {
        float i = 0;
        float j = 0;
        float n = 0;
        mixer.GetFloat(("MasterVol"), out i);
        mixer.GetFloat(("MusicVol"), out j);
        mixer.GetFloat(("SFXVol"), out n);

        if (mastVolTog.isOn)
        {
            mixer.SetFloat("MasterVol", -80);
            mastVolTog.GetComponent<Image>().sprite = mutedAudio;
            PlayerPrefs.SetFloat("MasterVol", -80);
        }
        else
        {
            SetMasterVol();
            mastVolTog.GetComponent<Image>().sprite = defaultAudio;
        }
        if (musicVolTog.isOn)
        {
            mixer.SetFloat("MusicVol", -80);
            musicVolTog.GetComponent<Image>().sprite = mutedAudio;
            PlayerPrefs.SetFloat("MusicVol", -80);
        }
        else
        {
            SetMusicVol();
            musicVolTog.GetComponent<Image>().sprite = defaultAudio;
        }
        if (sfxVolTog.isOn)
        {
            mixer.SetFloat("SFXVol", -80);
            sfxVolTog.GetComponent<Image>().sprite = mutedAudio;
            PlayerPrefs.SetFloat("SFXVol", -80);
        }
        else
        {
            SetSFXVol();
            sfxVolTog.GetComponent<Image>().sprite = defaultAudio;
        }


        if(i == -80)
            mastVolTog.GetComponent<Image>().sprite = mutedAudio;
        else
            mastVolTog.GetComponent<Image>().sprite = defaultAudio;

        if (j == -80)
            musicVolTog.GetComponent<Image>().sprite = mutedAudio;
        else
            musicVolTog.GetComponent<Image>().sprite = defaultAudio;

        if (n == -80)
            sfxVolTog.GetComponent<Image>().sprite = mutedAudio;
        else
            sfxVolTog.GetComponent<Image>().sprite = defaultAudio;
    }


    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    public void StartRebinding(GameObject startRebidingObject)
    {
        startRebidingObject.SetActive(false);
        waitingForInputObject.SetActive(true);

        GameManager.instance.hero.PlayerInput.SwitchCurrentActionMap("Menu");

        rebindingOperation = startRebidingObject.GetComponent<RebindingButtons>().input.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete(startRebidingObject))
            .Start();
    }

    private void RebindComplete(GameObject startRebidingObject)
    {
        int bindingIndex = startRebidingObject.GetComponent<RebindingButtons>().input.action.GetBindingIndexForControl(startRebidingObject.GetComponent<RebindingButtons>().input.action.controls[0]);

        startRebidingObject.GetComponentInChildren<TMP_Text>().text = InputControlPath.ToHumanReadableString(startRebidingObject.GetComponent<RebindingButtons>().input.action.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        rebindingOperation.Dispose();

        startRebidingObject.SetActive(true);
        waitingForInputObject.SetActive(false);

        GameManager.instance.hero.PlayerInput.SwitchCurrentActionMap("Gameplay");

        string rebinds = GameManager.instance.hero.PlayerInput.actions.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString("rebinds", rebinds);
    }

    private InputActionRebindingExtensions.RebindingOperation rebindingOperationMovement;

    public void StartRebindingMovement(GameObject startRebidingObject)
    {
        startRebidingObject.SetActive(false);
        waitingForInputObject.SetActive(true);

        GameManager.instance.hero.PlayerInput.SwitchCurrentActionMap("Menu");

        rebindingOperationMovement = startRebidingObject.GetComponent<RebindingButtons>().input.action.PerformInteractiveRebinding(startRebidingObject.GetComponent<RebindingButtons>().inputBindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation => RebindCompleteMovement(startRebidingObject))
            .Start()
            .OnCancel((x) =>
            {
                // deselect button
                FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
                x.Dispose();
                startRebidingObject.GetComponent<RebindingButtons>().input.action.Enable();
            }); ;

        if (startRebidingObject.GetComponent<RebindingButtons>().input.action.bindings[startRebidingObject.GetComponent<RebindingButtons>().inputBindingIndex].isPartOfComposite)
        {
            rebindingOperationMovement.WithExpectedControlType("Button");
        }
    }

    private void RebindCompleteMovement(GameObject startRebidingObject)
    {
        int bindingIndex = startRebidingObject.GetComponent<RebindingButtons>().input.action.GetBindingIndexForControl(startRebidingObject.GetComponent<RebindingButtons>().input.action.controls[0]);

        startRebidingObject.GetComponentInChildren<TMP_Text>().text = InputControlPath.ToHumanReadableString(startRebidingObject.GetComponent<RebindingButtons>().input.action.bindings[startRebidingObject.GetComponent<RebindingButtons>().inputBindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        rebindingOperationMovement.Dispose();

        startRebidingObject.SetActive(true);
        waitingForInputObject.SetActive(false);

        GameManager.instance.hero.PlayerInput.SwitchCurrentActionMap("Gameplay");

        string rebinds = GameManager.instance.hero.PlayerInput.actions.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString("rebinds", rebinds);
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}