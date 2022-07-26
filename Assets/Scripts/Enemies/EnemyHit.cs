using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public int damagePoint = 5;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Damage dmg = new Damage
            {
                damageAmount = damagePoint,
                origin = transform.position,
                dmgType = Damage.DmgType.physicalDamage
            };

            collision.SendMessage("ReceiveDamage", dmg, SendMessageOptions.DontRequireReceiver);
        }
    }
}
