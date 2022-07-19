using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IDropHandler
{
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
}
