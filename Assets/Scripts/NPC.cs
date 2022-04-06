using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Collectable
{
    public int coins;
    public Sprite AvoidedNPC;

    protected override void OnCollect()
    {
        if (!collected)
        {
            Debug.Log("You received " + coins + "coins!");
            GetComponent<SpriteRenderer>().sprite = AvoidedNPC;
            collected = true;
        }
        
    }
}
