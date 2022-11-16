using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableEnemy : ScriptableObject
{
    public bool alive;
    public bool canMove;
    public enum EnemyType { TreantMini, TreantNormal }
}
