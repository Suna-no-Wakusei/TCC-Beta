using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ManaShowing : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject manaShowing;
    public TMP_Text manaText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        manaShowing.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        manaShowing.SetActive(false);
    }

    private void Update()
    {
        manaText.text = Math.Round(GameManager.instance.currentMana, 0) + " / " + GameManager.instance.maxMana;
    }
}
