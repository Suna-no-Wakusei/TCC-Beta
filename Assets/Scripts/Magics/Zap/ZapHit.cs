using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapHit : MonoBehaviour
{
    public SpriteRenderer zapSR;
    public Transform zapLight;

    public int damagePoint;

    public Vector2 collisionPos;

    public void OnTriggerEnter2D(Collider2D coll)
    {
        collisionPos = coll.gameObject.GetComponent<Collider2D>().ClosestPoint(GameManager.instance.hero.transform.position);

        if (coll.gameObject.CompareTag("Hittable") && coll.gameObject.layer != gameObject.layer)
        {
            Damage dmg = new Damage
            {
                damageAmount = damagePoint * GameManager.instance.magicFactor,
                origin = transform.position,
                dmgType = Damage.DmgType.magicDamage
            };

            coll.transform.parent.SendMessage("ReceiveDamage", dmg, SendMessageOptions.DontRequireReceiver);

            zapSR.size = new Vector2(Vector2.Distance(collisionPos, zapSR.transform.position), (float)0.6875);
            zapLight.localScale = new Vector2(0.3f * Vector2.Distance(collisionPos, zapSR.transform.position), zapLight.localScale.y);
        }
        else if (coll.gameObject.layer != gameObject.layer)
        {
            if (coll.gameObject.layer != 8)
            {
                zapSR.size = new Vector2(Vector2.Distance(collisionPos, zapSR.transform.position), (float)0.6875);
                zapLight.localScale = new Vector2(0.3f * Vector2.Distance(collisionPos, zapSR.transform.position), zapLight.localScale.y);
            }
        }
    }
}
