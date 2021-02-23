using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttackScript : MonoBehaviour
{
    public GameObject npc;
    public NPCStatesScript states;
    public NPCMovementScript movement;
    public SeekTargetScript2 seekTarget;
    public Rigidbody2D rigidBody;

    public NPCData data;
    public NPCAbilities abilities;

    public JumpAttackScript jumpAttackData;

    //public ShowCurveScript curve;



    //public int attackOrder;

    public int standardAttack = 0;
    public int weaponAttack = 0;
    public int rangedAttack = 0;
    public int chargeAttack = 0;
    public int jumpAttack = 0;
    public int burrowAttack = 0;


    public List<int> attackPattern;

    public bool attackRunning;

    public int nextAttack;
    public AttackData nextAttackData;

    //public float attackRange;
    //public int attackDamage;
    //public float telegraphDuration;
    //public float attackLength;
    //public float attackCooldown;

    public Vector2 targetPosition;
    public float cooldown;

    private void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            npc = GetComponentInParent<OfInterest>().gameObject;
            states = npc.GetComponent<NPCStatesScript>();
            movement = npc.GetComponentInChildren<NPCMovementScript>();
            seekTarget = npc.GetComponentInChildren<SeekTargetScript2>();
            rigidBody = npc.GetComponent<Rigidbody2D>();

            data = npc.GetComponent<NPCStatesScript>().data;
            abilities = npc.GetComponent<NPCStatesScript>().abilities;

            //curve = GetComponentInChildren<ShowCurveScript>();

            if (!this.enabled)
            {
                standardAttack = 0;
                weaponAttack = 0;
                rangedAttack = 0;
                chargeAttack = 0;
                jumpAttack = 0;
                burrowAttack = 0;
            }

            if (abilities.canAttack && this.enabled)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (abilities.standardAttack && standardAttack == 0)
                    {
                        standardAttack = i;
                    }
                    else if (abilities.weaponAttack && weaponAttack == 0)
                    {
                        weaponAttack = i;
                    }
                    else if (abilities.rangedAttack && rangedAttack == 0)
                    {
                        rangedAttack = i;
                    }
                    else if (abilities.chargeAttack && chargeAttack == 0)
                    {
                        chargeAttack = i;
                    }
                    else if (abilities.jumpAttack && jumpAttack == 0)
                    {
                        jumpAttack = i;
                    }
                    else if (abilities.burrowAttack && burrowAttack == 0)
                    {
                        burrowAttack = i;
                    }
                }
            }

            if (abilities.jumpAttack)
            {
                jumpAttackData = GetComponentInChildren<JumpAttackScript>();
            }
        }
    }


    private void Update()
    {
        if (seekTarget.currentTarget != null)
        {
            targetPosition = seekTarget.currentTarget.transform.position;
        }
        else if (seekTarget.lastKnownPosition != Vector2.zero)
        {
            targetPosition = seekTarget.lastKnownPosition;
        }
        else
        {
            targetPosition = Vector2.zero;
        }
    }

    private void Start()
    {
        nextAttack = attackPattern[0];

        if (nextAttack == standardAttack)
        {
            nextAttack = standardAttack;
            //attackRange = data.standardAttackRange;
            //attackDamage = data.standardAttackDamage;
            //telegraphDuration = data.standardAttackTelegraph;
            //attackLength = data.standardAttackDuration;
        }
        else if (nextAttack == weaponAttack)
        {
            nextAttack = weaponAttack;
            //attackRange = data.weaponAttackRange;
            //telegraphDuration = data.weaponAttackTelegraph;
            //attackLength = data.weaponAttackDuration;
        }
        else if (nextAttack == rangedAttack)
        {
            nextAttack = rangedAttack;
            //attackRange = data.rangedAttackRange;
            //attackDamage = data.rangedAttackDamage;
            //telegraphDuration = data.rangedAttackTelegraph;
            //attackLength = data.rangedAttackDuration;
        }
        else if (nextAttack == chargeAttack)
        {
            nextAttack = chargeAttack;
            //attackRange = data.chargeAttackRange;
            //attackDamage = data.chargeAttackDamage;
            //telegraphDuration = data.chargeAttackTelegraph;
            //attackLength = data.chargeAttackDuration;
        }
        else if (nextAttack == jumpAttack)
        {
            nextAttack = jumpAttack;
            nextAttackData = jumpAttackData.data;
            //attackRange = data.jumpAttackRange;
            //attackDamage = data.jumpAttackDamage;
            //telegraphDuration = data.jumpAttackTelegraph;
            //attackLength = data.jumpAttackDuration;
        }
        else if (nextAttack == burrowAttack)
        {
            nextAttack = burrowAttack;
            //attackRange = data.burrowAttackRange;
            //telegraphDuration = data.burrowAttackTelegraph;
            //attackLength = data.burrowAttackDuration;
            //currentDamage = data.;
        }
    }

    public void LaunchAttack()
    {
        movement.StopMoving();
        StartCoroutine(Attack(nextAttack, nextAttackData.telegraph, nextAttackData.damage, nextAttackData.length));
    }

    public void UpdateAttackPattern()
    {
        Debug.Log("Updating Pattern");
        attackPattern.Remove(attackPattern[0]);
        attackPattern.Add(nextAttack);

        if (attackPattern[0] == standardAttack)
        {
            nextAttack = standardAttack;
            //attackRange = data.standardAttackRange;
            //attackDamage = data.standardAttackDamage;
            //telegraphDuration = data.standardAttackTelegraph;
            //attackLength = data.standardAttackDuration;
        }
        else if (attackPattern[0] == weaponAttack)
        {
            nextAttack = weaponAttack;
            //attackRange = data.weaponAttackRange;
            //telegraphDuration = data.weaponAttackTelegraph;
            //attackLength = data.weaponAttackDuration;
        }
        else if (attackPattern[0] == rangedAttack)
        {
            nextAttack = rangedAttack;
            //attackRange = data.rangedAttackRange;
            //attackDamage = data.rangedAttackDamage;
            //telegraphDuration = data.rangedAttackTelegraph;
            //attackLength = data.rangedAttackDuration;
        }
        else if (attackPattern[0] == chargeAttack)
        {
            nextAttack = chargeAttack;
            //attackRange = data.chargeAttackRange;
            //attackDamage = data.chargeAttackDamage;
            //telegraphDuration = data.chargeAttackTelegraph;
            //attackLength = data.chargeAttackDuration;
        }
        else if (attackPattern[0] == jumpAttack)
        {
            nextAttack = jumpAttack;
            nextAttackData = jumpAttackData.data;
            //attackRange = data.jumpAttackRange;
            //attackDamage = data.jumpAttackDamage;
            //telegraphDuration = data.jumpAttackTelegraph;
            //attackLength = data.jumpAttackDuration;
        }
        else if (attackPattern[0] == burrowAttack)
        {
            nextAttack = burrowAttack;
            //attackRange = data.burrowAttackRange;
            //telegraphDuration = data.burrowAttackTelegraph;
            //attackLength = data.burrowAttackDuration;
            //currentDamage = data.;
        }
    }
    public IEnumerator Attack(int attack, float telegraph, int damage, float length)
    {
        if (!attackRunning)
        {
            attackRunning = true;
            Debug.Log("Attack Started");
            if (!states.isTelegraphing && !states.isAttacking)
            {
                Debug.Log("Telegraph Started");
                states.isTelegraphing = true;
                states.isAttacking = false;
            }

            if (attack == jumpAttack)
            {
                if (targetPosition != Vector2.zero)
                {
                    Vector3 jumpStart = npc.transform.position;
                    Vector3 jumpStartControl = npc.transform.position;
                    Vector3 jumpEndControl = targetPosition;
                    Vector3 jumpEnd = targetPosition;
                    if (states.isTelegraphing)
                    {
                        jumpAttackData.lineRenderer.enabled = true;
                        while (telegraph > 0)
                        {
                            movement.StopMoving();

                                Vector3 curveStart = npc.transform.position;
                                Vector3 curveEnd = new Vector3(targetPosition.x, targetPosition.y + 4);

                                float distanceToTargetX = Mathf.Abs(curveStart.x - curveEnd.x);
                                float distanceToTargetY = Mathf.Abs(curveStart.y - curveEnd.y);

                                Vector3 curveStartControl;
                                Vector3 curveEndControl;

                                if (curveStart.y > curveEnd.y && Mathf.Abs(curveStart.y - curveEnd.y) > 10)
                                {
                                    curveStartControl = new Vector3(curveStart.x + (distanceToTargetX * 0.25f * states.facingDirection), curveStart.y/* + (distanceToTargetX / 5) + (distanceToTargetY / 2)*/);
                                    curveEndControl = new Vector3(curveEnd.x - (distanceToTargetX * 0.25f * states.facingDirection), curveEnd.y + (distanceToTargetX / 5) + (distanceToTargetY / 2));
                                }
                                else if (curveStart.y < curveEnd.y && Mathf.Abs(curveStart.y - curveEnd.y) > 5)
                                {
                                    curveStartControl = new Vector3(curveStart.x + (distanceToTargetX * 0.25f * states.facingDirection), curveStart.y + (distanceToTargetX / 5) + (distanceToTargetY / 4));
                                    curveEndControl = new Vector3(curveEnd.x - (distanceToTargetX * 0.25f * states.facingDirection), curveEnd.y);
                                }
                                else
                                {
                                    curveStartControl = new Vector3(curveStart.x + (distanceToTargetX * 0.25f * states.facingDirection), curveStart.y + (distanceToTargetX / 5) + (distanceToTargetY / 2));
                                    curveEndControl = new Vector3(curveEnd.x - (distanceToTargetX * 0.25f * states.facingDirection), curveEnd.y + (distanceToTargetX / 5) + (distanceToTargetY / 2));
                                }

                                //curve.PositionPoints(curveStart, curveStartControl, curveEndControl, curveEnd);


                                jumpAttackData.DrawCurve(curveStart, curveStartControl, curveEndControl, curveEnd);

                            telegraph -= Time.fixedDeltaTime;
                            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
                            //Debug.Log("Telegraph Duration: " + telegraph.ToString());
                            if (telegraph <= 0)
                            {
                                jumpStart = curveStart;
                                jumpStartControl = curveStartControl;
                                jumpEndControl = curveEndControl;
                                jumpEnd = curveEnd;
                                states.isTelegraphing = false;
                                states.isAttacking = true;
                                jumpAttackData.ResetCurve();
                                break;
                            }

                        }
                    }


                    if (states.isAttacking)
                    {
                        float tParam = 0;
                        int currentFacingDirection = states.facingDirection;

                        while (tParam < 1)
                        {
                            rigidBody.isKinematic = true;
                            tParam += Time.fixedDeltaTime * 1 / length;
                            Vector2 jumpTarget = jumpAttackData.CalculateCurve(tParam, jumpStart, jumpStartControl, jumpEndControl, new Vector2 (jumpEnd.x + 5 * currentFacingDirection, jumpEnd.y - 3));
                            //Debug.Log(tParam.ToString());
                            rigidBody.MovePosition(jumpTarget);
                            //npc.transform.position = jumpTarget;
                            //if (Physics2D.OverlapCircle(new Vector2 (npc.transform.position.x, npc.transform.position.y + 2f), 0.05f, 3))
                            //{
                            //    rigidBody.isKinematic = false;
                            //    break;
                            //}
                            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
                            //rigidBody.isKinematic = false;

                            //tParam += 0.05f;
                            //Vector2 jumpTarget = jumpAttackData.CalculateCurve(tParam, jumpStart, jumpStartControl, jumpEndControl, jumpEnd);
                            //rigidBody.MovePosition(jumpTarget);
                            //yield return new WaitForSecondsRealtime(0.05f);



                            //Debug.Log(jumpStart.ToString() + " " + jumpStartControl.ToString() + " " + jumpEndControl.ToString() + " " + jumpEnd.ToString());


                            //rigidBody.AddForce(targetPosition.normalized * 5 * -states.facingDirection, ForceMode2D.Impulse);



                            //npc.transform.position = targetPosition;

                            //objectPosition = showCurve.CalculateCurve(tParam, startPoint, startControl, endControl, endPoint);

                            //transform.position = objectPosition;

                            //gameObject.GetComponent<Rigidbody2D>().MovePosition(objectPosition);
                            //if (tParam >= 1)
                            //{
                            //    states.isAttacking = false;

                            //    //rigidBody.velocity = new Vector2(20 * states.facingDirection, rigidBody.velocity.y);
                            //    //attackRunning = false;
                            //    break;
                            //}
                        }


                        //tParam = 0f;

                        //Debug.Log("Attack!");
                        //while (duration > 0)
                        //{
                        //    duration -= Time.fixedDeltaTime;
                        //    yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
                        //    Debug.Log("Attack Duration: " + duration.ToString());
                        //    if (duration <= 0)
                        //    {
                        //        Debug.Log("Attack Ended");
                        //        states.isAttacking = false;
                        //        break;
                        //    }
                        //}
                    }
                    //rigidBody.isKinematic = false;
                }
            }
        }
        rigidBody.isKinematic = false;
        cooldown = nextAttackData.cooldown;
        //AttackDone();
        UpdateAttackPattern();
        attackRunning = false;

        states.isAttacking = false;
    }

    //void AttackDone()
    //{
    //    if (cooldown > 0)
    //    {
    //        cooldown -= Time.deltaTime;
    //        rigidBody.AddForce(new Vector2(states.facingDirection * data.runSpeed, rigidBody.velocity.y), ForceMode2D.Impulse);
    //        states.isAttacking = false;
    //        //rigidBody.velocity = new Vector2(states.facingDirection * data.runSpeed * 2, rigidBody.velocity.y);
    //        //AttackDone();
    //    }
    //    else
    //    {
    //        cooldown = 0;
    //        states.isAttacking = false;
    //    }
    //}



    void OnDrawGizmosSelected()
    {
        if (nextAttackData != null)
        {
            if (nextAttackData.range == 0)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, nextAttackData.range);
        }
    }


}
