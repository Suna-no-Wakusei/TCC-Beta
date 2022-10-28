using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hittable"))
        {
            Damage dmg = new Damage
            {
                damageAmount = GameManager.instance.hero.attackDamage * GameManager.instance.attackFactor,
                origin = transform.position,
                dmgType = Damage.DmgType.physicalDamage
            };

            collision.transform.parent.SendMessage("ReceiveDamage", dmg, SendMessageOptions.DontRequireReceiver);
        }
    }
}
