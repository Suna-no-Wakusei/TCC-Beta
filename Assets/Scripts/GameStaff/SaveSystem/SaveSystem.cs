using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public static class SaveSystem
{
    private static string savedScene;

    //Creating a Save object to store data from this instance
    private static Save createSaveGameObject()
    {
        Save save = new Save();

        save.coins = GameManager.instance.coins;
        save.fragments = GameManager.instance.fragments;
        save.health = GameManager.instance.health;
        save.currentMana = GameManager.instance.currentMana;
        save.experience = GameManager.instance.experience;
        save.xpPoints = GameManager.instance.xpPoints;
        save.magicProficiency = GameManager.instance.magicProficiency;
        save.level = GameManager.instance.level;
        save.selectedMagic = GameManager.instance.selectedMagic;
        save.weaponLevel = GameManager.instance.weaponLevel;

        save.sceneName = GameManager.instance.actualScene;

        save.playerPosX = GameManager.instance.nextScenePos.x;
        save.playerPosY = GameManager.instance.nextScenePos.y;

        //Saving Inventory items
        for (int i = 0; i < GameManager.instance.inventory.GetItemList().Length; i++)
        {
            Item item = GameManager.instance.inventory.GetItemList()[i];

            if (item == null)
            {
                save.savedItemAmount[i] = 0;
                save.savedItemType[i] = null;
                continue;
            }

            save.savedItemAmount[i] = item.amount;
            save.savedItemType[i] = item.itemType.ToString();
        }

        //Saving Current Quest
        if(GameManager.instance.objectiveManager.GetObjective() != null)
        {
            save.questTitle = GameManager.instance.objectiveManager.GetObjective().title;
            save.questDescription = GameManager.instance.objectiveManager.GetObjective().description;
            save.questReward = GameManager.instance.objectiveManager.GetObjective().reward;
        }
        else
        {
            save.questTitle = "";
            save.questDescription = "";
            save.questReward = 0;
        }

        //Saving Dialog State
        for(int i = 0; i < GameManager.instance.scriptableDialogs.Count; i++)
        {
            save.dialogPlayed.Add(GameManager.instance.scriptableDialogs[i].dialogAlreadyPlayed);
        }

        //Saving Chest State
        for (int i = 0; i < GameManager.instance.scriptableChests.Count; i++)
        {
            save.chestOpened.Add(GameManager.instance.scriptableChests[i].chestAlreadyOpened);
        }

        //Saving Enemy State
        for(int i = 0; i < GameManager.instance.scriptableEnemies.Count; i++)
        {
            save.enemyAlive.Add(GameManager.instance.scriptableEnemies[i].alive);
        }

        //Saving Spells
        for (int i = 0; i < GameManager.instance.spellBook.GetSpellList().Length; i++)
        {
            Spell spell = GameManager.instance.spellBook.GetSpellList()[i];

            if (spell == null)
            {
                Debug.Log(i);
                save.savedSpellType[i] = "";
                continue;
            }

            save.savedSpellType[i] = spell.spellType.ToString();
        }

        return save;
    }

    //Save State
    public static void SaveState()
    {
        Save save = createSaveGameObject();

        string JsonString = JsonUtility.ToJson(save);

        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/JSONData.sus");

        sw.Write(JsonString);
        sw.Close();
    }

    public static void LoadState()
    {
        if (File.Exists(Application.persistentDataPath + "/JSONData.sus"))
        {
            StreamReader sr = new StreamReader(Application.persistentDataPath + "/JSONData.sus");

            string JsonString = sr.ReadToEnd();

            sr.Close();

            //Convert JSON to the Object(save)
            Save save = JsonUtility.FromJson<Save>(JsonString);

            try
            {
                //Loading Data
                GameManager.instance.coins = save.coins;
                GameManager.instance.fragments = save.fragments;
                GameManager.instance.experience = save.experience;
                GameManager.instance.xpPoints = save.xpPoints;
                GameManager.instance.magicProficiency = save.magicProficiency;
                GameManager.instance.level = save.level;
                GameManager.instance.selectedMagic = save.selectedMagic;
                GameManager.instance.currentMana = save.currentMana;
                GameManager.instance.health = save.health;
                GameManager.instance.weaponLevel = save.weaponLevel;

                GameManager.instance.playerPos = new Vector2(save.playerPosX, save.playerPosY);

                //Loading Inventory Items
                Item[] tempItemList;
                tempItemList = new Item[20];

                GameManager.instance.inventory = new Inventory(GameManager.instance.UseItem);

                for (int i = 0; i < GameManager.instance.inventory.GetItemList().Length; i++)
                {
                    if (save.savedItemType[i] == null || save.savedItemAmount[i] == 0)
                    {
                        continue;
                    }

                    Item.ItemType itemTypeItem = (Item.ItemType)System.Enum.Parse(typeof(Item.ItemType), save.savedItemType[i]);

                    Item itemTemp = new Item();
                    itemTemp.amount = save.savedItemAmount[i];
                    itemTemp.itemType = itemTypeItem;

                    tempItemList[i] = itemTemp;
                }

                GameManager.instance.inventory.SetItemList(tempItemList);

                //Loading Quest
                if(save.questTitle != "" && save.questDescription != "")
                {
                    Debug.Log("Não nulo");

                    Objective objective = new Objective();

                    objective.title = save.questTitle;
                    objective.description = save.questDescription;
                    objective.reward = save.questReward;

                    GameManager.instance.objectiveManager.SetObjective(objective);
                }

                //Loading Dialog States
                for (int i = 0; i < GameManager.instance.scriptableDialogs.Count; i++)
                {
                    GameManager.instance.scriptableDialogs[i].dialogAlreadyPlayed = save.dialogPlayed[i];
                }

                //Loading Chest States
                for (int i = 0; i < GameManager.instance.scriptableChests.Count; i++)
                {
                    GameManager.instance.scriptableChests[i].chestAlreadyOpened = save.chestOpened[i];
                }

                //Loading Enemy State
                for (int i = 0; i < GameManager.instance.scriptableEnemies.Count; i++)
                {
                    GameManager.instance.scriptableEnemies[i].alive = save.enemyAlive[i];
                }

                //Loading Spells
                Spell[] tempSpellList;
                tempSpellList = new Spell[8];

                GameManager.instance.spellBook = new SpellBook(GameManager.instance.UseSpell);

                for (int i = 0; i < GameManager.instance.spellBook.GetSpellList().Length; i++)
                {
                    if (save.savedSpellType[i] == "")
                    {
                        Debug.Log("Tá nulo esse spell");
                        continue;
                    }

                    Spell.SpellType spellTypeSpell = (Spell.SpellType)Enum.Parse(typeof(Spell.SpellType), save.savedSpellType[i]);

                    Spell spellTemp = new Spell();
                    spellTemp.spellType = spellTypeSpell;

                    tempSpellList[i] = spellTemp;
                }

                GameManager.instance.spellBook.SetSpellList(tempSpellList);

                //Update things

                GameManager.instance.expSlider.value = GameManager.instance.experience;
            }
            catch(ArgumentException e)
            {
                Debug.Log(e);
            }

            savedScene = save.sceneName;
        }
        else
        {
            //Reseting Scriptable Objects when the file is deleted
            for (int i = 0; i < GameManager.instance.scriptableDialogs.Count; i++)
            {
                GameManager.instance.scriptableDialogs[i].dialogAlreadyPlayed = false;
            }
            for (int i = 0; i < GameManager.instance.scriptableDialogs.Count; i++)
            {
                GameManager.instance.scriptableDialogs[i].dialogStarted = false;
            }
            for (int i = 0; i < GameManager.instance.scriptableChests.Count; i++)
            {
                GameManager.instance.scriptableChests[i].chestAlreadyOpened = false;
            }
            for (int i = 0; i < GameManager.instance.scriptableEnemies.Count; i++)
            {
                GameManager.instance.scriptableEnemies[i].alive = true;
            }
        }
    }

    public static void LoadSavedScene()
    {
        SceneManager.LoadScene(savedScene);
    }

}
