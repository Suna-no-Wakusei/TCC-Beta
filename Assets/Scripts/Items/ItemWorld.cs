using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld instance;

    private bool itemCollectable;
    private Rigidbody2D rb;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.sfxManager.PlayPickupItem();

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
                        Destroy(this.gameObject);
                        if (InventoryManager.Instance.isOpen)
                            GameManager.instance.uiInventory.SetInventory(GameManager.instance.inventory);
                    }
                }
                else if (!GameManager.instance.inventory.IsArrayFull())
                {
                    //When Player touch the item
                    GameManager.instance.inventory.AddItem(item);
                    Destroy(this.gameObject);
                    if (InventoryManager.Instance.isOpen)
                        GameManager.instance.uiInventory.SetInventory(GameManager.instance.inventory);
                }
            }
        }
    }

    public void FixedUpdate()
    {
        if(InventoryManager.Instance.isOpen)
            itemCollectable = false;
        else
            itemCollectable = true;

        if (itemCollectable)
        {
            float distanceChecker = Vector2.Distance(GameManager.instance.hero.transform.position, transform.position);
            Vector2 velocity = (GameManager.instance.hero.transform.position - transform.position).normalized;

            if (distanceChecker <= 2)
            {
                rb.AddForce(velocity * 2, ForceMode2D.Impulse);
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
        GameManager.instance.sfxManager.PlayDropItem();
        Vector3 randomDir = UtilsClass.GetRandomDir();
        ItemWorld itemWorld = SpawnItemWorld(dropPosition + randomDir * 1.1f, item);
        itemWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 6f, ForceMode2D.Impulse);
        return itemWorld;
    }

    private Item item;
    private SpriteRenderer spriteRenderer;
    private TextMeshPro textMeshPro;

    private void Awake()
    {
        instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
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
}
