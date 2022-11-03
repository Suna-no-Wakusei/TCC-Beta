using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSound : Collidable
{
    protected override void OnCollide(Collider2D coll)
    {
        //teleport the player
        if (coll.name == "Hero")
        {
            GameManager.instance.sfxManager.PlayDoor();
        }
    }
}
