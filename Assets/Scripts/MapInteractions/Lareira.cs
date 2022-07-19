using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lareira : MonoBehaviour, ICollectable
{
    public static bool chimneyActive = false;
    private GameObject go;

    void Awake()
    {
        go = this.transform.GetChild(0).gameObject;

        if (chimneyActive)
        {
            go.SetActive(true);
        }
        else
        {
            go.SetActive(false);
        }
    }

    // Update is called once per frame
    public void Collect()
    {
        if (go.activeSelf)
        {
            go.SetActive(false);
            chimneyActive = false;
        }
        else
        {
            go.SetActive(true);
            chimneyActive = true;
        }
    }

    public static bool getChimneyState()
    {
        return chimneyActive;
    }
}
