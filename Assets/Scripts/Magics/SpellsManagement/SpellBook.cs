using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook
{
    public event EventHandler OnSpellBookChanged;

    private Spell[] spellList;
    private Action<Spell> useSpellAction;

    public SpellBook(Action<Spell> useSpellAction)
    {
        this.useSpellAction = useSpellAction;
        spellList = new Spell[8];
    }

    public bool IsArrayFull()
    {
        for (int i = 0; i < spellList.Length; i++)
        {
            if (spellList[i] == null)
                return false;
        }
        return true;
    }

    public bool AvailableToAdd(Spell spell)
    {
        if (GameManager.instance.xpPoints > 0)
        {
            for (int i = 0; i < spellList.Length; i++)
            {
                if (spell.SpellRank() == 1)
                    return true;

                if (spellList[i] == null)
                    continue;

                if (spell.SpellElement() == spellList[i].SpellElement())
                {
                    if (spell.SpellRank() == spellList[i].SpellRank() + 1)
                        return true;
                }
            }
            return false;
        }
        return false;
    }

    public bool SpellAlreadyInList(Spell spellToAdd)
    {
        foreach (Spell spell in spellList)
        {
            if (spell != null)
                if (spell.spellType == spellToAdd.spellType)
                    return true;
        }
        return false;
    }

    public void AddSpell(Spell spell)
    {
        if (AvailableToAdd(spell)) 
        {
            if (!SpellAlreadyInList(spell))
            {
                for (int i = 0; i < spellList.Length; i++)
                {
                    if (spellList[i] == null)
                    {
                        spellList[i] = spell;
                        GameManager.instance.xpPoints--;

                        OnSpellBookChanged?.Invoke(this, EventArgs.Empty);
                        return;
                    }
                }
            }
        }
    }

    public Spell[] GetSpellList()
    {
        return spellList;
    }

    public void SetSpellList(Spell[] spellList)
    {
        this.spellList = spellList;
    }

}
