using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SuperClassMagic : MonoBehaviour
{
    public Fireball fireball;
    public Zap zap;
    public Waterball waterball;
    public IceShard iceShard;
    public StoneCannon stoneCannon;
    public Speed speed;

    private bool decreasingMana;
    private bool speeding = true;

    public float regenRate = 0.25f;
    private float manaCost;

    public void UseSpell(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }

        if (GameManager.instance.hero.characterUnableToMove) return;

        //Running Magics
        if (GameManager.instance.hero.timeRunning == true)
        {
            switch (GameManager.instance.selectedMagic)
            {
                case 1:
                    manaCost = 1;
                    speed.speedEffect.Stop();
                    GameManager.instance.hero.moveSpeed = 5f;
                    if (GameManager.instance.currentMana >= manaCost)
                    {
                        if (!fireball.fireballRunning)
                        {
                            fireball.PlayFireball();
                            DecreaseMana();
                        }
                    }
                    break;
                case 2:
                    manaCost = 1;
                    speed.speedEffect.Stop();
                    GameManager.instance.hero.moveSpeed = 5f;
                    if (GameManager.instance.currentMana >= manaCost)
                    {
                        if (!zap.zapRunning)
                        {
                            zap.PlayZap();
                            DecreaseMana();
                        }
                    }
                    break;
                case 3:
                    manaCost = 1;
                    speed.speedEffect.Stop();
                    GameManager.instance.hero.moveSpeed = 5f;
                    if (GameManager.instance.currentMana >= manaCost)
                    {
                        if (!waterball.waterballRunning)
                        {
                            waterball.PlayWaterball();
                            DecreaseMana();
                        }
                    }
                    break;
                case 4:
                    manaCost = 1;
                    speed.speedEffect.Stop();
                    GameManager.instance.hero.moveSpeed = 5f;
                    if (GameManager.instance.currentMana >= manaCost)
                    {
                        if (!iceShard.iceShardRunning)
                        {
                            iceShard.PlayIceShard();
                            DecreaseMana();
                        }
                    }
                    break;
                case 5:
                    manaCost = 1;
                    speed.speedEffect.Stop();
                    GameManager.instance.hero.moveSpeed = 5f;
                    if (GameManager.instance.currentMana >= manaCost)
                    {
                        if (!stoneCannon.stoneCannonRunning)
                        {
                            stoneCannon.PlayStoneCannon();
                            DecreaseMana();
                        }
                    }
                    break;
                case 6:
                    manaCost = 0.005f;
                    if (GameManager.instance.currentMana >= manaCost && speeding == true)
                    {
                        speeding = false;
                        GameManager.instance.hero.moveSpeed = 10f;
                        DecreaseSpeedMana();
                        speed.PlayEffect();
                    }
                    else
                    {
                        speeding = true;
                        speed.speedEffect.Stop();
                        decreasingMana = false;
                        GameManager.instance.hero.moveSpeed = 5f;
                        GameManager.instance.sfxManager.StopWind();
                    }
                    break;
            }
        }
    }

    void Update()
    {
        //Mana regen
        if(GameManager.instance.currentMana < GameManager.instance.maxMana)
            ManaRegen();

        //Mana decrease
        if (decreasingMana)
            DecreaseMana();

        if(GameManager.instance.currentMana <= 0.025f)
        {
            speed.speedEffect.Stop();
            decreasingMana = false;
            GameManager.instance.hero.moveSpeed = 5f;
            GameManager.instance.sfxManager.StopWind();
        }
    }

    public void ManaRegen()
    {
        GameManager.instance.currentMana = Mathf.Min(GameManager.instance.currentMana + regenRate * Time.deltaTime, GameManager.instance.maxMana);
        GameManager.instance.manaSlider.fillAmount = Mathf.Min(GameManager.instance.manaSlider.fillAmount + (regenRate / GameManager.instance.maxMana) * Time.deltaTime, 1);
    }

    private void DecreaseSpeedMana()
    {
        decreasingMana = true;
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
