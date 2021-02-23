using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Data", menuName = "NPC/Attack")]
public class AttackData : ScriptableObject
{
    public string attackName;
    public float range;
    public float telegraph;
    public float length;
    public float cooldown;
    public int damage;
    public float maxDistance;
}
