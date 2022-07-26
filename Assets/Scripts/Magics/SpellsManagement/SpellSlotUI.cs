using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
        spellSlot = GameObject.FindGameObjectsWithTag("SpellSlot");
    }

    Dictionary<KeyCode, System.Action> keyCodeDic = new Dictionary<KeyCode, System.Action>();

    void Start()
    {
        //Register Keycodes to to match each function to call
        const int alphaStart = 49;
        const int alphaEnd = 56;

        int paramValue = 0;
        for (int i = alphaStart; i <= alphaEnd; i++)
        {
            KeyCode tempKeyCode = (KeyCode)i;

            //Use temp variable to prevent it from being capture
            int temParam = paramValue;
            keyCodeDic.Add(tempKeyCode, () => MethodCall(temParam));
            paramValue++;
        }
    }

    void MethodCall(int keyNum)
    {
        for (int i = 0; i < spellSlot.Length; i++)
        {
            if (spellSlot[i].GetComponent<SpellSlotUI>().slotSelected)
            {
                spellSlot[i].GetComponent<SpellSlotUI>().slotSelected = false;
            }
        }
        spellSlot[keyNum].GetComponent<SpellSlotUI>().slotSelected = true;
    }

    private void Update()
    {
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


        //Loop through the Dictionary and check if the Registered Keycode is pressed
        foreach (KeyValuePair<KeyCode, System.Action> entry in keyCodeDic)
        {
            //Check if the keycode is pressed
            if (Input.GetKeyDown(entry.Key))
            {
                //Check if the key pressed exist in the dictionary key
                if (keyCodeDic.ContainsKey(entry.Key))
                {
                    //Call the function stored in the Dictionary's value
                    keyCodeDic[entry.Key].Invoke();
                }
            }
        }
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
