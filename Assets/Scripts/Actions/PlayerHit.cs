using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public int damagePoint = 1;
    public float pushForce = 2f;

    public int FireballLevel = 0;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hittable")
        {
            Damage dmg = new Damage
            {
                damageAmount = damagePoint,
                pushForce = pushForce,
                origin = transform.position
            };

            collision.SendMessage("ReceiveDamage", dmg);
        }
    }
}
