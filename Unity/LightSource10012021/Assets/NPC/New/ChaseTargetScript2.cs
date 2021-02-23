using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTargetScript2 : MonoBehaviour
{
    public GameObject npc;

    public NPCData data;
    public NPCAbilities abilities;

    public NPCStatesScript states;

    public SeekTargetScript2 seekTarget;

    public NPCAttackScript attacks;

    public NPCMovementScript movement;

    public NavMeshAgent2D navAgent;

    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] int directionToTarget;


    private void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            npc = GetComponentInParent<OfInterest>().gameObject;
            states = npc.GetComponent<NPCStatesScript>();

            data = states.data;
            abilities = states.abilities;

            seekTarget = npc.GetComponentInChildren<SeekTargetScript2>();
            attacks = npc.GetComponentInChildren<NPCAttackScript>();
            movement = npc.GetComponentInChildren<NPCMovementScript>();
            rigidBody = npc.GetComponent<Rigidbody2D>();

            if (abilities.canFly)
            {
                navAgent = npc.GetComponent<NavMeshAgent2D>();
            }
        }
    }

    private void FixedUpdate()
    {
        //IF THERE IS A TARGET
        if (seekTarget.currentTarget != null)
        {
            //FOR MOBILE UNITS
            if (abilities.canMove)
            {
                //CHASE TARGETS POSITION
                ChaseTarget(seekTarget.currentTarget.transform.position);
            }
        }
        //IF NO TARGET, BUT LAST KNOWN POSITION ISN'T EMPTY
        else if (seekTarget.lastKnownPosition != Vector2.zero)
        {
            //FOR MOBILE UNITS
            if (abilities.canMove)
            {
                //CHASE LAST KNOWN POSITION
                ChaseTarget(seekTarget.lastKnownPosition);
            }
        }    
        //IF NO TARGET AND NO LAST KNOWN POSITION
        else if (seekTarget.currentTarget == null && seekTarget.lastKnownPosition == Vector2.zero)
        {
            //STOP CHASING
            states.isChasing = false;
        }
    }

    void ChaseTarget(Vector2 position)
    {
        //FOR HOSTILE UNITS
        if (abilities.canAttack && !states.isTelegraphing && !states.isAttacking)
        {
            //IF TARGET IS OUT OF CURRENT ATTACK RANGE
            //if (Mathf.Abs(npc.transform.position.x - position.x) > attacks.currentAttackRange || Mathf.Abs(npc.transform.position.y - position.y) > attacks.currentAttackRange)
            if (Vector2.Distance(npc.transform.position, position) > attacks.nextAttackData.range)
            {
                //START CHASING
                states.isChasing = true;

                //FOR GROUND UNITS
                if (!abilities.canFly)
                {
                        if (Mathf.Abs(npc.transform.position.x - position.x) > attacks.nextAttackData.range)
                        {
                            //RUN
                            movement.Move(data.runSpeed, position);
                        }
                        else if (Mathf.Abs(npc.transform.position.y - position.y) > attacks.nextAttackData.range)
                        {
                        if (states.stepAhead)
                        {
                            if (abilities.canClimb || abilities.canJump)
                            {
                                movement.Move(data.runSpeed, position);
                            }
                            else
                            {
                                movement.StopMoving();
                            }
                        }
                        else
                        {
                            movement.StopMoving();
                        }
                    }
                }
                //FOR FLYING UNITS
                else
                {
                    //FLY
                    if (seekTarget.currentTarget != null && seekTarget.currentTarget.gameObject.tag == "Player")
                    {
                        movement.Move(data.flySpeed, new Vector2(position.x, position.y + 4));
                    }
                    else
                    {
                        movement.Move(data.flySpeed, position);
                    }
                }
            }
            //IF TARGET IS WITHIN ATTACK RANGE
            else
            {
                //STOP MOVING
                movement.StopMoving();
                //ATTACKS TO BE ADDED
                attacks.LaunchAttack();
            }
        }

        //TRACK DIRECTION TO TARGET
        if (npc.transform.position.x - position.x < 0)
        {
            directionToTarget = 1;
        }
        if (npc.transform.position.x - position.x > 0)
        {
            directionToTarget = -1;
        }
        //FLIP NPC TO FACE IT
        movement.FlipNPC(directionToTarget);
    }
}
