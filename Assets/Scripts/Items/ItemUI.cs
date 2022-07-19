using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    public static ItemUI instance { get; private set; }

    public Item item;
    private Inventory inventory;
    private Image image;
    private TextMeshProUGUI textMeshPro;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        instance = this;

        GameObject tempObject = GameObject.Find("InventoryCanvas");
        if (tempObject != null)
        {
            canvas = tempObject.GetComponent<Canvas>();
        }

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        image = GetComponent<Image>();

        foreach(Transform child in transform)
        {
            textMeshPro = child.GetComponent<TextMeshProUGUI>();
        }
    }

    public static ItemUI CatchItem(Vector3 position, Item item, string slotNumber)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemUI, position, Quaternion.identity);
        transform.SetParent(GameObject.Find(slotNumber).transform);

        transform.localScale = new Vector3((float)0.9, (float)0.9, (float)0.9);

        ItemUI itemUI = transform.GetComponent<ItemUI>();
        itemUI.SetItem(item);

        return itemUI;
    }

    public void SetItem(Item item)
    {
        this.item = item;
        image.sprite = item.GetSprite();

        if (item.amount > 1)
        {
            textMeshPro.SetText(item.amount.ToString());
        }
        else
        {
            textMeshPro.SetText("");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

}
