using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public bool isOpen;
    public event Action OnPause;
    public event Action OnEndPause;
    private int menuState;
    [SerializeField] private GameObject inventoryPanel, inventoryMenu, spellsMenu, missionsMenu, configsMenu, iconInventory, iconSpells, iconMissions, iconConfigs;
    [SerializeField] private Sprite invIcon, spellsIcon, missionsIcon, configsIcon;
    [SerializeField] private Sprite invIconSelected, spellsIconSelected, missionsIconSelected, configsIconSelected;


    public static InventoryManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (menuState)
        {
            case 0:
                inventoryMenu.SetActive(true);
                spellsMenu.SetActive(false);
                missionsMenu.SetActive(false);
                configsMenu.SetActive(false);
                iconInventory.GetComponent<Image>().sprite = invIconSelected;
                iconSpells.GetComponent<Image>().sprite = spellsIcon;
                iconMissions.GetComponent<Image>().sprite = missionsIcon;
                iconConfigs.GetComponent<Image>().sprite = configsIcon;
                break;
            case 1:
                inventoryMenu.SetActive(false);
                spellsMenu.SetActive(true);
                missionsMenu.SetActive(false);
                configsMenu.SetActive(false);
                iconInventory.GetComponent<Image>().sprite = invIcon;
                iconSpells.GetComponent<Image>().sprite = spellsIconSelected;
                iconMissions.GetComponent<Image>().sprite = missionsIcon;
                iconConfigs.GetComponent<Image>().sprite = configsIcon;
                break;
            case 2:
                inventoryMenu.SetActive(false);
                spellsMenu.SetActive(false);
                missionsMenu.SetActive(true);
                configsMenu.SetActive(false);
                iconInventory.GetComponent<Image>().sprite = invIcon;
                iconSpells.GetComponent<Image>().sprite = spellsIcon;
                iconMissions.GetComponent<Image>().sprite = missionsIconSelected;
                iconConfigs.GetComponent<Image>().sprite = configsIcon;
                break;
            case 3:
                inventoryMenu.SetActive(false);
                spellsMenu.SetActive(false);
                missionsMenu.SetActive(false);
                configsMenu.SetActive(true);
                iconInventory.GetComponent<Image>().sprite = invIcon;
                iconSpells.GetComponent<Image>().sprite = spellsIcon;
                iconMissions.GetComponent<Image>().sprite = missionsIcon;
                iconConfigs.GetComponent<Image>().sprite = configsIconSelected;
                break;
        }
    }

    public void OpenIt(InputAction.CallbackContext ctx)
    {
        if (GameManager.instance.state != GameState.Dialog)
        {
            isOpen = !isOpen;

            if (isOpen)
            {
                inventoryPanel.SetActive(true);
                OnPause?.Invoke();
                Time.timeScale = 0f;
            }
            else
            {
                CloseIt();
            }
        }
    }

    public void CloseIt()
    {
        inventoryPanel.SetActive(false);
        OnEndPause?.Invoke();
        Time.timeScale = 1f;
    }

    public void ChangeMenu(int i)
    {
        menuState = i;
    }
}
