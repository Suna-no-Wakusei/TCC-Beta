using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;

    private Item[] itemList;
    private Action<Item> useItemAction;

    public Inventory(Action<Item> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemList = new Item[20];
    }

    public bool IsArrayEmpty()
    {
        for (int i = 0; i < itemList.Length; i++)
        {
            if (itemList[i] != null)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsArrayFull()
    {
        for (int i = 0; i < itemList.Length; i++)
        {
            if (itemList[i] == null)
                return false;
        }
        return true;
    }

    public bool ItemAlreadyInInventory(Item itemToAdd)
    {
        foreach(Item item in itemList)
        {
            if (item != null)
                if (item.itemType == itemToAdd.itemType)
                    return true;
        }
        return false;
    }

    public bool ItemFull(Item itemToAdd)
    {
        foreach (Item item in itemList)
        {
            if (item != null)
                if (item.IsStackable())
                    if(item.itemType == itemToAdd.itemType)
                        if(item.amount == 24)
                            return true;
        }
        return false;
    }

    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if(inventoryItem != null)
                {
                    if (inventoryItem.itemType == item.itemType)
                    {
                        if(inventoryItem.amount < 24)
                        {
                            inventoryItem.amount += item.amount;
                            itemAlreadyInInventory = true;
                        }
                    }
                }
            }
            if (!itemAlreadyInInventory)
            {
                for(int i = 0; i < itemList.Length; i++)
                {
                    if (itemList[i] == null)
                    {
                        itemList[i] = item;
                        return;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < itemList.Length; i++)
            {
                if (itemList[i] == null)
                {
                    itemList[i] = item;
                    return;
                }
            }
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem != null)
                {
                    if (inventoryItem.itemType == item.itemType)
                    {
                        inventoryItem.amount -= item.amount;
                        itemInInventory = inventoryItem;
                    }
                }
            }
            if (itemInInventory != null && itemInInventory.amount <= 0)
            {
                for (int i = 0; i < itemList.Length; i++)
                {
                    if (itemList[i] == itemInInventory)
                    {
                        itemList[i] = null;
                        return;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < itemList.Length; i++)
            {
                if (itemList[i] == item)
                {
                    itemList[i] = null;
                    return;
                }
            }
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseItem(Item item)
    {
        useItemAction(item);
    }

    public Item[] GetItemList()
    {
        return itemList;
    }

    public void SetItemList(Item[] inventoryList)
    {
        itemList = inventoryList;
    }

}
