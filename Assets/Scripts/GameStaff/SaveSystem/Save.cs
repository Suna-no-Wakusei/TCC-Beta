using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    //Saving Current Scene
    public string sceneName;

    //Saving Player Stats
    public int coins;
    public int fragments;
    public float experience;
    public int level;
    public int xpPoints;
    public int magicProficiency;
    public int selectedMagic;
    public float currentMana;
    public float health;
    public int weaponLevel;

    public float playerPosX;
    public float playerPosY;

    //Saving Player Inventory
    public int[] savedItemAmount = new int[20];
    public string[] savedItemType = new string[20];

    //Saving Current Quest
    public string questTitle;
    public string questDescription;
    public int questReward;

    //Saving Dialog State
    public List<bool> dialogPlayed = new List<bool>();

    //Saving Chest State
    public List<bool> chestOpened = new List<bool>();

    //Saving Enemy State
    public List<bool> enemyAlive = new List<bool>();

    //Saving Spell Book
    public string[] savedSpellType = new string[8];
}
