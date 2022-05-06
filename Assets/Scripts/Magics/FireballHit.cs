using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballHit : MonoBehaviour
{
    public Fireball fireball;

    public int damagePoint;
    public float pushForce = 2f;

    public int FireballLevel = 0;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hittable" && collision.gameObject.layer != gameObject.layer)
        {
            Damage dmg = new Damage
            {
                damageAmount = damagePoint,
                pushForce = pushForce,
                origin = transform.position
            };

            collision.SendMessage("ReceiveDamage", dmg, SendMessageOptions.DontRequireReceiver);
            StartCoroutine(Colisor());
        }
        else if (collision.gameObject.layer != gameObject.layer)
        {
            StartCoroutine(Colisor());
        }
    }

    IEnumerator Colisor()
    {
        fireball.animator.SetTrigger("Collide");

        fireball.fireballRb.velocity = Vector3.zero;
        fireball.StopFireball();
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);

        fireball.fireballRunning = false;
    }
}
