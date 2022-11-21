using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFloorType : MonoBehaviour
{
    public GameManager.FloorType floorSound;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.floorType = floorSound;
            GameManager.instance.hero.stepingOnDefault = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.floorType = floorSound;
            GameManager.instance.hero.stepingOnDefault = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.floorType = GameManager.instance.levelType;
            GameManager.instance.hero.stepingOnDefault = true;
        }
    }

}
