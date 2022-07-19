using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState {FreeRoam, Dialog, Paused};

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool started;

    GameState state;

    private void Awake()
    {
        instance = this;
        actualScene = SceneManager.GetActiveScene().name;

        SceneManager.sceneLoaded += LoadState;
    }

    // Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> Magics;

    // References
    public Player hero;
    public SuperClassMagic allMagics;
    public Inventory inventory;
    public InventoryUI uiInventory;

    //public weapon weapon...
    public FloatingTextManager floatingTextManager;
    public TeleportPoint teleport;
    public Slider manaSlider, expSlider, hpSlider;

    //Actual scene
    public string actualScene = null;

    // Logic
    public int coins;
    public float experience;
    public int maxExp;
    public int level;
    public int selectedMagic;
    public float health;
    public float maxHP;
    public float maxMana;
    public float currentMana;

    //Floating text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    //Using items
    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion:
                inventory.RemoveItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
                break;
            case Item.ItemType.ManaPotion:
                inventory.RemoveItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 1 });
                break;
            case Item.ItemType.Axe:
                inventory.RemoveItem(new Item { itemType = Item.ItemType.Axe, amount = 1 });
                break;
        }
    }

    //Save State
    public void SaveState()
    {
        string s = "";

        /* INT PreferedSkin
         * INT Coins
         * FLOAT Experience
         * INT Level
         * INT Magic selected
         * FLOAT Current mana
         * FLOAT HP
         * INT WeaponExperience
         */

        s += "0" + "|";
        s += coins.ToString() + "|";
        s += experience.ToString() + "|";
        s += level.ToString() + "|";
        s += selectedMagic.ToString() + "|";
        s += currentMana.ToString() + "|";
        s += health.ToString() + "|";
        s += "0";

        SaveInventory();

        PlayerPrefs.SetString("SaveState", s);
    }

    public void SaveInventory()
    {
        //Saving the inventory items on PlayerPrefs

        string j = "";

        for (int i = 0; i < inventory.GetItemList().Length; i++)
        {
            Item item = inventory.GetItemList()[i];

            if(item == null)
            {
                if (i != 14)
                {
                    j += "null" + "|";
                    continue;
                }
                else
                {
                    j += "null";
                    continue;
                }
            }

            if (i == 14)
            {
                j += item.itemType + "/" + item.amount.ToString();
                continue;
            }

            j += item.itemType.ToString() + "/" + item.amount.ToString() + "|";
        }

        PlayerPrefs.SetString("SaveInventory", j);
    }

    private void Start()
    {
        if (!started)
        {
            //Player level
            level = 0;
            experience = 0;
            //Mana
            currentMana = instance.maxMana;
            //HP
            health = instance.maxHP;

            started = true;
        }
        //Inventory UI
        uiInventory.SetInventory(inventory);
        //Mana Slider
        manaSlider.value = currentMana;
        manaSlider.maxValue = maxMana;
        //Experience Slider
        expSlider.value = experience;
        expSlider.maxValue = maxExp;
        //HP Slider
        hpSlider.value = health;
        hpSlider.maxValue = maxHP;
        //Player States
        DialogueManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };

        DialogueManager.Instance.OnCloseDialog += () =>
        {
            if (state == GameState.Dialog)
                state = GameState.FreeRoam;
        };

        hero.OnAttack += () =>
        {
            state = GameState.Paused;
        };

        hero.OnEndAttack += () =>
        {
            if (state == GameState.Paused)
                state = GameState.FreeRoam;
        };
        InventoryManager.Instance.OnPause += () =>
        {
            state = GameState.Paused;
        };

        InventoryManager.Instance.OnEndPause += () =>
        {
            if (state == GameState.Paused)
                state = GameState.FreeRoam;
        };
    }
    public void LoadState(Scene s, LoadSceneMode mode)
    {
        if(!PlayerPrefs.HasKey("SaveState"))
            return;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        //Change player skin
        coins = int.Parse(data[1]);
        experience = float.Parse(data[2]);
        level = int.Parse(data[3]);
        selectedMagic = int.Parse(data[4]);
        currentMana = float.Parse(data[5]);
        health = float.Parse(data[6]);
        //Change weapon level

        //Update things
        expSlider.value = experience;
        manaSlider.value = currentMana;
        hpSlider.value = health;
        GameObject.Find("Hero").transform.position = teleport.initialValue;
        LoadInventory();
    }

    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey("SaveInventory"))
            return;

        string[] data = PlayerPrefs.GetString("SaveInventory").Split('|');

        Item[] tempItemList;
        tempItemList = new Item[15];

        inventory = new Inventory(UseItem);

        for (int i = 0; i < inventory.GetItemList().Length; i++)
        {
            if (data[i] == "null")
            {
                continue;
            }

            string[] itemPart = data[i].Split('/');

            Item.ItemType itemTypeItem = (Item.ItemType)System.Enum.Parse(typeof(Item.ItemType), itemPart[0]);
            int amountItem = int.Parse(itemPart[1]);

            Item itemTemp = new Item();
            itemTemp.amount = amountItem;
            itemTemp.itemType = itemTypeItem;

            tempItemList[i] = itemTemp;
        }

        inventory.SetItemList(tempItemList);
    }

    private void Update()
    {
        //Player States

        if(state == GameState.FreeRoam)
        {
            hero.timeRunning = true;
            hero.HandleUpdate();
        }
        else if(state == GameState.Dialog)
        {
            hero.timeRunning = false;
            DialogueManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Paused)
        {
            hero.timeRunning = false;
        }

        //Player Mana
        manaSlider.value = currentMana;

        //Player Health
        hpSlider.value = health;
        hpSlider.maxValue = maxHP;

        health = hero.hp;
        hero.maxHP = maxHP;

        //Player Level
        expSlider.value = experience;
        expSlider.maxValue = maxExp;

        switch (level)
        {
            case 0:
                maxExp = 50;
                maxHP = 100;
                break;
            case 1:
                maxExp = 100;
                maxHP = 150;
                break;
            case 2:
                maxExp = 200;
                maxHP = 200;
                break;
            case 3:
                maxExp = 400;
                maxHP = 350;
                break;
            case 4:
                maxExp = 800;
                maxHP = 400;
                break;
            case 5:
                maxExp = 1600;
                maxHP = 450;
                break;
            case 6:
                maxExp = 3200;
                maxHP = 500;
                break;
            case 7:
                maxExp = 6400;
                maxHP = 600;
                break;
        }

        if (experience >= maxExp)
        {
            if (level == 7)
                return;
            experience -= maxExp;
            level++;
        }
    }
}
