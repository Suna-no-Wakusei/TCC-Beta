using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IDropHandler, IPointerDownHandler
{
    public bool slotSelected;
    private GameObject[] itemSlot;
    private int thisSlot;
    public Sprite selector;
    public Sprite emptySlot;

    public void Awake()
    {
        itemSlot = GameObject.FindGameObjectsWithTag("ItemSlot");

        thisSlot = int.Parse(gameObject.name);

        if (thisSlot == 0)
            slotSelected = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if(gameObject.transform.Find("ItemUIpf(Clone)") == null)
            {
                eventData.pointerDrag.gameObject.transform.SetParent(gameObject.transform);
                eventData.pointerDrag.GetComponent<Transform>().position = GetComponent<Transform>().position;
            }
            else if(gameObject.transform.Find("ItemUIpf(Clone)") == eventData.pointerDrag.gameObject.transform)
            {
                eventData.pointerDrag.GetComponent<Transform>().position = GetComponent<Transform>().position;
            }
            else if(gameObject.transform.Find("ItemUIpf(Clone)") != eventData.pointerDrag.gameObject.transform)
            {
                gameObject.transform.Find("ItemUIpf(Clone)").position = gameObject.transform.Find("ItemUIpf(Clone)").parent.position;
                eventData.pointerDrag.gameObject.transform.position = eventData.pointerDrag.gameObject.transform.parent.position;
            }
        }
    }

    private void Update()
    {
        if (!slotSelected)
        {
            Image[] image = new Image[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                image[i] = transform.GetChild(i).GetComponent<Image>();
            }

            image[1].sprite = emptySlot;
        }
        else
        {
            Image[] image = new Image[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                image[i] = transform.GetChild(i).GetComponent<Image>();
            }

            image[1].sprite = selector;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].GetComponent<ItemSlotUI>().slotSelected)
            {
                itemSlot[i].GetComponent<ItemSlotUI>().slotSelected = false;
            }
        }

        slotSelected = true;

        Image[] image = new Image[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            image[i] = transform.GetChild(i).GetComponent<Image>();
        }

        image[1].sprite = selector;
    }
}
