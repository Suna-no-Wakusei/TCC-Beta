using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MizumiHidden : MonoBehaviour
{
    public GameObject mizumi;
    void Update()
    {
        if(GameManager.instance.objectiveManager.objectiveActive != null)
            if (GameManager.instance.objectiveManager.objectiveActive.id != 6)
                mizumi.SetActive(true);
    }
}
