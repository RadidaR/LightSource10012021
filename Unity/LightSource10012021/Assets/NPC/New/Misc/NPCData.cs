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

    [Header("Idle Behaviour")]
    public string idleBehaviour;
    public float idleStay1;
    public float idleStay2;
    public float idleMove1;
    public float idleMove2;

    [Range(0, 99999)] public float idleBoundaryPosX;
    [Range(-99999, 0)] public float idleBoundaryNegX;
    [Range(0, 99999)] public float idleBoundaryPosY;
    [Range(-99999, 0)] public float idleBoundaryNegY;

    [Header("Chase Behaviour")]
    public float stopChaseAfter;

    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;
    public float flySpeed;
    public float flyAcceleration;
    public float climbSpeed;
    public float jumpForce;
    public float jumpDelay;

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
