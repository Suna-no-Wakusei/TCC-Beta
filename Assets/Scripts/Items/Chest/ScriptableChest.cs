using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableChest : ScriptableObject
{
    public bool needKey;
    public bool weapon;
    public bool chestAlreadyOpened;
    public Animator animator;
    public List<Item> items;
    public int weaponLevel;
    public enum TypeKey
    {
        GreyKey,
        GoldenKey
    }

    public TypeKey typeKey;
}
