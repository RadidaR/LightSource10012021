using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public NPCType type;

    [Header("Health")]
    [SerializeField] int currentHealth;
    public int healthLost;

    [Header("Body")]
    [SerializeField] Rigidbody2D body;

    [Header("States")]
    [Range(-1, 1)] [SerializeField] public int facingDirection;
    [SerializeField] public bool isIdle;
    [SerializeField] public bool followsLight;
    [SerializeField] public bool chasesTarget;
    [SerializeField] public bool isTelegraphing;
    [SerializeField] public bool isAttacking;
    [SerializeField] public bool isHurt;

    [Header("Ground Movement")]
    [SerializeField] public bool isGrounded;
    [SerializeField] public bool isAirborne;
    [SerializeField] public bool isStill;
    [SerializeField] public bool isWalking;
    [SerializeField] public bool isRunning;
    [SerializeField] public bool isJumping;
    [SerializeField] public bool isClimbing;

    [SerializeField] public bool ledgeAhead;
    [SerializeField] public bool wallAhead;
    [SerializeField] public bool stepAhead;

    void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            body = GetComponentInChildren<Rigidbody2D>();
        }
    }
    void Start()
    {
        currentHealth = type.maxHealth;

        InvokeRepeating("CheckFacingDirection", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!followsLight && !chasesTarget && !isTelegraphing && !isAttacking && !isHurt)
        {
            isIdle = true;
        }
        else
        {
            isIdle = false;
        }
    }

    void CheckFacingDirection()
    {
        if (body.gameObject.transform.localScale.x < 0)
        {
            facingDirection = -1;
        }
        else if (body.gameObject.transform.localScale.x > 0)
        {
            facingDirection = 1;
        }
    }
}
