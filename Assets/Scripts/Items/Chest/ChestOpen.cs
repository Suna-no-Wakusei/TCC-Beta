using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour, ICollectable
{
    public ScriptableChest chest;
    public event Action OnPause;
    public event Action OnEndPause;

    public static ChestOpen instance { get; private set; }

    void Awake()
    {
        instance = this;
        chest.animator = this.GetComponent<Animator>();
    }

    void Start()
    {
        if (chest.animator != null)
        {
            if (chest.chestAlreadyOpened)
                chest.animator.SetBool("Opened", true);
            else
                chest.animator.SetBool("Opened", false);
        }  
    }

    private bool IsThereGoldenKey()
    {
        Item[] itemList = GameManager.instance.inventory.GetItemList();
        if(!GameManager.instance.inventory.IsArrayEmpty())
        {
            for (int i = 0; i < itemList.Length; i++)
            {
                if(itemList[i] != null)
                {
                    if (itemList[i].itemType == Item.ItemType.GoldenKey)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool IsThereGreyKey()
    {
        Item[] itemList = GameManager.instance.inventory.GetItemList();
        if (!GameManager.instance.inventory.IsArrayEmpty())
        {
            for (int i = 0; i < itemList.Length; i++)
            {
                if (itemList[i].itemType == Item.ItemType.GreyKey)
                {
                    GameManager.instance.inventory.RemoveItem(itemList[i]);
                    return true;
                }
            }
        }
        return false;
    }

    private void OpenChest()
    {
        if (!chest.chestAlreadyOpened)
        {
            GameManager.instance.sfxManager.PlayChestOpening();
            double n = 1;
            if(chest.animator != null)
                chest.animator.SetTrigger("Opening");

            if (chest.weapon)
            {
                if (chest.animator != null)
                    StartCoroutine(GettingWeapon());
                GameManager.instance.weaponLevel = chest.weaponLevel;
            }
            foreach (var item in chest.items)
            {
                if (item.IsStackable() && GameManager.instance.inventory.ItemAlreadyInInventory(item))
                {
                    if (!GameManager.instance.inventory.ItemFull(item))
                    {
                        //When Player touch the item
                        GameManager.instance.inventory.AddItem(item);
                        GameManager.instance.ShowText("+" + item.amount + " " + item.ItemName(), 20, Color.white, transform.position + new Vector3(0, (float)n), Vector3.up * 35, 1f);
                        n = n + 0.4;
                    }
                }
                else if (!GameManager.instance.inventory.IsArrayFull())
                {
                    //When Player touch the item
                    GameManager.instance.inventory.AddItem(item);
                    GameManager.instance.ShowText("+" + item.amount + " " + item.ItemName(), 20, Color.white, transform.position + new Vector3(0, (float)n), Vector3.up * 35, 1f);
                    n = n + 0.4;
                }
            }
            chest.chestAlreadyOpened = true;
        }
    }

    public void Collect()
    {
        if (chest.needKey)
        {
            if(chest.typeKey == ScriptableChest.TypeKey.GoldenKey && IsThereGoldenKey())
            {
                OpenChest();
                Item[] itemList = GameManager.instance.inventory.GetItemList();
                if (!GameManager.instance.inventory.IsArrayEmpty())
                {
                    for (int i = 0; i < itemList.Length; i++)
                    {
                        if (itemList[i] != null)
                        {
                            if (itemList[i].itemType == Item.ItemType.GoldenKey)
                            {
                                GameManager.instance.inventory.RemoveItem(itemList[i]);
                                continue;
                            }
                        }
                    }
                }
            }
            else if (chest.typeKey == ScriptableChest.TypeKey.GreyKey && IsThereGreyKey())
            {
                OpenChest();
                Item[] itemList = GameManager.instance.inventory.GetItemList();
                if (!GameManager.instance.inventory.IsArrayEmpty())
                {
                    for (int i = 0; i < itemList.Length; i++)
                    {
                        if (itemList[i].itemType == Item.ItemType.GreyKey)
                        {
                            GameManager.instance.inventory.RemoveItem(itemList[i]);
                            continue;
                        }
                    }
                }
            }
        }
        else
        {
            OpenChest();
        }
    }

    IEnumerator GettingWeapon()
    {
        chest.animator.SetBool("GetWeapon", true);
        OnPause?.Invoke();
        yield return null;
        chest.animator.SetBool("GetWeapon", false);
        yield return new WaitForSeconds(8f);
        OnEndPause?.Invoke();
    }
}
