using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {FreeRoam, Dialog};

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    bool loaded = false;

    GameState state;

    private void Awake()
    {
        if(GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        DontDestroyOnLoad(gameObject);
    }

    // Ressources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;
    public List<int> Magics; 

    // References
    public Player hero;
    public SuperClassMagic allMagics;
    //public weapon weapon...
    public FloatingTextManager floatingTextManager;
    public TeleportPoint teleport;

    // Logic
    public int coins;
    public int experience;
    public int selectedMagic;

    //Floating text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    //Save State
    public void SaveState()
    {
        string s = "";

        /* INT PreferedSkin
         * INT Coins
         * INT Experience
         * INT Magic selected
         * INT WeaponExperience
         */

        s += "0" + "|";
        s += coins.ToString() + "|";
        s += experience.ToString() + "|";
        s += selectedMagic.ToString() + "|";
        s += "0";

        PlayerPrefs.SetString("SaveState", s);
    }
    private void Start()
    {

        DialogueManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };

        DialogueManager.Instance.OnCloseDialog += () =>
        {
            if (state == GameState.Dialog)
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
        experience = int.Parse(data[2]);
        //Change weapon level
        if(loaded == false)
            teleport.initialValue = new Vector2(2,-1);

        hero.transform.position = teleport.initialValue;

        loaded = true;
    }

    private void Update()
    {
        if(state == GameState.FreeRoam)
        {
            hero.HandleUpdate();
        }
        else if(state == GameState.Dialog)
        {
            DialogueManager.Instance.HandleUpdate();
        }
    }
}
