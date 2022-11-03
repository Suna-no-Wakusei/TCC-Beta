using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpellUI : MonoBehaviour
{
    public static SpellUI instance { get; private set; }

    public Spell spell;
    private SpellBook spellBook;
    private Image image;

    private void Awake()
    {
        instance = this;

        GameObject tempObject = GameObject.Find("InventoryCanvas");

        image = GetComponent<Image>();
    }

    public static SpellUI InstanceSpellIcon(Vector3 position, Spell spell, int slotNumber)
    {
        Transform transform = Instantiate(SpellAssets.Instance.pfSpellUI, position, Quaternion.identity);
        transform.SetParent(GameObject.Find("Slot" + slotNumber.ToString()).transform);

        transform.localScale = new Vector3((float)0.875, (float)0.975, 1);

        SpellUI spellUI = transform.GetComponent<SpellUI>();
        spellUI.SetSpell(spell);

        return spellUI;
    }

    public void SetSpell(Spell spell)
    {
        this.spell = spell;
        image.sprite = spell.GetSprite();
    }
}
