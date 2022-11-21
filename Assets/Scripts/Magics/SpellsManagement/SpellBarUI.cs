using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class SpellBarUI : MonoBehaviour
{
    public static SpellBarUI instance;

    private SpellBook spellBook;
    public GameObject[] spellSlot = new GameObject[8];
    private GameObject[] spellPF;

    private void Awake()
    {
        spellPF = new GameObject[8];

        instance = this;
    }

    void Update()
    {
        if(spellBook != null)
            UseSelectedSpell();
    }

    public void SpellBarSelect(InputAction.CallbackContext ctx)
    {
        int spellNumber = 0;

        for (int i = 0; i < spellSlot.Length; i++)
        {
            if (spellSlot[i].GetComponent<SpellSlotUI>().slotSelected)
            {
                spellSlot[i].GetComponent<SpellSlotUI>().slotSelected = false;
            }
        }

        for(int i = 0; i < ctx.action.controls.ToArray().Length; i++)
        {
            if (ctx.control == ctx.action.controls[i])
            {
                spellNumber = i;
                continue;
            }
        }

        spellSlot[spellNumber].GetComponent<SpellSlotUI>().slotSelected = true;
    }

    public void UseSelectedSpell()
    {
        Spell spell = new Spell();

        for (int i = 0; i < spellSlot.Length; i++)
        {
            if (spellSlot[i].GetComponent<SpellSlotUI>().slotSelected)
            {
                spell = spellBook.GetSpellList()[i];
            }
        }

        if (spell != null)
        {
            GameManager.instance.UseSpell(spell);
        }
        else
        {
            GameManager.instance.selectedMagic = 0;
        }

    }

    public void SetSpellBook(SpellBook spellBook)
    {
        this.spellBook = spellBook;

        spellBook.OnSpellBookChanged += SpellBook_OnSpellListChanged;

        RefreshBookSpells();
    }

    private void SpellBook_OnSpellListChanged(object sender, EventArgs args)
    {
        for (int i = 0; i < spellBook.GetSpellList().Length; i++)
        {
            if (spellBook.GetSpellList()[i] == null) continue;
        }

        RefreshBookSpells();
    }

    private void RefreshBookSpells()
    {
        if (spellSlot == null)
            return;

        for (int i = 0; i < spellBook.GetSpellList().Length; i++)
        {
            Spell spell = spellBook.GetSpellList()[i];

            if (spellBook.GetSpellList()[i] == null) continue;

            if(spellSlot[i].transform.childCount == 0)
                SpellUI.InstanceSpellIcon(spellSlot[i].transform.position, spell, i + 1);
        }
    }
}
