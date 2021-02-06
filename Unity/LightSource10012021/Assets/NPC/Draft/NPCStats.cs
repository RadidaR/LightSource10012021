using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Stats", menuName = "NPC/Stats")]
public class NPCStats : ScriptableObject
{
    public int maxHealth;
    public int attackDamage;
}
