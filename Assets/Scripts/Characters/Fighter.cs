using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    //Public fields
    public float hp;
    public float maxHP;

    //Private fields
    private Rigidbody2D rb1;
    private Vector2 difference;

    //Immunity
    protected float immuneTime = 1.0f;
    protected float immuneEnemyTime = 0.5f;
    protected float lastImmune;
    protected float lastEnemyImmune;

    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;

    public bool characterUnableToMove = false;

    protected bool immune;
    protected bool damaged;

    //All fighters can hit / die

    private void Awake()
    {
        rb1 = GetComponent<Rigidbody2D>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
    }

    protected virtual void ReceiveDamage(Damage dmg)
    {
        //Player taking damage
        if ((Time.time - lastImmune > immuneTime) && transform.tag == "Player")
        {
            lastImmune = Time.time;
            GameManager.instance.health -= dmg.damageAmount;

            CameraShake.Shake(0.25f, 0.25f);

            GameManager.instance.ShowText(dmg.damageAmount.ToString(),20,Color.red,transform.position, Vector3.up * 25, 0.5f);

            if(GameManager.instance.health <= 0)
            {
                GameManager.instance.health = 0;
                Death();
            }

            //Knockback
            difference = transform.position - dmg.origin;
            StartCoroutine(Knockback());

            //Blinking Immunity Effect

            StartCoroutine(BlinkingImmune());

            GameManager.instance.sfxManager.PlayHumanHurt();
        }
        //Enemy taking damage
        else if ((Time.time - lastEnemyImmune > immuneEnemyTime) && transform.tag != "Player"){
            lastEnemyImmune = Time.time;
            hp -= dmg.damageAmount;
            StartCoroutine(BlinkingDamage());

            CameraShake.Shake(0.25f, 0.25f);

            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 18, Color.red, transform.position, Vector3.up * 25, 0.5f);

            if (hp <= 0)
            {
                hp = 0;
                Death();
            }

            GameManager.instance.sfxManager.PlayMobHit();

            if (dmg.dmgType == Damage.DmgType.physicalDamage)
            {
                //Knockback
                difference = transform.position - dmg.origin;
                StartCoroutine(Knockback());
            }
            else
            {
                //Magic Knockback
                difference = transform.position - dmg.origin;
                StartCoroutine(EnemyMagicKnockback());
            }
        }
    }

    IEnumerator BlinkingDamage()
    {
        damaged = true;
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        damaged = false;
    }

    IEnumerator BlinkingImmune()
    {
        immune = true;
        for (int i = 0; i < 5; i++)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        immune = false;
    }

    IEnumerator Knockback()
    {
        if (rb1 != null)
        {
            characterUnableToMove = true;

            rb1.AddForce(difference * 10f, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.3f);

            characterUnableToMove = false;
        }
    }

    IEnumerator EnemyMagicKnockback()
    {
        if (rb1 != null)
        {
            characterUnableToMove = true;

            rb1.AddForce(difference * 6f, ForceMode2D.Impulse);

            yield return new WaitForSeconds(1f);

            rb1.velocity = Vector2.zero;

            characterUnableToMove = false;
        }
    }

    protected virtual void Death()
    {

    }
}
