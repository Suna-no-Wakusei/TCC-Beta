using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IconDesc : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject iconDesc;

    public void OnPointerEnter(PointerEventData eventData)
    {
        iconDesc.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        iconDesc.SetActive(false);
    }
}
