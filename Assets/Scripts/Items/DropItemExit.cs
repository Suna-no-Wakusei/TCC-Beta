using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropItemExit : MonoBehaviour, IDropHandler
{
    private Item item;
    private ItemUI itemUI;
    private GameObject thisItem;

    public void OnDrop(PointerEventData eventData)
    {
        List<GameObject> hoveredList = eventData.hovered;
        foreach (var GO in hoveredList)
        {
            if (GO.name == "DropCatcher")
            {
                thisItem = eventData.pointerDrag.gameObject;
                itemUI = thisItem.GetComponent<ItemUI>();

                item = itemUI.item;

                // Drop Item
                Destroy(thisItem);

                Item duplicateItem = new Item { itemType = item.itemType, amount = item.amount };
                GameManager.instance.inventory.RemoveItem(item);
                ItemWorld.DropItem(GameObject.Find("Hero").transform.position, duplicateItem);

            }
        }
    }
}
