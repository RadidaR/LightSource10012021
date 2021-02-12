using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Stats Data", menuName = "NPC/Stats")]
public class NPCStatsData : ScriptableObject
{
    public int maxHealth;
    public float hurtDuration;

    public int collisionDamage;
    public int attackDamage;
    public float movementSpeed;

    public float visionRange;
    public float visionExpansion;
}
