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

    
    public float stayTimer;


    public float moveTimer;

    public bool idleRunning;


    public int directionToDestination;
    public Vector2 patrolDestination;


    public List<Transform> patrolSpots;
    public GameObject npcObject;

    public int lastVisited;
    public int goingTo;

    public LayerMask groundLayer;

    //public ShowCurveScript curve;


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

            npcObject = npc.transform.parent.gameObject;

            
        }
    }

    private void Start()
    {
        //curve = npc.GetComponentInChildren<ShowCurveScript>();
    }

    public void Awake()
    {
        Transform[] allTransforms = npcObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < allTransforms.Length; i++)
        {
            //Debug.Log(i.ToString());
            if (allTransforms[i].gameObject.tag == "Spot")
            {
                patrolSpots.Add(allTransforms[i]);
            }
        }     
    }
    private void Update()
    {
        if (data.idleBehaviour == "Patrol")
        {
            for (int i = 0; i < patrolSpots.Count; i++)
            {
                if (Physics2D.OverlapCircle(patrolSpots[i].position, 0.1f, groundLayer))
                {
                    patrolSpots[i].position = new Vector2(patrolSpots[i].position.x, patrolSpots[i].position.y + Time.deltaTime * 10);
                }
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
            stayTimer = 0;
            moveTimer = 0;
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
                            if (moveTimer == 0)
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
                                moveTimer = Random.Range(data.idleMove1, data.idleMove2);
                            }
                        }
                        //IF THERE IS REMAINING WALK TIME
                        if (moveTimer > 0)
                        {
                            //TICK AWAY AT IT
                            moveTimer -= Time.fixedDeltaTime;
                            //AND MOVE
                            movement.Move(data.moveSpeed, Vector2.zero);
                        }
                        //IF WALK TIME HAS RUN OUT
                        else
                        {
                            //MAKE WALK TIMER 0
                            moveTimer = 0;
                            //AND TURN IDLE_RUNNING OFF
                            idleRunning = false;
                        }
                    }
                }
                //FOR FLYING UNITS
                else
                {
                    //IF NOT IDLE ALREADY
                    if (!idleRunning)
                    {
                        //TURN IDLE ON
                        idleRunning = true;
                        //ASSIGN RANDOM STAY TIMER
                        stayTimer = Random.Range(data.idleStay1, data.idleStay2);
                    }
                    //IF ALREADY IDLE
                    else
                    {
                        //IF STAY TIMER HASN'T COUNTED DOWN
                        if (stayTimer > 0)
                        {
                            //TICK AWAY AT IT
                            stayTimer -= Time.fixedDeltaTime;
                            //STOP MOTION
                            movement.StopMoving();
                            //AND GO BACK TO TOP
                            return;
                        }
                        //IF STAY TIMER HAS RUN OUT
                        else
                        {
                            //RESET STAY TIMER TO 0
                            stayTimer = 0;
                            //CHECK MOVE TIMER
                            if (moveTimer == 0)
                            {
                                //ASSIGN RANDOM POSITIONS TO ALL PATROL SPOTS
                                for (int i = 0; i < patrolSpots.Count; i++)
                                {
                                    Vector2 randomPosition = new Vector2(Random.Range(data.idleBoundaryNegX, data.idleBoundaryPosX), Random.Range(data.idleBoundaryNegY, data.idleBoundaryPosY));
                                    patrolSpots[i].localPosition = randomPosition;
                                }
                                //ASSIGN RANDOM MOVE TIMER
                                moveTimer = Random.Range(data.idleMove1, data.idleMove2);

                                //SET RANDOM PATROL SPOT AS PATROL DESTINATION
                                int randomNumber = Random.Range(0, patrolSpots.Count);
                                patrolDestination = patrolSpots[randomNumber].position;
                            }
                            //IF MOVE TIMER HASN'T COUNTED DOWN
                            else if (moveTimer > 0)
                            {
                                //TICK AWAY AT IT
                                moveTimer -= Time.fixedDeltaTime;
                                //AND CHECK IF THE PATROL DESTINATION IS OUT OF VISION RANGE
                                if (Vector2.Distance(npc.transform.position, patrolDestination) > data.visionRange)
                                {
                                    //CHECK DIRECTION TO DESTINATION
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
                                    //AND MOVE TOWARDS IT
                                    movement.Move(data.flySpeed, patrolDestination);
                                }
                                //IF IT IS WITHIN VISION RANGE
                                else
                                {
                                    //RESET MOVE TIMER
                                    moveTimer = 0;
                                    //STOP MOVING
                                    movement.StopMoving();
                                    //AND RESET IDLE
                                    idleRunning = false;
                                }
                            }
                            //IF MOVE TIMER HAS COUNTED DOWN
                            else
                            {
                                //RESET IT
                                moveTimer = 0;
                                //AND RESET IDLE
                                idleRunning = false;
                            }
                        }
                    }
                }
            }
            //PATROLLING IDLE BEHAVIOUR
            else if (data.idleBehaviour == "Patrol")
            {
                //ARRAY TO HOLD DISTANCES TO PATROL SPOTS
                float[] distances = new float[patrolSpots.Count];

                //GATHER ALL DISTANCES
                for (int i = 0; i < patrolSpots.Count; i++)
                {
                    distances[i] = Vector2.Distance(npc.transform.position, patrolSpots[i].position);
                }

                //FOR GROUND UNITS
                if (!abilities.canFly)
                {
                    //IF NOT IDLE ALREADY
                    if (!idleRunning)
                    {
                        //TURN IDLE ON
                        idleRunning = true;

                        //ASSIGN POSSIBLY RANDOM STAY TIMER
                        stayTimer = Random.Range(data.idleStay1, data.idleStay2);

                        //CHECK WHICH PATROL POINT IS CLOSER
                        if (distances[0] < distances[1])
                        {
                            //ASSIGN IT AS A PATROL DESTINATION
                            patrolDestination = patrolSpots[0].localPosition;
                        }
                        else
                        {
                            patrolDestination = patrolSpots[1].localPosition;
                        }

                        //CHECK DIRECTION TO PATROL DESTINATION
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
                    //IF ALREADY IDLE
                    else
                    {
                        //IF STAY TIMER HASN'T COUNTED DOWN
                        if (stayTimer > 0)
                        {
                            //TICK AWAY AT IT
                            stayTimer -= Time.fixedDeltaTime;
                            //STOP MOVING
                            movement.StopMoving();
                            //AND GO BACK TO TOP
                            return;
                        }

                        //IF PATROL DESTINATION IS OUT OF SIGHT ON X AXIS
                        if (Mathf.Abs(npc.transform.localPosition.x - patrolDestination.x) > data.visionRange / 3)
                        {
                            //MOVE TOWARDS IT
                            movement.Move(data.moveSpeed, patrolDestination);
                            stayTimer = 0;
                        }
                        //IF IN SIGHT
                        else
                        {
                            //IF STAY TIMER HAS COUNTED DOWN
                            if (stayTimer == 0)
                            {
                                //IF SO - ASSIGN POSSIBLY RANDOM STAY TIMER
                                stayTimer = Random.Range(data.idleStay1, data.idleStay2);
                            }
                            //ELSE IF NOT
                            else if (stayTimer > 0)
                            {
                                //TICK AWAY AT IT
                                stayTimer -= Time.fixedDeltaTime;
                                //AND STOP MOVING
                                movement.StopMoving();
                            }
                            //IF STAY TIMER BELOW 0
                            else if (stayTimer < 0)
                            {
                                //CHECK TO SEE WHICH PATROL SPOT IS FURTHER
                                if (distances[0] < distances[1])
                                {
                                    //AND ASSIGN IT
                                    patrolDestination = patrolSpots[1].localPosition;
                                }
                                else
                                {
                                    patrolDestination = patrolSpots[0].localPosition;
                                }

                                //CHECK DIRECTION TO PATROL DESTINATION
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
                                //RESET STAY TIMER
                                stayTimer = 0;
                            }                            
                        }
                    }
                }
                //FOR FLYING UNITS
                else
                {
                    //IF NOT IDLE ALREADY
                    if (!idleRunning)
                    {
                        //SET LAST VISITED TO -1
                        lastVisited = -1;
                        //TURN IDLE ON
                        idleRunning = true;
                        for (int i = 0; i < patrolSpots.Count; i++)
                        {
                            //CHECK NEAREST PATROL SPOT
                            if (Vector2.Distance(npc.transform.position, patrolSpots[i].position) == Mathf.Min(distances))
                            {
                                //ASSIGN IT'S INDEX AS GOING TO
                                goingTo = i;
                                //AND ITS POSITION AS PATROL DESTINATION
                                patrolDestination = patrolSpots[i].position;
                            }
                        }
                        //AND ASSIGN POSSIBLY RANDOM STAY TIMER
                        stayTimer = Random.Range(data.idleStay1, data.idleStay2);
                    }
                    //IF ALREADY IDLE
                    else
                    {
                        //if (Physics2D.OverlapCircle(patrolSpots[goingTo].localPosition, 2f, groundLayer))
                        //{
                        //    //Debug.Log(patrolSpots[goingTo].ToString() + " overlaps with ground");
                        //    patrolSpots[goingTo].localPosition = new Vector2(patrolSpots[goingTo].localPosition.x, patrolSpots[goingTo].localPosition.y + Time.fixedDeltaTime * 3);
                        //}

                        //IF STAY TIMER HASN'T COUNTED DOWN
                        if (stayTimer > 0)
                        {                            
                            //TICK AWAY AT IT
                            stayTimer -= Time.fixedDeltaTime;
                            //STOP MOVING
                            movement.StopMoving();
                            //AND GO BACK TO TOP
                            return;
                        }
                        //IF STAY TIMER HAS COUNTED DOWN
                        else
                        {

                            //CHECK IF LAST VISITED -1 (WHICH ISN'T A PATROL SPOT) // WOULD BE IF NPC JUST TURNED IDLE
                            if (lastVisited == -1)
                            {
                                //IF PATROL DESTINATION IS OUT OF SIGHT
                                if (Vector2.Distance(npc.transform.position, patrolDestination) > data.visionRange / 2)
                                {
                                    //FLY TOWARDS IT
                                    movement.Move(data.flySpeed, patrolDestination);
                                }
                                //IF WITHIN SIGHT
                                else
                                {                     
                                    //IF NPC WAS NOT GOING TO THE LAST PATROL SPOT
                                    if (goingTo < patrolSpots.Count - 1)
                                    {
                                        //MAKE LAST VISITED BE THE SAME AS GOING TO
                                        lastVisited = goingTo;
                                        //AND INCREMENT 'GOING TO'
                                        goingTo++;
                                    }
                                    //ELSE IF NPC WAS GOING TO THE LAST PATROL SPOT
                                    else if (goingTo == patrolSpots.Count - 1)
                                    {
                                        //MAKE IT LAST VISITED
                                        lastVisited = goingTo;
                                        //AND DECREMENT 'GOING TO'
                                        goingTo--;
                                    }
                                    //ASSIGN POSSIBLY RANDOM STAY TIMER
                                    stayTimer = Random.Range(data.idleStay1, data.idleStay2);
                                    //AND GO BACK TO TOP
                                    return;
                                }
                            }
                            //IF LAST VISITED IS ONE OF THE PATROL SPOTS
                            else
                            {
                                //SET PATROL DESTINATION TO 'GOING TO'
                                patrolDestination = patrolSpots[goingTo].position;

                                //IF PATROL DESTINATION IS OUT OF SIGHT
                                if (Vector2.Distance(npc.transform.position, patrolDestination) > data.visionRange / 2)
                                {
                                    //FLY TOWARDS IT
                                    movement.Move(data.flySpeed, patrolDestination);
                                }
                                //IF NOT
                                else
                                {
                                    //CHECK
                                    //IF GOING TO SHOULD BE INCREMENTED OR DECREMENTED BASED ON IT'S AND LAST VISITED'S VALUES
                                    if (goingTo < patrolSpots.Count - 1 && lastVisited < goingTo)
                                    {
                                        lastVisited = goingTo;
                                        goingTo++;
                                    }
                                    else if (goingTo < patrolSpots.Count - 1 && goingTo != 0 && lastVisited > goingTo)
                                    {
                                        lastVisited = goingTo;
                                        goingTo--;
                                    }
                                    else if (goingTo == patrolSpots.Count - 1 && lastVisited < goingTo)
                                    {
                                        lastVisited = goingTo;
                                        goingTo--;
                                    }
                                    else if (goingTo == 0 && lastVisited > goingTo)
                                    {
                                        lastVisited = goingTo;
                                        goingTo++;
                                    }

                                    //ASSIGN A POSSIBLY RANDOM STAY TIMER
                                    stayTimer = Random.Range(data.idleStay1, data.idleStay2);
                                    //AND GO BACK TO TOP
                                    return;
                                }
                            }
                            //CHECK DIRECTION TO PATROL DESTINATION
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
                    }                    
                }
            }
            //TEST
            //else if (data.idleBehaviour == "Jump")
            //{
            //    float[] distances = new float[patrolSpots.Count];

            //    for (int i = 0; i < patrolSpots.Count; i++)
            //    {
            //        distances[i] = Vector2.Distance(npc.transform.position, patrolSpots[i].position);
            //    }

            //    if (!idleRunning)
            //    {
            //        idleRunning = true;
            //        stayTimer = Random.Range(data.idleStay1, data.idleStay2);

            //        if (distances[0] < distances[1])
            //        {
            //            //ASSIGN IT AS A PATROL DESTINATION
            //            patrolDestination = patrolSpots[0].localPosition;
            //        }
            //        else
            //        {
            //            patrolDestination = patrolSpots[1].localPosition;
            //        }

            //        //CHECK DIRECTION TO PATROL DESTINATION
            //        if (npc.transform.localPosition.x - patrolDestination.x < 0)
            //        {
            //            directionToDestination = 1;
            //        }
            //        if (npc.transform.localPosition.x - patrolDestination.x > 0)
            //        {
            //            directionToDestination = -1;
            //        }
            //        //FLIP NPC TO FACE IT
            //        movement.FlipNPC(directionToDestination);
            //    }
            //    //IF ALREADY IDLE
            //    else
            //    {
            //        //IF STAY TIMER HASN'T COUNTED DOWN
            //        if (stayTimer > 0)
            //        {
            //            //TICK AWAY AT IT
            //            stayTimer -= Time.fixedDeltaTime;
            //            //STOP MOVING
            //            movement.StopMoving();

            //            //Debug.Log("In range");
            //            curve.controlPoints[0].position = npc.transform.position;
            //            curve.controlPoints[3].position = patrolDestination;

            //            float distanceToTarget = Mathf.Abs(curve.controlPoints[0].position.x - curve.controlPoints[3].position.x);

            //            curve.controlPoints[1].position = new Vector2(curve.controlPoints[0].position.x + (distanceToTarget * 0.25f * directionToDestination), curve.controlPoints[0].position.y + (distanceToTarget / 2) + 
            //                Vector2.Distance(curve.controlPoints[0].position, curve.controlPoints[3].position) / 3);
            //            curve.controlPoints[2].position = new Vector2(curve.controlPoints[3].position.x - (distanceToTarget * 0.25f * directionToDestination), curve.controlPoints[3].position.y + (distanceToTarget / 2) +
            //                Vector2.Distance(curve.controlPoints[0].position, curve.controlPoints[3].position) / 3);
            //            curve.DrawCurve();
            //            //AND GO BACK TO TOP
            //            return;
            //        }

            //        //IF PATROL DESTINATION IS OUT OF SIGHT ON X AXIS
            //        if (Mathf.Abs(npc.transform.localPosition.x - patrolDestination.x) > data.visionRange)
            //        {
            //            //MOVE TOWARDS IT
            //            movement.Move(data.moveSpeed, patrolDestination);

            //            curve.controlPoints[0].position = npc.transform.position;
            //            curve.controlPoints[3].position = patrolDestination;
                        
            //            float distanceToTarget = Mathf.Abs(curve.controlPoints[0].position.x - curve.controlPoints[3].position.x);

            //            curve.controlPoints[1].position = new Vector2(curve.controlPoints[0].position.x + (distanceToTarget * 0.25f * directionToDestination), curve.controlPoints[0].position.y + (distanceToTarget / 2)/* +
            //                Vector2.Distance(curve.controlPoints[0].position, curve.controlPoints[3].position) / 3*/);
            //            curve.controlPoints[2].position = new Vector2(curve.controlPoints[3].position.x - (distanceToTarget * 0.25f * directionToDestination), curve.controlPoints[3].position.y + (distanceToTarget / 2)/* +
            //                Vector2.Distance(curve.controlPoints[0].position, curve.controlPoints[3].position) / 3*/);
            //            curve.DrawCurve();
            //        }
            //        //IF IN SIGHT
            //        else
            //        {
            //            //IF STAY TIMER HAS COUNTED DOWN
            //            if (stayTimer == 0)
            //            {
            //                //IF SO - ASSIGN POSSIBLY RANDOM STAY TIMER
            //                stayTimer = Random.Range(data.idleStay1, data.idleStay2);
            //            }
            //            //ELSE IF NOT
            //            else if (stayTimer > 0)
            //            {
            //                //TICK AWAY AT IT
            //                stayTimer -= Time.fixedDeltaTime;
            //                //AND STOP MOVING
            //                movement.StopMoving();


            //            }
            //            //IF STAY TIMER BELOW 0
            //            else if (stayTimer < 0)
            //            {
            //                //CHECK TO SEE WHICH PATROL SPOT IS FURTHER
            //                if (distances[0] < distances[1])
            //                {
            //                    //AND ASSIGN IT
            //                    patrolDestination = patrolSpots[1].localPosition;
            //                }
            //                else
            //                {
            //                    patrolDestination = patrolSpots[0].localPosition;
            //                }

            //                //CHECK DIRECTION TO PATROL DESTINATION
            //                if (npc.transform.localPosition.x - patrolDestination.x < 0)
            //                {
            //                    directionToDestination = 1;
            //                }
            //                if (npc.transform.localPosition.x - patrolDestination.x > 0)
            //                {
            //                    directionToDestination = -1;
            //                }

            //                //FLIP NPC TO FACE IT
            //                movement.FlipNPC(directionToDestination);
            //                //RESET STAY TIMER
            //                stayTimer = 0;
            //            }
            //        }
            //    }


            //}
        }
    }

    private void OnDrawGizmos()
    {
        if (patrolDestination != Vector2.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(patrolDestination, 1f);
        }
    }
}
