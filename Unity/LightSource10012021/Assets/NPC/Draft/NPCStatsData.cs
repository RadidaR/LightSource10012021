using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Stats Data", menuName = "NPC/Stats")]
public class NPCStatsData : ScriptableObject
{
    public int maxHealth;
    public float hurtDuration;

    public bool canFly;
    public float movementSpeed;

    public int collisionDamage;

    public float attackRange;
    public int attackDamage;

    public float visionRange;
    public float visionExpansion;
}
