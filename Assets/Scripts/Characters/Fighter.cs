using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    //Public fields
    public int hp = 10;
    public int maxHP = 10;
    public float pushRecoverySpeed = 0.2f;

    //Immunity
    protected float immuneTime = 1.0f;
    protected float lastImmune;

    //Push
    protected Vector3 pushDirection;

    //All fighters can hit / die
    protected virtual void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hp -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            GameManager.instance.ShowText(dmg.damageAmount.ToString(),8,Color.red,transform.position, Vector3.up * 25, 0.5f);

            if(hp <= 0)
            {
                hp = 0;
                Death();
            }
        }
    }

    protected virtual void Death()
    {

    }
}
