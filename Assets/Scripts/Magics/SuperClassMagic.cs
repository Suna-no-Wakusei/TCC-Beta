using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperClassMagic : MonoBehaviour
{
    public Fireball fireball;

    void Start()
    {
        
    }

    void Update()
    {
        if(GameManager.instance.selectedMagic == 1)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && !fireball.fireballRunning)
            {
                fireball.PlayFireball();
            }
        }
    }
}
