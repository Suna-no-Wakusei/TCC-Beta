using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D collision)
    {
        this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .35f);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
}