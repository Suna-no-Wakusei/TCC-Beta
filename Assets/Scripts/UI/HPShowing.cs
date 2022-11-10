using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HPShowing : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hpShowing;
    public TMP_Text hpText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hpText.text = GameManager.instance.health + " / " + GameManager.instance.maxHP;
        hpShowing.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hpShowing.SetActive(false);
    }

    private void Update()
    {
        hpText.text = GameManager.instance.health + " / " + GameManager.instance.maxHP;
    }
}
