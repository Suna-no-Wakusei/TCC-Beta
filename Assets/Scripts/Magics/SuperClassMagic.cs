using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperClassMagic : MonoBehaviour
{
    public Fireball fireball;
    public Zap zap;
    public Waterball waterball;

    public float regenRate = 0.25f;
    private float manaCost;

    void Update()
    {
        //Mana regen
        if(GameManager.instance.currentMana < GameManager.instance.maxMana)
            ManaRegen();

        //Running Magics
        if(GameManager.instance.hero.timeRunning == true)
        {
            switch (GameManager.instance.selectedMagic)
            {
                case 1:
                    manaCost = 1;
                    if (GameManager.instance.currentMana >= manaCost)
                    {
                        if (Input.GetKeyDown(KeyCode.Mouse1) && !fireball.fireballRunning)
                        {
                            fireball.PlayFireball();
                            DecreaseMana();
                        }
                    }
                    break;
                case 2:
                    manaCost = 1;
                    if (GameManager.instance.currentMana >= manaCost)
                    {
                        if (Input.GetKeyDown(KeyCode.Mouse1) && !zap.zapRunning)
                        {
                            zap.PlayZap();
                            DecreaseMana();
                        }
                    }
                    break;
                case 3:
                    manaCost = 1;
                    if (GameManager.instance.currentMana >= manaCost)
                    {
                        if (Input.GetKeyDown(KeyCode.Mouse1) && !waterball.waterballRunning)
                        {
                            waterball.PlayWaterball();
                            DecreaseMana();
                        }
                    }
                    break;
            }
        }
    }

    public void ManaRegen()
    {
        GameManager.instance.currentMana = Mathf.Min(GameManager.instance.currentMana + regenRate * Time.deltaTime, GameManager.instance.maxMana);
        GameManager.instance.manaSlider.fillAmount = Mathf.Min(GameManager.instance.manaSlider.fillAmount + (regenRate / GameManager.instance.maxMana) * Time.deltaTime, 1);
    }

    public void DecreaseMana()
    {
        if (GameManager.instance.manaSlider.fillAmount < 0)
        {
            GameManager.instance.manaSlider.fillAmount = 0;
            GameManager.instance.currentMana = 0;
        }
        GameManager.instance.manaSlider.fillAmount -= manaCost/ GameManager.instance.maxMana;
        GameManager.instance.currentMana -= manaCost;
    }
}
