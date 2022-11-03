using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneCannonHit : MonoBehaviour
{
    public StoneCannon stoneCannon;

    public int damagePoint;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hittable") && collision.gameObject.layer != gameObject.layer)
        {
            Damage dmg = new Damage
            {
                damageAmount = damagePoint * GameManager.instance.magicFactor,
                origin = transform.position,
                dmgType = Damage.DmgType.magicDamage
            };

            collision.transform.parent.SendMessage("ReceiveDamage", dmg, SendMessageOptions.DontRequireReceiver);
            StartCoroutine(Colisor());
        }
        else if (collision.gameObject.layer != gameObject.layer && collision.gameObject.tag != "NotSolid")
        {
            if (collision.gameObject.layer != 8)
                StartCoroutine(Colisor());
        }
    }

    IEnumerator Colisor()
    {
        GameManager.instance.hero.characterUnableToMove = false;
        stoneCannon.StopStoneCannon();

        stoneCannon.animator.SetTrigger("Collide");

        stoneCannon.stoneCannonRb.velocity = Vector3.zero;
        
        yield return new WaitForSeconds(0.4f);
        gameObject.SetActive(false);

        stoneCannon.stoneCannonRunning = false;
    }
}
