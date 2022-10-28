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

    [SerializeField] private TMP_Text coins, fragments, level, attackDamage;
    [SerializeField] private Sprite weapon;

    private void Awake()
    {
        itemSlot = new GameObject[20];
        textMeshPro = new TextMeshProUGUI[20];
        itemPF = new GameObject[20];

        itemSlot = GameObject.FindGameObjectsWithTag("ItemSlot");
    }

    private void Update()
    {
        InventoryVerify();

        coins.text = GameManager.instance.coins.ToString();
        fragments.text = GameManager.instance.fragments.ToString();
        level.text = GameManager.instance.level.ToString();
        attackDamage.text = (GameManager.instance.hero.attackDamage * GameManager.instance.attackFactor).ToString();
    }

    public void UseSelectedItem()
    {
        Item item = new Item();

        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].GetComponent<ItemSlotUI>().slotSelected)
            {
                item = inventory.GetItemList()[i];
            }
        }

        if (item != null)
            GameManager.instance.UseItem(item);

        RefreshInventoryItems();
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
            Item item = inventory.GetItemList()[i];

            if (itemSlot[i].transform.childCount >= 1)
            {
                if (item == null)
                {
                    Destroy(itemSlot[i].transform.Find("ItemUIpf(Clone)").gameObject);

                    continue;
                }

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

            if (inventory.GetItemList()[i] == null) continue;

            ItemUI.CatchItem(itemSlot[i].transform.position, item, i.ToString());
        }
    }

    private void InventoryVerify()
    {
        if (itemSlot == null)
            return;

        for (int i = 0; i < inventory.GetItemList().Length; i++)
        {
            if (itemSlot[i].transform.childCount >= 1)
            {
                if(inventory.GetItemList()[i] == null)
                {
                    for (int y = 0; y < inventory.GetItemList().Length; y++)
                    {
                        if((itemSlot[y].transform.childCount <= 0))
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
