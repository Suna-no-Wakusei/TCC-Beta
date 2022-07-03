using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public int damagePoint = 1;

    private Vector2 direction;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hittable"))
        {
            Damage dmg = new Damage
            {
                damageAmount = damagePoint,
                origin = transform.position
            };

            collision.transform.parent.SendMessage("ReceiveDamage", dmg, SendMessageOptions.DontRequireReceiver);
        }
    }
}
