using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemDescUI : MonoBehaviour
{
    public static ItemDescUI instance { get; private set; }

    private TextMeshProUGUI[] textMeshPro;
    private RectTransform rectTransform;

    private void Awake()
    {
        instance = this;

        textMeshPro = new TextMeshProUGUI[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            textMeshPro[i] = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
        }

    }

    public static ItemDescUI ShowItemDescription(Vector2 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemDescUI, position, Quaternion.identity);
        transform.SetParent(GameObject.Find("Inventory").transform);

        transform.localScale = new Vector3((float)1, (float)1, (float)1);

        ItemDescUI itemDescUI = transform.GetComponent<ItemDescUI>();
        itemDescUI.SetItemDesc(item);

        return itemDescUI;
    }

    public void SetItemDesc(Item item)
    {
        string itemTitle = item.ItemName();
        string itemDescription = item.ItemDesc();

        textMeshPro[0].SetText(itemTitle);
        textMeshPro[1].SetText(itemDescription);
    }

    public void DestroyDescBox()
    {
        if(gameObject != null)
            Destroy(gameObject);
    }
}
