using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Data", menuName = "NPC/Type")]
public class NPCType : ScriptableObject
{
    [Header("Type")]
    public string npcName;
    public bool eyeSpawn;
    public bool guard;
    public bool bandit;
    public bool civilian;

    [Header("Health")]
    [Range(0, 500)] public int maxHealth;

    [Header("Movement")]
    public bool groundUnit;
    public bool flyingUnit;
    public bool isMobile;

    [Header("Ground Movement")]
    [Range(0, 100)] public float walkSpeed;
    public bool canRun;
    [Range(0, 200)] public float runSpeed;
    public bool canJump;
    [Range(0, 100)] public float jumpForce;
    [Range(0, 3)] public float jumpDelay;
    public bool canLeap;
    [Range(0, 50)] public float leapDistance;
    public bool canClimb;
    [Range(0, 100)] public float climbSpeed;
    public bool avoidsLedges;

    [Header("Flying Movement")]
    [Range(0, 100)] public float flySpeed;
    [Range(0, 100)] public float flyAcceleration;

    [Header("Vision")]
    [Range(0, 100)] public float idleVision;
    [Range(0, 200)] public float focusedVision;

    [Header("Idles Behaviour")]
    public string idleBehaviour;
    [Range(0, 30)] public float idleStayMin;
    [Range(30, 90)] public float idleStayMax;
    [Range(0, 30)] public float idleMoveMin;
    [Range(30, 90)] public float idleMoveMax;

    [Range(0, 500)] public float idleBoundaryPosX;
    [Range(-500, 0)] public float idleBoundaryNegX;
    [Range(0, 500)] public float idleBoundaryPosY;
    [Range(-500, 0)] public float idleBoundaryNegY;

    [Header("Chasing Behaviour")]
    [Range(0, 30)] public float abandonChaseAfter;

    [Header("Collision")]
    [Range(0, 100)] public int collisionDamage;

    [Header("Offense")]
    public bool canFight;
    public bool meleeUnit;
    public bool rangedUnit;
    public bool usesWeapons;
    public bool hasCombo;

    [Header("Attack Types")]
    public bool hasMeleeAttack;
    public AttackData meleeAttack;
    public bool hasRangedAttack;
    public AttackData rangedAttack;
    public bool hasChargeAttack;
    public AttackData chargeAttack;
    public bool hasLeapAttack;
    public AttackData leapAttack;
    public bool hasDiveAttack;
    public AttackData diveAttack;
    public bool hasBurrowAttack;
    public AttackData burrowAttack;

    public List<int> attackPattern;
}
