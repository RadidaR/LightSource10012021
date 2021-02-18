using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviourScript : MonoBehaviour
{
    public GameObject npc;

    public NPCData data;
    public NPCAbilities abilities;

    public NPCStatesScript states;

    public NPCMovementScript movement;

    public Rigidbody2D rigidBody;

    public NavMeshAgent2D navAgent;

    //public float stayTime1;
    //public float stayTime2;
    
    public float stayTimer;

    //public float walkTime1;
    //public float walkTime2;

    public float walkTimer;

    public bool idleRunning;


    public int directionToDestination;
    public Vector2 patrolDestination;

    public Transform patrolSpot1;
    public Transform patrolSpot2;
    public Transform patrolSpot3;
    public Transform patrolSpot4;



    private void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            npc = GetComponentInParent<OfInterest>().gameObject;
            states = npc.GetComponent<NPCStatesScript>();

            data = states.data;
            abilities = states.abilities;

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
        //IF IDLE
        if (states.isIdle)
        {
            //START IDLE BEHAVIOUR
            IdleBehaviour();
        }
        //IF NOT
        else
        {
            //RESET IDLE VALUES
            idleRunning = false;
            if (data.idleBehaviour == "Random")
            {
                stayTimer = 0;
                walkTimer = 0;
            }
        }
    }

    public void IdleBehaviour()
    {
        //FOR MOBILE UNITS
        if (abilities.canMove)
        {
            //RANDOM IDLE BEHAVIOUR
            if (data.idleBehaviour == "Random")
            {
                //FOR GROUND UNITS
                if (!abilities.canFly)
                {
                    //IF NOT IDLE ALREADY
                    if (!idleRunning)
                    {
                        //TURN IDLE_RUNNING ON
                        idleRunning = true;
                        //AND ASSIGN RANDOM STAY TIME
                        stayTimer = Random.Range(data.idleStay1, data.idleStay2);
                    }
                    //IF IDLE
                    else
                    {
                        //IF THERE IS REMAINING STAY TIME
                        if (stayTimer > 0)
                        {
                            //TICK AWAY AT IT
                            stayTimer -= Time.fixedDeltaTime;
                            //KEEP STILL
                            movement.StopMoving();
                            //AND GO BACK TO TOP
                            return;
                        }
                        //IF STAY TIME HAS RUN OUT
                        else
                        {
                            //MAKE STAY TIMER 0
                            stayTimer = 0;
                            //CHECK IF WALK TIMER IS 0
                            if (walkTimer == 0)
                            {
                                //ASSIGN RANDOM DIRECTION
                                int direction = Random.Range(-1, 2);
                                //IF DIRECTION IS -1 OR 1
                                if (direction != 0)
                                {
                                    if (!states.isClimbing)
                                    {
                                        //FLIP NPC IN THAT DIRECTION
                                        movement.FlipNPC(direction);
                                    }
                                }
                                //IF DIRECTION IS 0
                                else
                                {
                                    //RESET STAY TIMER
                                    stayTimer = Random.Range(data.idleStay1, data.idleStay2);
                                    //AND GO BACK TO TOP
                                    return;
                                }

                                //AND ASSIGN RANDOM WALK TIME
                                walkTimer = Random.Range(data.idleMove1, data.idleMove2);
                            }
                        }
                        //IF THERE IS REMAINING WALK TIME
                        if (walkTimer > 0)
                        {
                            //TICK AWAY AT IT
                            walkTimer -= Time.fixedDeltaTime;
                            //AND MOVE
                            movement.Move(data.moveSpeed, Vector2.zero);
                        }
                        //IF WALK TIME HAS RUN OUT
                        else
                        {
                            //MAKE WALK TIMER 0
                            walkTimer = 0;
                            //AND TURN IDLE_RUNNING OFF
                            idleRunning = false;
                        }
                    }
                }
            }
            if (data.idleBehaviour == "Patrol")
            {
                if (!abilities.canFly)
                {
                    float distanceToSpot1 = Vector2.Distance(npc.transform.position, patrolSpot1.position);
                    float distanceToSpot2 = Vector2.Distance(npc.transform.position, patrolSpot2.position);

                    if (!idleRunning)
                    {
                        idleRunning = true;

                        stayTimer = Random.Range(data.idleStay1, data.idleStay2);

                        if (distanceToSpot1 < distanceToSpot2)
                        {
                            patrolDestination = patrolSpot1.localPosition;
                        }
                        else
                        {
                            patrolDestination = patrolSpot2.localPosition;
                        }

                        if (npc.transform.localPosition.x - patrolDestination.x < 0)
                        {
                            directionToDestination = 1;
                        }
                        if (npc.transform.localPosition.x - patrolDestination.x > 0)
                        {
                            directionToDestination = -1;
                        }
                        //FLIP NPC TO FACE IT
                        movement.FlipNPC(directionToDestination);
                    }
                    else
                    {
                        if (stayTimer > 0)
                        {
                            stayTimer -= Time.fixedDeltaTime;
                            movement.StopMoving();
                            return;
                        }
                        //else
                        //{
                        //    stayTimer = 0;
                        //}

                        if (Mathf.Abs(npc.transform.localPosition.x - patrolDestination.x) > data.visionRange)
                        {
                            movement.Move(data.moveSpeed, patrolDestination);
                        }
                        else
                        {
                            if (stayTimer == 0)
                            {
                                stayTimer = Random.Range(data.idleStay1, data.idleStay2);
                            }
                            else if (stayTimer > 0)
                            {
                                stayTimer -= Time.fixedDeltaTime;
                                movement.StopMoving();
                            }
                            else if (stayTimer < 0)
                            {
                                if (distanceToSpot1 < distanceToSpot2)
                                {
                                    patrolDestination = patrolSpot2.localPosition;
                                }
                                else
                                {
                                    patrolDestination = patrolSpot1.localPosition;
                                }

                                if (npc.transform.localPosition.x - patrolDestination.x < 0)
                                {
                                    directionToDestination = 1;
                                }
                                if (npc.transform.localPosition.x - patrolDestination.x > 0)
                                {
                                    directionToDestination = -1;
                                }

                                movement.FlipNPC(directionToDestination);
                                stayTimer = 0;
                            }                            
                        }
                    }
                }
            }
        }
    }
}
