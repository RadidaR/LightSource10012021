using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Data", menuName = "NPC/Data")]
public class NPCData : ScriptableObject
{
    [Header("NPC Type")]
    public string name;
    public bool zombie;
    public bool human;

    [Header("Health")]
    public int maxHealth;
    public float hurtDuration;

    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;
    public float flySpeed;
    public float climbSpeed;
    public float jumpForce;

    [Header("Collision")]
    public int collisionDamage;

    [Header("Standard Attack")]
    public float standardAttackRange;
    public float standardAttackSpeed;
    public float standardAttackTelegraph;
    public float standardAttackDuration;
    public float standardFullAttackDuration;
    public int standardAttackDamage;

    [Header("Weapon Attack")]
    public float weaponAttackRange;
    public float weaponAttackSpeed;
    public float weaponAttackTelegraph;
    public float weaponAttackDuration;
    public float weapontFullAttackDuration;

    [Header("Ranged Attack")]
    public float rangedAttackRange;
    public float rangedAttackSpeed;
    public float rangedAttackTelegraph;
    public float rangedAttackDuration;
    public float rangedFullAttackDuration;
    public int rangedAttackDamage;

    [Header("Charge Attack")]
    public float chargeAttackRange;
    public float chargeAttackSpeed;
    public float chargeAttackTelegraph;
    public float chargeAttackDuration;
    public float chargeFullAttackDuration;
    public int chargeAttackDamage;

    [Header("Jump Attack")]
    public float jumpAttackRange;
    public float jumpAttackSpeed;
    public float jumpAttackTelegraph;
    public float jumpAttackDuration;
    public float jumpFullAttackDuration;
    public int jumpAttackDamage;

    [Header("Burrow Attack")]
    public float burrowAttackRange;
    public float burrowAttackSpeed;
    public float burrowAttackTelegraph;
    public float burrowAttackDuration;
    public float burrowFullAttackDuration;

    [Header("Vision")]
    public float visionRange;
    public float visionExpansion;
}
