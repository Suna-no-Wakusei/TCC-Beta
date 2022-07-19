using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : Collidable, ICollectable
{
    public int coins;

    [SerializeField] DialogueText dialog;

    protected override void OnCollide(Collider2D coll)
    {
        GameManager.instance.ShowText("Press E", 15, Color.white, transform.position + new Vector3(3,0,0), Vector3.zero, 0);
    }

    public void Collect()
    {

        GameManager.instance.ShowText("+" + coins + " Coins", 15, Color.yellow, transform.position, Vector3.up * 25, 1.0f);

        if(DialogueManager.Instance.dialogRunning == false)
            StartCoroutine(DialogueManager.Instance.ShowDialog(dialog));

    }

}
