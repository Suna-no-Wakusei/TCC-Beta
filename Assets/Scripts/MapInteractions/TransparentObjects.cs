using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObjects : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.layer == 9 || collision.gameObject.layer == 8)
            this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .35f);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.layer == 9 || collision.gameObject.layer == 8)
            this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
}