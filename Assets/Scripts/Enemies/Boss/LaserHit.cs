using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHit : MonoBehaviour
{
    public SpriteRenderer laserSR;
    public Transform laserLight;

    public int damagePoint;

    public Vector2 collisionPos;

    public void OnTriggerEnter2D(Collider2D coll)
    {
        collisionPos = coll.gameObject.GetComponent<Collider2D>().ClosestPoint(GameManager.instance.hero.transform.position);

        if (coll.gameObject.CompareTag("Player") && coll.gameObject.layer != gameObject.layer)
        {
            Damage dmg = new Damage
            {
                damageAmount = damagePoint * GameManager.instance.magicFactor,
                origin = transform.position,
                dmgType = Damage.DmgType.magicDamage
            };

            coll.transform.SendMessage("ReceiveDamage", dmg, SendMessageOptions.DontRequireReceiver);

            laserSR.size = new Vector2(8, Vector2.Distance(collisionPos, laserSR.transform.position) + 5.75f);
            laserLight.localScale = new Vector2(0.1f * (Vector2.Distance(collisionPos, laserSR.transform.position) + 5.75f), laserLight.localScale.y);
        }
        else if (coll.gameObject.layer != gameObject.layer && coll.gameObject.tag != "NotSolid")
        {
            if (coll.gameObject.layer != 8)
            {
                laserSR.size = new Vector2(8, Vector2.Distance(collisionPos, laserSR.transform.position) + 5.75f);
                laserLight.localScale = new Vector2(0.1f * (Vector2.Distance(collisionPos, laserSR.transform.position) + 5.75f), laserLight.localScale.y);
            }
        }
    }
}
