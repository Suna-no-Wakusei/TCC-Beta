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
    private bool kinematico;

    //Immunity
    protected float immuneTime = 2.0f;
    protected float lastImmune;

    //All fighters can hit / die
    protected virtual void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
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

            rb1 = this.GetComponent<Rigidbody2D>();
                
            difference = rb1.transform.position - dmg.origin;
            rb1.transform.position = new Vector2(rb1.transform.position.x + difference.x, rb1.transform.position.y + difference.y);
        }
    }

    protected virtual void Death()
    {

    }
}
