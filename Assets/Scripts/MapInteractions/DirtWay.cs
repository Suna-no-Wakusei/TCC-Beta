using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtWay : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.floorType = GameManager.FloorType.Earth;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.floorType = GameManager.instance.levelType;
        }
    }

}
