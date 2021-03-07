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


    public GameObject GetNamedChild(GameObject parentObject, string childName)
    {
        GameObject childObject = parentObject;

        if (parentObject.transform.childCount != 0)
        {
            foreach (Transform childT1 in parentObject.transform)
            {
                if (childT1.name == childName)
                {
                    childObject = childT1.gameObject;
                }
                else if (childT1.childCount != 0)
                {
                    foreach (Transform subChildT2 in childT1.transform)
                    {
                        if (subChildT2.name == childName)
                        {
                            childObject = subChildT2.gameObject;
                        }
                        else if (subChildT2.childCount != 0)
                        {
                            foreach (Transform subChildT3 in subChildT2.transform)
                            {
                                if (subChildT3.name == childName)
                                {
                                    childObject = subChildT3.gameObject;
                                }
                                else if (subChildT3.childCount != 0)
                                {
                                    foreach (Transform subChildT4 in subChildT3.transform)
                                    {
                                        if (subChildT4.name == childName)
                                        {
                                            childObject = subChildT4.gameObject;
                                        }
                                        else if (subChildT4.childCount != 0)
                                        {
                                            foreach (Transform subChildT5 in subChildT4.transform)
                                            {
                                                if (subChildT5.name == childName)
                                                {
                                                    childObject = subChildT5.gameObject;
                                                }
                                                else if (subChildT5.childCount != 0)
                                                {
                                                    foreach (Transform subChildT6 in subChildT5.transform)
                                                    {
                                                        if (subChildT6.name == childName)
                                                        {
                                                            childObject = subChildT6.gameObject;
                                                        }
                                                        else if (subChildT6.childCount != 0)
                                                        {
                                                            foreach (Transform subChildT7 in subChildT6.transform)
                                                            {
                                                                if (subChildT7.name == childName)
                                                                {
                                                                    childObject = subChildT7.gameObject;
                                                                }
                                                                else if (subChildT7.childCount != 0)
                                                                {
                                                                    foreach (Transform subChildT8 in subChildT7.transform)
                                                                    {
                                                                        if (subChildT8.name == childName)
                                                                        {
                                                                            childObject = subChildT8.gameObject;
                                                                        }
                                                                        else if (subChildT8.childCount != 0)
                                                                        {
                                                                            foreach (Transform subChildT9 in subChildT8.transform)
                                                                            {
                                                                                if (subChildT9.name == childName)
                                                                                {
                                                                                    childObject = subChildT9.gameObject;
                                                                                }
                                                                                else if (subChildT9.childCount != 0)
                                                                                {
                                                                                    foreach (Transform subChildT10 in subChildT9.transform)
                                                                                    {
                                                                                        if (subChildT10.name == childName)
                                                                                        {
                                                                                            childObject = subChildT10.gameObject;
                                                                                        }
                                                                                        else if (subChildT10.childCount != 0)
                                                                                        {
                                                                                            foreach (Transform subChildT11 in subChildT10.transform)
                                                                                            {
                                                                                                if (subChildT11.name == childName)
                                                                                                {
                                                                                                    childObject = subChildT11.gameObject;
                                                                                                }
                                                                                                else if (subChildT11.childCount != 0)
                                                                                                {
                                                                                                    foreach (Transform subChildT12 in subChildT11.transform)
                                                                                                    {
                                                                                                        if (subChildT12.name == childName)
                                                                                                        {
                                                                                                            childObject = subChildT12.gameObject;
                                                                                                        }
                                                                                                        else if (subChildT12.childCount != 0)
                                                                                                        {
                                                                                                            foreach (Transform subChildT13 in subChildT12.transform)
                                                                                                            {
                                                                                                                if (subChildT13.name == childName)
                                                                                                                {
                                                                                                                    childObject = subChildT13.gameObject;
                                                                                                                }
                                                                                                                else if (subChildT13.childCount != 0)
                                                                                                                {
                                                                                                                    foreach (Transform subChildT14 in subChildT13.transform)
                                                                                                                    {
                                                                                                                        if (subChildT14.name == childName)
                                                                                                                        {
                                                                                                                            childObject = subChildT14.gameObject;
                                                                                                                        }
                                                                                                                        else if (subChildT14.childCount != 0)
                                                                                                                        {
                                                                                                                            foreach (Transform subChildT15 in subChildT14.transform)
                                                                                                                            {
                                                                                                                                if (subChildT15.name == childName)
                                                                                                                                {
                                                                                                                                    childObject = subChildT15.gameObject;
                                                                                                                                }
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return childObject;
    }
}
