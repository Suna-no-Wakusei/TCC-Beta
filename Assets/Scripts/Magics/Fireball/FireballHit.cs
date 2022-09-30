using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballHit : MonoBehaviour
{
    public Fireball fireball;

    public int damagePoint;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hittable") && collision.gameObject.layer != gameObject.layer)
        {
            Damage dmg = new Damage
            {
                damageAmount = damagePoint,
                origin = transform.position,
                dmgType = Damage.DmgType.magicDamage
            };

            collision.transform.parent.SendMessage("ReceiveDamage", dmg, SendMessageOptions.DontRequireReceiver);
            StartCoroutine(Colisor());
        }
        else if (collision.gameObject.layer != gameObject.layer)
        {
            if(collision.gameObject.layer != 8)
                StartCoroutine(Colisor());
        }
    }

    IEnumerator Colisor()
    {
        fireball.animator.SetTrigger("Collide");

        fireball.fireballRb.velocity = Vector3.zero;
        fireball.StopFireball();
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);

        fireball.fireballRunning = false;
    }
}
