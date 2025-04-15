using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class AttackAttributes : ScriptableObject
{
    [Header("melee")]
    public float StartTimeBtwAttack;
    public float AttackRange = 0.5f;
    public LayerMask EnemyLayers;
    public LayerMask WallLayers;
    public LayerMask DroneLayer;
    public LayerMask ConsoleLayer;


}


