using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;
    private GameObject[] itemSlot;
    private TextMeshProUGUI[] textMeshPro;
    private GameObject[] itemPF;

    private void Awake()
    {
        itemSlot = new GameObject[15];
        textMeshPro = new TextMeshProUGUI[15];
        itemPF = new GameObject[15];

        itemSlot = GameObject.FindGameObjectsWithTag("ItemSlot");
    }

    private void Update()
    {
        InventoryVerify();
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        InventoryManager.Instance.OnPause += Inventory_OnItemListChanged;

        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged()
    {
        for (int i = 0; i < inventory.GetItemList().Length; i++)
        {
            if (inventory.GetItemList()[i] == null) continue;
            Debug.Log(inventory.GetItemList()[i].itemType);
            Debug.Log(inventory.GetItemList()[i].amount);
            Debug.Log(i);
        }

        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        if (itemSlot == null)
            return;

        for (int i = 0; i < inventory.GetItemList().Length; i++)
        {
            if (inventory.GetItemList()[i] == null) continue;

            Item item = inventory.GetItemList()[i];

            if (itemSlot[i].transform.childCount >= 3)
            {
                if (item.amount == 1 && textMeshPro[i] != null)
                {
                    textMeshPro[i].SetText("");
                }
                else if (textMeshPro[i] != null)
                {
                    textMeshPro[i].SetText(item.amount.ToString());
                }
                continue;
            }

            ItemUI.CatchItem(itemSlot[i].transform.position, item, i.ToString());
        }
    }

    private void InventoryVerify()
    {
        if (itemSlot == null)
            return;

        for (int i = 0; i < inventory.GetItemList().Length; i++)
        {
            if (itemSlot[i].transform.childCount >= 3)
            {
                if(inventory.GetItemList()[i] == null)
                {
                    for (int y = 0; y < inventory.GetItemList().Length; y++)
                    {
                        if((itemSlot[y].transform.childCount <= 2))
                        {
                            if (inventory.GetItemList()[y] != null)
                            {
                                Item item = inventory.GetItemList()[y];

                                inventory.GetItemList()[i] = item;
                                inventory.GetItemList()[y] = null;


                            }
                        }
                    }
                }

                itemPF[i] = itemSlot[i].transform.Find("ItemUIpf(Clone)").gameObject;
                textMeshPro[i] = itemPF[i].transform.Find("AmountText").GetComponent<TextMeshProUGUI>();
            }
        }
    }
}
