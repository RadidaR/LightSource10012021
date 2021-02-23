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

    public LayerMask groundLayer;

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
    public string targetType;
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

            if (seekTarget.currentTarget.gameObject.tag == "Player")
            {
                targetType = "Player";
            }
            else if (seekTarget.currentTarget.gameObject.tag == "Light")
            {
                targetType = "Light";
            }
            else
            {
                targetType = "Other";
            }
        }
        else if (seekTarget.lastKnownPosition != Vector2.zero)
        {
            targetPosition = seekTarget.lastKnownPosition;

            targetType = "Last Known Position";
        }
        else
        {
            targetPosition = Vector2.zero;
        }

        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            cooldown = 0;
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

    public void LaunchAttack(GameObject target)
    {
        if (cooldown == 0)
        {
            StartCoroutine(Attack(target, nextAttack, nextAttackData.telegraph, nextAttackData.damage, nextAttackData.length));
        }
        //movement.StopMoving();
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
    public IEnumerator Attack(GameObject target, int attack, float telegraph, int damage, float length)
    {
        if (!attackRunning)
        {
            attackRunning = true;
            movement.StopMoving();

            if (!states.isTelegraphing && !states.isAttacking)
            {
                states.isTelegraphing = true;
                states.isAttacking = false;
            }
            if (attack == jumpAttack)
            {
                //if (targetPosition != Vector2.zero)
                //{
                    Vector3 jumpStart = npc.transform.position;
                    Vector3 jumpStartControl = npc.transform.position;
                    Vector3 jumpEndControl = target.transform.position;
                    Vector3 jumpEnd = target.transform.position;


                    RaycastHit2D obstaclesToTarget = Physics2D.Raycast(jumpStart, jumpEnd - jumpStart, Vector2.Distance(jumpStart, jumpEnd), groundLayer);
                    if (states.isTelegraphing)
                    {
                        while (telegraph > 0)
                        {
                            movement.StopMoving();

                            Vector3 curveStart = npc.transform.position;
                            Vector3 curveEnd;

                            if (targetType == "Player")
                            {
                                curveEnd = new Vector3(target.transform.position.x, target.transform.position.y + 4);
                            }
                            else
                            {
                                curveEnd = target.transform.position;
                            }

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


                            if (obstaclesToTarget)
                            {
                                //movement.Move(data.runSpeed, targetPosition);
                                states.isTelegraphing = false;
                                jumpAttackData.ResetCurve();
                                break;
                            }
                            else
                            {
                                movement.StopMoving();
                                jumpAttackData.lineRenderer.enabled = true;

                                if (Vector2.Distance(curveStart, curveEnd) < /*nextAttackData.maxDistance */jumpAttackData.data.maxDistance)
                                {
                                    jumpAttackData.DrawCurve(curveStart, curveStartControl, curveEndControl, curveEnd);
                                }
                            }



                            //curve.PositionPoints(curveStart, curveStartControl, curveEndControl, curveEnd);
                            //RaycastHit2D firstBit = Physics2D.Raycast(new Vector2(jumpStart.x, jumpStart.y + 2), jumpStartControl - jumpStart, Vector2.Distance(jumpStart, jumpStartControl), groundLayer);
                            //RaycastHit2D secondBit = Physics2D.Raycast(jumpStartControl, jumpEndControl - jumpStartControl, Vector2.Distance(jumpStartControl, jumpEndControl), groundLayer);



                            telegraph -= Time.fixedDeltaTime;
                            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);


                            //Debug.Log("Telegraph Duration: " + telegraph.ToString());
                            if (telegraph <= 0)
                            {
                                if (Vector2.Distance(curveStart, curveEnd) < jumpAttackData.data.maxDistance)
                                {
                                    jumpStart = curveStart;
                                    jumpStartControl = curveStartControl;
                                    jumpEndControl = curveEndControl;
                                    jumpEnd = curveEnd;
                                    states.isAttacking = true;
                                }
                                states.isTelegraphing = false;
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
                            tParam += Time.fixedDeltaTime * 1 / length;
                            //Vector2 jumpTarget = jumpAttackData.CalculateCurve(tParam, jumpStart, jumpStartControl, jumpEndControl, new Vector2 (jumpEnd.x + 5 * currentFacingDirection, jumpEnd.y - 3));
                            Vector2 jumpTarget;
                            if (targetType == "Player")
                            {
                                jumpTarget = jumpAttackData.CalculateCurve(tParam, jumpStart, jumpStartControl, jumpEndControl, new Vector2(jumpEnd.x + 5 * currentFacingDirection, jumpEnd.y - 3));
                            }
                            else
                            {
                                jumpTarget = jumpAttackData.CalculateCurve(tParam, jumpStart, jumpStartControl, jumpEndControl, jumpEnd);
                            }
                            //Debug.Log(tParam.ToString());

                            //RaycastHit2D directSight = Physics2D.Raycast(jumpStart, jumpEnd - jumpStart, Vector2.Distance(jumpStart,jumpEnd), groundLayer);

                            RaycastHit2D firstBit = Physics2D.Raycast(new Vector2(jumpStart.x, jumpStart.y + 2), jumpStartControl - jumpStart, Vector2.Distance(jumpStart, jumpStartControl), groundLayer);
                            RaycastHit2D secondBit = Physics2D.Raycast(jumpStartControl, jumpEndControl - jumpStartControl, Vector2.Distance(jumpStartControl, jumpEndControl), groundLayer);
                            if ((!firstBit && !secondBit) || !obstaclesToTarget)
                            {
                                rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                                rigidBody.isKinematic = true;
                                rigidBody.MovePosition(jumpTarget);
                            }
                            else
                            {
                                //rigidBody.isKinematic = false;
                                states.isAttacking = false;
                                break;
                                //movement.Move(data.runSpeed, jumpEnd);
                                //rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                            }
                            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);

                        }





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
                //RaycastHit2D obstaclesToTarget = Physics2D.Raycast(jumpStart, jumpEnd - jumpStart, Vector2.Distance(jumpStart, jumpEnd), groundLayer);

                if (obstaclesToTarget && !states.isTelegraphing && !states.isAttacking)
                {
                    float chaseTarget = 1.5f;
                    while (chaseTarget > 0)
                    {
                        chaseTarget -= Time.fixedDeltaTime;
                        movement.Move(data.runSpeed, target.transform.position);
                        yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
                        if (chaseTarget <= 0)
                        {
                            break;
                        }
                        else if (!obstaclesToTarget)
                        {
                            attackRunning = false;
                            rigidBody.isKinematic = false;
                            StopCoroutine("Attack");
                            break;
                        }
                    }
                }



                //if (attack == jumpAttack)
                //{
                //    if (targetPosition != Vector2.zero)
                //    {
                //        Vector3 jumpStart = npc.transform.position;
                //        Vector3 jumpStartControl = npc.transform.position;
                //        Vector3 jumpEndControl = targetPosition;
                //        Vector3 jumpEnd = targetPosition;


                //        RaycastHit2D obstaclesToTarget = Physics2D.Raycast(jumpStart, jumpEnd - jumpStart, Vector2.Distance(jumpStart, jumpEnd), groundLayer);
                //        if (states.isTelegraphing)
                //        {
                //            while (telegraph > 0)
                //            {                         

                //                Vector3 curveStart = npc.transform.position;
                //                Vector3 curveEnd;

                //                if (targetType == "Player")
                //                {
                //                    curveEnd = new Vector3(targetPosition.x, targetPosition.y + 4);
                //                }
                //                else
                //                {
                //                    curveEnd = new Vector3(targetPosition.x, targetPosition.y);
                //                }

                //                float distanceToTargetX = Mathf.Abs(curveStart.x - curveEnd.x);
                //                float distanceToTargetY = Mathf.Abs(curveStart.y - curveEnd.y);

                //                Vector3 curveStartControl;
                //                Vector3 curveEndControl;

                //                if (curveStart.y > curveEnd.y && Mathf.Abs(curveStart.y - curveEnd.y) > 10)
                //                {
                //                    curveStartControl = new Vector3(curveStart.x + (distanceToTargetX * 0.25f * states.facingDirection), curveStart.y/* + (distanceToTargetX / 5) + (distanceToTargetY / 2)*/);
                //                    curveEndControl = new Vector3(curveEnd.x - (distanceToTargetX * 0.25f * states.facingDirection), curveEnd.y + (distanceToTargetX / 5) + (distanceToTargetY / 2));
                //                }
                //                else if (curveStart.y < curveEnd.y && Mathf.Abs(curveStart.y - curveEnd.y) > 5)
                //                {
                //                    curveStartControl = new Vector3(curveStart.x + (distanceToTargetX * 0.25f * states.facingDirection), curveStart.y + (distanceToTargetX / 5) + (distanceToTargetY / 4));
                //                    curveEndControl = new Vector3(curveEnd.x - (distanceToTargetX * 0.25f * states.facingDirection), curveEnd.y);
                //                }
                //                else
                //                {
                //                    curveStartControl = new Vector3(curveStart.x + (distanceToTargetX * 0.25f * states.facingDirection), curveStart.y + (distanceToTargetX / 5) + (distanceToTargetY / 2));
                //                    curveEndControl = new Vector3(curveEnd.x - (distanceToTargetX * 0.25f * states.facingDirection), curveEnd.y + (distanceToTargetX / 5) + (distanceToTargetY / 2));
                //                }


                //                if (obstaclesToTarget)
                //                {
                //                    //movement.Move(data.runSpeed, targetPosition);
                //                    states.isTelegraphing = false;
                //                    jumpAttackData.ResetCurve();
                //                    break;
                //                }
                //                else
                //                {
                //                    movement.StopMoving();
                //                    jumpAttackData.lineRenderer.enabled = true;

                //                    if (Vector2.Distance(curveStart, curveEnd) < /*nextAttackData.maxDistance */jumpAttackData.data.maxDistance)
                //                    {
                //                        jumpAttackData.DrawCurve(curveStart, curveStartControl, curveEndControl, curveEnd);
                //                    }
                //                }



                //                //curve.PositionPoints(curveStart, curveStartControl, curveEndControl, curveEnd);
                //                //RaycastHit2D firstBit = Physics2D.Raycast(new Vector2(jumpStart.x, jumpStart.y + 2), jumpStartControl - jumpStart, Vector2.Distance(jumpStart, jumpStartControl), groundLayer);
                //                //RaycastHit2D secondBit = Physics2D.Raycast(jumpStartControl, jumpEndControl - jumpStartControl, Vector2.Distance(jumpStartControl, jumpEndControl), groundLayer);



                //                telegraph -= Time.fixedDeltaTime;
                //                yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);


                //                //Debug.Log("Telegraph Duration: " + telegraph.ToString());
                //                if (telegraph <= 0)
                //                {
                //                    if (Vector2.Distance(curveStart, curveEnd) < jumpAttackData.data.maxDistance)
                //                    {
                //                        jumpStart = curveStart;
                //                        jumpStartControl = curveStartControl;
                //                        jumpEndControl = curveEndControl;
                //                        jumpEnd = curveEnd;
                //                        states.isAttacking = true;
                //                    }
                //                    states.isTelegraphing = false;
                //                    jumpAttackData.ResetCurve();
                //                    break;
                //                }
                //            }
                //        }


                //        if (states.isAttacking)
                //        {
                //            float tParam = 0;
                //            int currentFacingDirection = states.facingDirection;

                //            while (tParam < 1)
                //            {
                //                tParam += Time.fixedDeltaTime * 1 / length;
                //                //Vector2 jumpTarget = jumpAttackData.CalculateCurve(tParam, jumpStart, jumpStartControl, jumpEndControl, new Vector2 (jumpEnd.x + 5 * currentFacingDirection, jumpEnd.y - 3));
                //                Vector2 jumpTarget;
                //                if (targetType == "Player")
                //                {
                //                    jumpTarget = jumpAttackData.CalculateCurve(tParam, jumpStart, jumpStartControl, jumpEndControl, new Vector2(jumpEnd.x + 5 * currentFacingDirection, jumpEnd.y - 3));
                //                }
                //                else
                //                {
                //                    jumpTarget = jumpAttackData.CalculateCurve(tParam, jumpStart, jumpStartControl, jumpEndControl, jumpEnd);
                //                }
                //                //Debug.Log(tParam.ToString());

                //                //RaycastHit2D directSight = Physics2D.Raycast(jumpStart, jumpEnd - jumpStart, Vector2.Distance(jumpStart,jumpEnd), groundLayer);

                //                RaycastHit2D firstBit = Physics2D.Raycast(new Vector2(jumpStart.x, jumpStart.y + 2), jumpStartControl - jumpStart, Vector2.Distance(jumpStart, jumpStartControl), groundLayer);
                //                RaycastHit2D secondBit = Physics2D.Raycast(jumpStartControl, jumpEndControl - jumpStartControl, Vector2.Distance(jumpStartControl, jumpEndControl), groundLayer);
                //                if (!firstBit && !secondBit)
                //                {
                //                    rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                //                    rigidBody.isKinematic = true;
                //                    rigidBody.MovePosition(jumpTarget);
                //                }
                //                else
                //                {
                //                    //rigidBody.isKinematic = false;
                //                    states.isAttacking = false;
                //                    break;
                //                    //movement.Move(data.runSpeed, jumpEnd);
                //                    //rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                //                }
                //                yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);

                //            }





                //            //Debug.Log("Attack!");
                //            //while (duration > 0)
                //            //{
                //            //    duration -= Time.fixedDeltaTime;
                //            //    yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
                //            //    Debug.Log("Attack Duration: " + duration.ToString());
                //            //    if (duration <= 0)
                //            //    {
                //            //        Debug.Log("Attack Ended");
                //            //        states.isAttacking = false;
                //            //        break;
                //            //    }
                //            //}
                //        }
                //        //RaycastHit2D obstaclesToTarget = Physics2D.Raycast(jumpStart, jumpEnd - jumpStart, Vector2.Distance(jumpStart, jumpEnd), groundLayer);

                //        if (obstaclesToTarget && !states.isTelegraphing && !states.isAttacking)
                //        {                        
                //            float chaseTarget = 2.5f;
                //            while (chaseTarget > 0)
                //            {
                //                chaseTarget -= Time.fixedDeltaTime;
                //                movement.Move(data.runSpeed, targetPosition);
                //                yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
                //                if (chaseTarget <= 0)
                //                {
                //                    break;
                //                }
                //            }
                //        }

                //    }
                //}

            }
            states.isAttacking = false;
            rigidBody.isKinematic = false;
            cooldown = nextAttackData.cooldown;
            UpdateAttackPattern();
            attackRunning = false;
        }
    }

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

