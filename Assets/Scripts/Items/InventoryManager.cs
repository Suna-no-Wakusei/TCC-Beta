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
    [SerializeField] private GameObject inventoryPanel, inventoryMenu, spellsMenu, missionsMenu, mapMenu, configsMenu, iconInventory, iconSpells, iconMissions, iconMap, iconConfigs, quitMenu;
    [SerializeField] private Sprite invIcon, spellsIcon, missionsIcon, mapIcon, configsIcon;
    [SerializeField] private Sprite invIconSelected, spellsIconSelected, missionsIconSelected, mapIconSelected, configsIconSelected;

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
                mapMenu.SetActive(false);
                configsMenu.SetActive(false);
                iconInventory.GetComponent<Image>().sprite = invIconSelected;
                iconSpells.GetComponent<Image>().sprite = spellsIcon;
                iconMissions.GetComponent<Image>().sprite = missionsIcon;
                iconMap.GetComponent<Image>().sprite = mapIcon;
                iconConfigs.GetComponent<Image>().sprite = configsIcon;
                break;
            case 1:
                inventoryMenu.SetActive(false);
                spellsMenu.SetActive(true);
                missionsMenu.SetActive(false);
                mapMenu.SetActive(false);
                configsMenu.SetActive(false);
                iconInventory.GetComponent<Image>().sprite = invIcon;
                iconSpells.GetComponent<Image>().sprite = spellsIconSelected;
                iconMissions.GetComponent<Image>().sprite = missionsIcon;
                iconMap.GetComponent<Image>().sprite = mapIcon;
                iconConfigs.GetComponent<Image>().sprite = configsIcon;
                break;
            case 2:
                inventoryMenu.SetActive(false);
                spellsMenu.SetActive(false);
                missionsMenu.SetActive(true);
                mapMenu.SetActive(false);
                configsMenu.SetActive(false);
                iconInventory.GetComponent<Image>().sprite = invIcon;
                iconSpells.GetComponent<Image>().sprite = spellsIcon;
                iconMissions.GetComponent<Image>().sprite = missionsIconSelected;
                iconMap.GetComponent<Image>().sprite = mapIcon;
                iconConfigs.GetComponent<Image>().sprite = configsIcon;
                break;
            case 3:
                inventoryMenu.SetActive(false);
                spellsMenu.SetActive(false);
                missionsMenu.SetActive(false);
                mapMenu.SetActive(true);
                configsMenu.SetActive(false);
                iconInventory.GetComponent<Image>().sprite = invIcon;
                iconSpells.GetComponent<Image>().sprite = spellsIcon;
                iconMissions.GetComponent<Image>().sprite = missionsIcon;
                iconMap.GetComponent<Image>().sprite = mapIconSelected;
                iconConfigs.GetComponent<Image>().sprite = configsIcon;
                break;
            case 4:
                inventoryMenu.SetActive(false);
                spellsMenu.SetActive(false);
                missionsMenu.SetActive(false);
                mapMenu.SetActive(false);
                configsMenu.SetActive(true);
                iconInventory.GetComponent<Image>().sprite = invIcon;
                iconSpells.GetComponent<Image>().sprite = spellsIcon;
                iconMissions.GetComponent<Image>().sprite = missionsIcon;
                iconMap.GetComponent<Image>().sprite = mapIcon;
                iconConfigs.GetComponent<Image>().sprite = configsIconSelected;
                break;
        }
    }

    public void OpenMenuQuit()
    {
        quitMenu.SetActive(true);
    }

    public void CloseMenuQuit()
    {
        quitMenu.SetActive(false);
    }

    public void OpenIt(InputAction.CallbackContext ctx)
    {
        if (quitMenu.activeSelf) return;

        if (DialogueManager.Instance.dialogRunning) return;

        if (!GameManager.instance.hero.availableToInteract) return;

        if (GameManager.instance.state != GameState.Dialog)
        {
            isOpen = !isOpen;

            if (isOpen)
            {
                GameManager.instance.hero.characterUnableToMove = true;
                menuState = 0;
                inventoryPanel.SetActive(true);
                OnPause?.Invoke();
            }
            else
            {
                CloseIt();
            }
        }
    }

    public void CloseIt()
    {
        if (GameObject.Find("ItemBox(Clone)") != null)
            Destroy(GameObject.Find("ItemBox(Clone)"));

        if (GameObject.Find("SpellBox(Clone)") != null)
            Destroy(GameObject.Find("SpellBox(Clone)"));

        var foundIconDescs = FindObjectsOfType<IconDesc>();

        foreach (var iconDesc in foundIconDescs)
        {
            foreach(Transform child in iconDesc.transform) child.gameObject.SetActive(false);
        }

        inventoryPanel.SetActive(false);
        OnEndPause?.Invoke();
        GameManager.instance.hero.characterUnableToMove = false;
    }

    public void ChangeMenu(int i)
    {
        menuState = i;
    }
}
