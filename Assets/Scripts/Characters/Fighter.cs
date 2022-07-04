using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Fighter : MonoBehaviour
{
    //Public fields
    public float hp;
    public float maxHP;
    public float thrust;
    public float knockTime;

    //Private fields
    private Rigidbody2D rb1;
    private Vector2 difference;

    //Immunity
    protected float immuneTime = 2.0f;
    protected float immuneEnemyTime = 0.5f;
    protected float lastImmune;
    protected float lastEnemyImmune;

    bool immune;

    //All fighters can hit / die

    protected virtual void ReceiveDamage(Damage dmg)
    {
        //Player taking damage
        if ((Time.time - lastImmune > immuneTime) && transform.tag == "Player")
        {
            lastImmune = Time.time;
            hp -= dmg.damageAmount;

            GameManager.instance.ShowText(dmg.damageAmount.ToString(),8,Color.red,transform.position, Vector3.up * 25, 0.5f);

            if(hp <= 0)
            {
                hp = 0;
                Death();
            }

            //Knockback

            difference = transform.position - dmg.origin;
            transform.position = new Vector2(transform.position.x + difference.x, transform.position.y + difference.y);

            //Blinking Immunity Effect

            StartCoroutine(BlinkingImmune());
        }
        //Enemy taking damage
        else if ((Time.time - lastEnemyImmune > immuneEnemyTime) && transform.tag != "Player"){
            lastEnemyImmune = Time.time;
            hp -= dmg.damageAmount;

            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 8, Color.red, transform.position, Vector3.up * 25, 0.5f);

            if (hp <= 0)
            {
                hp = 0;
                Death();
            }

            //Knockback

            difference = transform.position - dmg.origin;
            transform.position = new Vector2(transform.position.x + difference.x, transform.position.y + difference.y);
        }
    }

    IEnumerator BlinkingImmune()
    {
        for(int i = 0; i < 5; i++)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.2f);
            GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
    }

    protected virtual void Death()
    {

    }
}
