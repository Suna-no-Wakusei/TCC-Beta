using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperClassMagic : MonoBehaviour
{
    public Fireball fireball;

    public float regenRate = 0.25f;
    private float manaCost;

    void Start()
    {
        
    }

    void Update()
    {
        //Mana regen
        if(GameManager.instance.currentMana < GameManager.instance.maxMana)
            ManaRegen();

        //Running Magics
        if(GameManager.instance.hero.timeRunning == true)
        {
            if (GameManager.instance.selectedMagic == 1)
            {
                manaCost = 3f;
                if (GameManager.instance.currentMana >= manaCost)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse1) && !fireball.fireballRunning)
                    {
                        fireball.PlayFireball();
                        DecreaseMana();
                    }
                }
            }
        }
    }

    public void ManaRegen()
    {
        GameManager.instance.currentMana = Mathf.Min(GameManager.instance.currentMana + regenRate * Time.deltaTime, GameManager.instance.maxMana);
        GameManager.instance.manaSlider.value = Mathf.Min(GameManager.instance.manaSlider.value + regenRate * Time.deltaTime, GameManager.instance.manaSlider.maxValue);
    }

    public void DecreaseMana()
    {
        if (GameManager.instance.manaSlider.value < 0)
        {
            GameManager.instance.manaSlider.value = 0;
            GameManager.instance.currentMana = 0;
        }
        GameManager.instance.manaSlider.value -= manaCost;
        GameManager.instance.currentMana -= manaCost;
    }
}
