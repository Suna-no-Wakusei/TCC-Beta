using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour, Collectable
{
    public int coins;

    [SerializeField] DialogueText dialog;

    public void Collect()
    {

        GameManager.instance.ShowText("+" + coins + " Coins", 8, Color.yellow, transform.position, Vector3.up * 25, 1.0f);

        if(DialogueManager.Instance.dialogRunning == false)
            StartCoroutine(DialogueManager.Instance.ShowDialog(dialog));

    }

}
