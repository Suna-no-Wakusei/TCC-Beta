using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveAssets : MonoBehaviour
{
    public static ObjectiveAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Transform pfQuestNot;
}
