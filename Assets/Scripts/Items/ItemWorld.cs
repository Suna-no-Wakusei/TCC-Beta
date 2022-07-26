using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;

public class ItemWorld : MonoBehaviour, ICollectable
{

    public void Collect()
    {
        ItemWorld itemWorld = GetComponent<BoxCollider2D>().GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            Item item = itemWorld.GetItem();
            if (item.IsStackable() && GameManager.instance.inventory.ItemAlreadyInInventory(item))
            {
                if (!GameManager.instance.inventory.ItemFull(item))
                {
                    //When Player touch the item
                    GameManager.instance.inventory.AddItem(item);
                    itemWorld.DestroySelf();
                }
            }
            else if (!GameManager.instance.inventory.IsArrayFull())
            {
                //When Player touch the item
                GameManager.instance.inventory.AddItem(item);
                itemWorld.DestroySelf();
            }
        }
    }

    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }

    public static ItemWorld DropItem(Vector3 dropPosition, Item item)
    {
        Vector3 randomDir = UtilsClass.GetRandomDir();
        ItemWorld itemWorld = SpawnItemWorld(dropPosition + randomDir * 1.5f, item);
        itemWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 1.5f, ForceMode2D.Impulse);
        return itemWorld;
    }

    private Item item;
    private SpriteRenderer spriteRenderer;
    private TextMeshPro textMeshPro;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();

        if(item.amount > 1)
        {
            textMeshPro.SetText(item.amount.ToString());
        }
        else
        {
            textMeshPro.SetText("");
        }
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
