using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering;

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
        sfxManager = GameObject.FindWithTag("SFX").GetComponent<SFXManager>();

        if (sfxManager == null)
            sfxManager = new SFXManager();

        SaveSystem.LoadState();
        hero.transform.position = playerPos;

        actualScene = SceneManager.GetActiveScene().name;

        if (actualScene == "Projeto")
            sfxManager.PlayAmbient();
        else
            sfxManager.StopAmbient();

        StartCoroutine(FadeStart());
    }

    public IEnumerator FadeStart()
    {
        if (fadeInPanel != null)
        {
            Transform transform = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            Destroy(transform.gameObject);
        }
        
    }

    // Resources
    public List<ScriptableDialog> scriptableDialogs;
    public List<ScriptableChest> scriptableChests;
    public List<ScriptableEnemy> scriptableEnemies;

    //UI
    public Texture2D cursorDefault;
    public Transform fadeInPanel;

    // References
    public Player hero;
    public SuperClassMagic allMagics;
    public Vector2 nextScenePos;
    public Vector2 playerPos;
    public Inventory inventory;
    public InventoryUI uiInventory;
    public SpellBook spellBook;
    public SpellBarUI spellBarUI;
    public ObjectiveManager objectiveManager;
    public ObjectivePanel objectiveUI;
    public GameObject nullObjective;
    public GameObject nullSpells;
    public GameObject spellBookUI;
    public GameObject deathScreen;
    public int playerMode;
    public Sprite akemiMode, tamakiMode;
    public Image playerModeHolder;
    public Volume globalVolume;
    public VolumeProfile tamakiProfile, akemiProfile;
    public SFXManager sfxManager;

    //Sounds
    public enum FloorType
    {
        Grass, TallGrass, Wood, Earth
    }

    public FloorType floorType;
    public FloorType levelType;

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
    public int fragments;
    public float experience;
    public int xpPoints;
    public int maxExp = 50;
    public int level;
    public int weaponLevel = 0;
    public int selectedMagic;
    public float health;
    public float maxHP;
    public float maxMana;
    public float currentMana;
    public int magicFactor;
    public int attackFactor;
    public int magicProficiency = 0;

    //Floating text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    //Using items
    public void UseItem(Item item)
    {
        GameManager.instance.sfxManager.PlayItem();

        switch (item.itemType)
        {
            case Item.ItemType.LargeHealthPotion:
                if (health == maxHP) return;
                health += maxHP / 2;
                inventory.RemoveItem(new Item { itemType = Item.ItemType.LargeHealthPotion, amount = 1 });
                break;
            case Item.ItemType.MediumHealthPotion:
                if (health == maxHP) return;
                health += 75;
                inventory.RemoveItem(new Item { itemType = Item.ItemType.MediumHealthPotion, amount = 1 });
                break;
            case Item.ItemType.SmallHealthPotion:
                if (health == maxHP) return;
                health += 50;
                inventory.RemoveItem(new Item { itemType = Item.ItemType.SmallHealthPotion, amount = 1 });
                break;
            case Item.ItemType.LargeManaPotion:
                if (currentMana == maxMana) return;
                currentMana = maxMana;
                inventory.RemoveItem(new Item { itemType = Item.ItemType.LargeManaPotion, amount = 1 });
                break;
            case Item.ItemType.MediumManaPotion:
                if (currentMana == maxMana) return;
                currentMana += maxMana * 75/100;
                inventory.RemoveItem(new Item { itemType = Item.ItemType.MediumManaPotion, amount = 1 });
                break;
            case Item.ItemType.SmallManaPotion:
                if (currentMana == maxMana) return;
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
        //Loading binds
        string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);

        if (string.IsNullOrEmpty(rebinds)) { return; }

        hero.PlayerInput.actions.LoadBindingOverridesFromJson(rebinds);

        //UI
        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Confined;

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
    }

    private void Update()
    {
        if (state == GameState.Dialog)
        {
            hero.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            hero.timeRunning = false;
            DialogueManager.Instance.HandleUpdate();
        }

        //SpellManager
        if(magicProficiency == 0)
        {
            spellBookUI.SetActive(false);
            nullSpells.SetActive(true);
        }
        else
        {
            spellBookUI.SetActive(true);
            nullSpells.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        //Player States
        if(state == GameState.FreeRoam)
        {
            hero.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            hero.timeRunning = true;
            hero.HandleUpdate();
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

        hpSlider.fillAmount = health/maxHP;

        //Player Position
        playerPos = hero.transform.position;

        //Player Level
        expSlider.value = experience;
        expSlider.maxValue = maxExp;

        switch (playerMode)
        {
            case 0:
                playerModeHolder.sprite = tamakiMode;
                attackFactor = 2;
                magicFactor = 1;
                break;
            case 1:
                playerModeHolder.sprite = akemiMode;
                attackFactor = 1;
                magicFactor = 2;
                break;
        }

        switch (weaponLevel)
        {
            case 0:
                break;
            case 1:
                hero.attackDamage = 1;
                break;
            case 2:
                hero.attackDamage = 5;
                break;
            case 3:
                hero.attackDamage = 10;
                break;
            case 4:
                hero.attackDamage = 20;
                break;
        }
        

        switch (level)
        {
            case 0:
                maxExp = 50;
                maxMana = 10;
                maxHP = 100;

                manaSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(200, manaSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxManaUI.sizeDelta = new Vector2(200, maxManaUI.sizeDelta.y);

                hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(200, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxHPUI.sizeDelta = new Vector2(200, maxHPUI.sizeDelta.y);
                break;
            case 1:
                maxExp = 100;
                maxHP = 200;

                hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(240, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxHPUI.sizeDelta = new Vector2(240, maxHPUI.sizeDelta.y);
                break;
            case 2:
                maxExp = 200;
                maxHP = 300;

                hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(280, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxHPUI.sizeDelta = new Vector2(280, maxHPUI.sizeDelta.y);
                break;
            case 3:
                maxExp = 400;
                maxHP = 400;

                hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(320, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxHPUI.sizeDelta = new Vector2(320, maxHPUI.sizeDelta.y);
                break;
            case 4:
                maxExp = 800;
                maxHP = 500;

                hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(360, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxHPUI.sizeDelta = new Vector2(360, maxHPUI.sizeDelta.y);
                break;
            case 5:
                maxExp = 1600;
                maxHP = 600;

                hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(400, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxHPUI.sizeDelta = new Vector2(400, maxHPUI.sizeDelta.y);
                break;
            case 6:
                maxExp = 3200;
                maxHP = 700;

                hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(440, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxHPUI.sizeDelta = new Vector2(440, maxHPUI.sizeDelta.y);
                break;
            case 7:
                experience = maxExp;
                maxHP = 800;

                hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(480, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxHPUI.sizeDelta = new Vector2(480, maxHPUI.sizeDelta.y);
                break;
            case 8:
                experience = maxExp;
                maxHP = 900;

                hpSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(520, hpSlider.GetComponent<RectTransform>().sizeDelta.y);
                maxHPUI.sizeDelta = new Vector2(520, maxHPUI.sizeDelta.y);
                break;
        }

        if (experience >= maxExp)
        {
            if (level == 8)
                return;
            maxHP += 100;
            health += 100;
            xpPoints++;
            level++;
            experience -= maxExp;
        }

        //Player Health
        if (health > maxHP)
        {
            health = maxHP;
        }
    }

    public void PlayerMode(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }

        if(!hero.timeRunning) { return; }

        if (playerMode == 0)
        {
            sfxManager.StopAmbient();
            sfxManager.PlayMagicAmbient();
            playerMode = 1;
            //Loading Global Volume
            globalVolume.profile = akemiProfile; 
        }
        else
        {
            sfxManager.StopMagicAmbient();
            if(actualScene == "Projeto")
                sfxManager.PlayAmbient();
            playerMode = 0;
            //Loading Global Volume
            globalVolume.profile = tamakiProfile;
        }
    }

    public void UseFItem(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }

        string pressedButton = ((KeyControl)ctx.control).keyCode.ToString().Substring(1);

        if (inventory.GetItemList()[int.Parse(pressedButton) - 1] != null)
            UseItem(inventory.GetItemList()[int.Parse(pressedButton) - 1]);
    }
}
