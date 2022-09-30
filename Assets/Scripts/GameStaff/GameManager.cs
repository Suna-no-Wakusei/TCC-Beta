using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public enum GameState {FreeRoam, Dialog, Paused};

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool started;

    public GameState state;

    private void Awake()
    {
        instance = this;

        inventory = new Inventory(UseItem);
        spellBook = new SpellBook(UseSpell);
        objectiveManager = new ObjectiveManager();

        SaveSystem.LoadState();
        hero.transform.position = playerPos;

        actualScene = SceneManager.GetActiveScene().name;
    }

    // Resources
    public List<ScriptableDialog> scriptableDialogs;

    // References
    public Player hero;
    public Enemy[] enemies;
    public SuperClassMagic allMagics;
    public Vector2 nextScenePos;
    public Vector2 playerPos;
    public Inventory inventory;
    public InventoryUI uiInventory;
    public SpellBook spellBook;
    public SpellBarUI spellBarUI;
    public ObjectiveManager objectiveManager;
    public ObjectivePanel objectiveUI;

    //public weapon weapon...
    public FloatingTextManager floatingTextManager;
    public TeleportPoint teleport;
    public Slider expSlider;
    public SlicedFilledImage manaSlider, hpSlider;
    public RectTransform maxHPUI, maxManaUI;

    //Actual scene
    public string actualScene;

    // Logic
    public int coins;
    public float experience;
    public int xpPoints;
    public int maxExp = 50;
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
    public void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.LargeHealthPotion:
                hero.hp += maxHP / 2;
                inventory.RemoveItem(new Item { itemType = Item.ItemType.LargeHealthPotion, amount = 1 });
                break;
            case Item.ItemType.MediumHealthPotion:
                hero.hp += 75;
                inventory.RemoveItem(new Item { itemType = Item.ItemType.MediumHealthPotion, amount = 1 });
                break;
            case Item.ItemType.SmallHealthPotion:
                hero.hp += 50;
                inventory.RemoveItem(new Item { itemType = Item.ItemType.SmallHealthPotion, amount = 1 });
                break;
            case Item.ItemType.LargeManaPotion:
                currentMana = maxMana;
                inventory.RemoveItem(new Item { itemType = Item.ItemType.LargeManaPotion, amount = 1 });
                break;
            case Item.ItemType.MediumManaPotion:
                currentMana += maxMana * 75/100;
                inventory.RemoveItem(new Item { itemType = Item.ItemType.MediumManaPotion, amount = 1 });
                break;
            case Item.ItemType.SmallManaPotion:
                currentMana += maxMana / 2;
                inventory.RemoveItem(new Item { itemType = Item.ItemType.SmallManaPotion, amount = 1 });
                break;
            case Item.ItemType.GreyKey:
                break;
            case Item.ItemType.GoldenKey:
                break;
        }
    }

    //Using spells
    public void UseSpell(Spell spell)
    {
        switch (spell.spellType)
        {
            case Spell.SpellType.Fireball:
                selectedMagic = 1;
                break;
            case Spell.SpellType.Zap:
                selectedMagic = 2;
                break;
            case Spell.SpellType.Waterball:
                selectedMagic = 3;
                break;
            case Spell.SpellType.IceShard:
                selectedMagic = 4;
                break;
            case Spell.SpellType.StoneCannon:
                selectedMagic = 5;
                break;
            case Spell.SpellType.Speed:
                selectedMagic = 6;
                break;
        }
    }

    private void Start()
    {
        selectedMagic = 0;
        objectiveUI.SetObjective(objectiveManager);
        //Inventory UI
        uiInventory.SetInventory(inventory);
        //Spell Bar UI
        spellBarUI.SetSpellBook(spellBook);
        //Mana Slider
        manaSlider.fillAmount = currentMana/maxMana;
        //Experience Slider
        expSlider.value = experience;
        expSlider.maxValue = maxExp;
        //HP Slider
        hpSlider.fillAmount = health/maxHP;

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
        SpellManager.Instance.OnPause += () =>
        {
            state = GameState.Paused;
        };

        SpellManager.Instance.OnEndPause += () =>
        {
            if (state == GameState.Paused)
                state = GameState.FreeRoam;
        };
    }

    private void Update()
    {
        //Player States

        if(state == GameState.FreeRoam)
        {
            hero.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            hero.timeRunning = true;
            hero.HandleUpdate();
        }
        else if(state == GameState.Dialog)
        {
            hero.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            hero.timeRunning = false;
            DialogueManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Paused)
        {
            hero.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            hero.timeRunning = false;
        }

        //Player Mana
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }

        manaSlider.fillAmount = currentMana/maxMana;

        //Player Health
        if (hero.hp > maxHP)
        {
            hero.hp = maxHP;
        }

        hero.maxHP = maxHP;
        health = hero.hp;

        hpSlider.fillAmount = health/maxHP;

        //Player Position
        playerPos = hero.transform.position;

        //Player Level
        expSlider.value = experience;
        expSlider.maxValue = maxExp;

        //Using Items by Shortcuts
        for(int i = 1; i <= 5; i++)
        {
            string key = "f" + i.ToString();
            if (Input.GetKeyDown(key))
            {
                if(inventory.GetItemList()[i - 1] != null)
                    UseItem(inventory.GetItemList()[i - 1]);
            }
        }
        

        switch (level)
        {
            case 0:
                maxExp = 50;
                maxMana = 10;

                manaSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(200, manaSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxManaUI.sizeDelta = new Vector2(200, maxManaUI.sizeDelta.y);

                hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(200, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxHPUI.sizeDelta = new Vector2(200, maxHPUI.sizeDelta.y);
                break;
            case 1:
                maxExp = 100;

                hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(240, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxHPUI.sizeDelta = new Vector2(240, maxHPUI.sizeDelta.y);
                break;
            case 2:
                maxExp = 200;

                hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(280, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxHPUI.sizeDelta = new Vector2(280, maxHPUI.sizeDelta.y);
                break;
            case 3:
                maxExp = 400;

                hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(320, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxHPUI.sizeDelta = new Vector2(320, maxHPUI.sizeDelta.y);
                break;
            case 4:
                maxExp = 800;

                hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(360, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxHPUI.sizeDelta = new Vector2(360, maxHPUI.sizeDelta.y);
                break;
            case 5:
                maxExp = 1600;

                hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(400, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxHPUI.sizeDelta = new Vector2(400, maxHPUI.sizeDelta.y);
                break;
            case 6:
                maxExp = 3200;

                hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(440, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxHPUI.sizeDelta = new Vector2(440, maxHPUI.sizeDelta.y);
                break;
            case 7:
                experience = maxExp;

                hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(480, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxHPUI.sizeDelta = new Vector2(480, maxHPUI.sizeDelta.y);
                break;
            case 8:
                experience = maxExp;

                hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(520, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxHPUI.sizeDelta = new Vector2(520, maxHPUI.sizeDelta.y);
                break;
        }

        if (experience >= maxExp)
        {
            if (level == 8)
                return;
            experience -= maxExp;
            level++;
            maxHP += 100;
            hero.hp += 100;
            xpPoints++;
        }
    }
}
