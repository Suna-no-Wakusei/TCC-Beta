using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserVerify : MonoBehaviour
{
    public Vector2 collisionPos;
    public bool colliderExist;
    public static LaserVerify instance { get; private set; }

    public void Awake()
    {
        instance = this;
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        collisionPos = coll.gameObject.GetComponent<Collider2D>().ClosestPoint(GameManager.instance.hero.transform.position);
        colliderExist = true;
    }
}
