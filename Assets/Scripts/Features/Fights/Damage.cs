using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    public Vector3 origin;
    public int damageAmount;
    public enum DmgType
    {
        physicalDamage,
        magicDamage
    }

    public DmgType dmgType;
}
