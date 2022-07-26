using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LearnSpell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Spell spell;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameObject.Find("SpellBox(Clone)") == null)
            SpellsDescUI.ShowItemDescription(this.transform.GetComponent<RectTransform>().position - new Vector3(-250, 130, 0), spell);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameObject.Find("SpellBox(Clone)") != null)
            Destroy(GameObject.Find("SpellBox(Clone)"));
    }

    public void Update()
    {
        if (GameManager.instance.spellBook.AvailableToAdd(spell))
        {
            this.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else
        {
            this.GetComponent<Image>().color = new Color32(46, 46, 46, 255);
        }
    }
}
