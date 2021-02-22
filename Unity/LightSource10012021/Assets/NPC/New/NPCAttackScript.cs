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

    public ShowCurveScript curve;



    //public int attackOrder;

    public int standardAttack;
    public int weaponAttack;
    public int rangedAttack;
    public int chargeAttack;
    public int jumpAttack;
    public int burrowAttack;


    public List<int> attackPattern;

    public bool attackRunning;

    public int nextAttack;
    public float currentAttackRange;
    public int currentDamage;
    public float telegraphDuration;
    public float attackDuration;

    public Vector2 targetPosition;

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

            curve = GetComponentInChildren<ShowCurveScript>();

            if (abilities.canAttack)
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
            currentAttackRange = data.standardAttackRange;
            currentDamage = data.standardAttackDamage;
            telegraphDuration = data.standardAttackTelegraph;
            attackDuration = data.standardAttackDuration;
        }
        else if (nextAttack == weaponAttack)
        {
            nextAttack = weaponAttack;
            currentAttackRange = data.weaponAttackRange;
            telegraphDuration = data.weaponAttackTelegraph;
            attackDuration = data.weaponAttackDuration;
        }
        else if (nextAttack == rangedAttack)
        {
            nextAttack = rangedAttack;
            currentAttackRange = data.rangedAttackRange;
            currentDamage = data.rangedAttackDamage;
            telegraphDuration = data.rangedAttackTelegraph;
            attackDuration = data.rangedAttackDuration;
        }
        else if (nextAttack == chargeAttack)
        {
            nextAttack = chargeAttack;
            currentAttackRange = data.chargeAttackRange;
            currentDamage = data.chargeAttackDamage;
            telegraphDuration = data.chargeAttackTelegraph;
            attackDuration = data.chargeAttackDuration;
        }
        else if (nextAttack == jumpAttack)
        {
            nextAttack = jumpAttack;
            currentAttackRange = data.jumpAttackRange;
            currentDamage = data.jumpAttackDamage;
            telegraphDuration = data.jumpAttackTelegraph;
            attackDuration = data.jumpAttackDuration;
        }
        else if (nextAttack == burrowAttack)
        {
            nextAttack = burrowAttack;
            currentAttackRange = data.burrowAttackRange;
            telegraphDuration = data.burrowAttackTelegraph;
            attackDuration = data.burrowAttackDuration;
            //currentDamage = data.;
        }
    }

    public void LaunchAttack()
    {
        movement.StopMoving();
        StartCoroutine(Attack(nextAttack, telegraphDuration, currentDamage, attackDuration));
    }

    public void UpdateAttackPattern()
    {
        Debug.Log("Updating Pattern");
        attackPattern.Remove(attackPattern[0]);
        attackPattern.Add(nextAttack);

        if (attackPattern[0] == standardAttack)
        {
            nextAttack = standardAttack;
            currentAttackRange = data.standardAttackRange;
            currentDamage = data.standardAttackDamage;
            telegraphDuration = data.standardAttackTelegraph;
            attackDuration = data.standardAttackDuration;
        }
        else if (attackPattern[0] == weaponAttack)
        {
            nextAttack = weaponAttack;
            currentAttackRange = data.weaponAttackRange;
            telegraphDuration = data.weaponAttackTelegraph;
            attackDuration = data.weaponAttackDuration;
        }
        else if (attackPattern[0] == rangedAttack)
        {
            nextAttack = rangedAttack;
            currentAttackRange = data.rangedAttackRange;
            currentDamage = data.rangedAttackDamage;
            telegraphDuration = data.rangedAttackTelegraph;
            attackDuration = data.rangedAttackDuration;
        }
        else if (attackPattern[0] == chargeAttack)
        {
            nextAttack = chargeAttack;
            currentAttackRange = data.chargeAttackRange;
            currentDamage = data.chargeAttackDamage;
            telegraphDuration = data.chargeAttackTelegraph;
            attackDuration = data.chargeAttackDuration;
        }
        else if (attackPattern[0] == jumpAttack)
        {
            nextAttack = jumpAttack;
            currentAttackRange = data.jumpAttackRange;
            currentDamage = data.jumpAttackDamage;
            telegraphDuration = data.jumpAttackTelegraph;
            attackDuration = data.jumpAttackDuration;
        }
        else if (attackPattern[0] == burrowAttack)
        {
            nextAttack = burrowAttack;
            currentAttackRange = data.burrowAttackRange;
            telegraphDuration = data.burrowAttackTelegraph;
            attackDuration = data.burrowAttackDuration;
            //currentDamage = data.;
        }
    }
    public IEnumerator Attack(int attack, float telegraph, int damage, float duration)
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
                        curve.lineRenderer.enabled = true;
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

                                curve.PositionPoints(curveStart, curveStartControl, curveEndControl, curveEnd);


                                curve.DrawCurve(curveStart, curveStartControl, curveEndControl, curveEnd);

                            telegraph -= Time.fixedDeltaTime;
                            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
                            //Debug.Log("Telegraph Duration: " + telegraph.ToString());
                            if (telegraph <= 0)
                            {
                                jumpStart = curveStart;
                                jumpStartControl = curveStartControl;
                                jumpEndControl = curveEndControl;
                                jumpEnd = curveEndControl;
                                states.isTelegraphing = false;
                                states.isAttacking = true;
                                curve.ResetCurve();
                                break;
                            }

                        }
                    }


                    if (states.isAttacking)
                    {
                        float tParam = 0;

                        while (tParam < 1)
                        {
                            tParam += Time.fixedDeltaTime * 1 / duration;
                            //Debug.Log(jumpStart.ToString() + " " + jumpStartControl.ToString() + " " + jumpEndControl.ToString() + " " + jumpEnd.ToString());

                            targetPosition = curve.CalculateCurve(tParam, jumpStart, jumpStartControl, jumpEndControl, jumpEnd);
                            rigidBody.MovePosition(targetPosition);
                            //npc.transform.position = targetPosition;

                            //objectPosition = showCurve.CalculateCurve(tParam, startPoint, startControl, endControl, endPoint);

                            //transform.position = objectPosition;

                            //gameObject.GetComponent<Rigidbody2D>().MovePosition(objectPosition);
                            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
                            if (tParam >= 1)
                            {
                                states.isAttacking = false;
                                //attackRunning = false;
                                break;
                            }
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
                }
            }
        }

            UpdateAttackPattern();
            attackRunning = false;

    }



    void OnDrawGizmosSelected()
    {
        if (currentAttackRange == 0)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currentAttackRange);
    }


}
