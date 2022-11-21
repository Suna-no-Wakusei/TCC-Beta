using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class SpellSlotUI : MonoBehaviour, IPointerDownHandler
{
    public bool slotSelected;
    private GameObject[] spellSlot;
    private Image image;
    public Sprite selector;
    private Sprite defaultSprite;

    public void Awake()
    {
        image = transform.GetComponent<Image>();
        defaultSprite = image.sprite;
    }

    private void Update()
    {
        spellSlot = SpellBarUI.instance.spellSlot;

        if (!slotSelected)
        {
            image.sprite = defaultSprite;
        }
        else
        {
            image.sprite = selector;
        }

        int w = 0;
        for (int i = 0; i < spellSlot.Length; i++)
        {
            if (spellSlot[i].GetComponent<SpellSlotUI>().slotSelected)
                w = 1;
        }

        if(w == 0)
            spellSlot[0].GetComponent<SpellSlotUI>().slotSelected = true;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        for (int i = 0; i < spellSlot.Length; i++)
        {
            if (spellSlot[i].GetComponent<SpellSlotUI>().slotSelected)
            {
                spellSlot[i].GetComponent<SpellSlotUI>().slotSelected = false;
            }
        }

        slotSelected = true;

        image = transform.GetComponent<Image>();

        image.sprite = selector;
    }
}
