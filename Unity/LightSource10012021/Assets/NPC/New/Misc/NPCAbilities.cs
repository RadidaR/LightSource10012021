using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Abilities", menuName = "NPC/Abilities")]
public class NPCAbilities : ScriptableObject
{
    [Header("NPC Type")]
    public string type;

    [Header("Movement")]
    public bool canMove;
    public bool canRun;
    public bool canJump;
    public bool canClimb;
    public bool avoidsLedges;
    public bool canFly;

    [Header("Attack")]
    public bool canAttack;
    public bool hasCombo;
    public bool usesWeapons;

    [Header("Attack Types")]
    public bool standardAttack;
    public bool weaponAttack;
    public bool rangedAttack;
    public bool chargeAttack;
    public bool jumpAttack;
    public bool burrowAttack;
}