using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementScript : MonoBehaviour
{
    public GameObject npc;

    public NPCData data;
    public NPCAbilities abilities;

    public NPCStatesScript states;

    public Rigidbody2D rigidBody;

    public Vector2 velocity;

    public Transform groundCheck;
    public float groundCheckRadius;

    public Transform ledgeCheck;
    public float ledgeCheckRadius;

    //public bool ledgeAhead;

    public Transform wallCheck;
    public Vector2 wallCheckSize;

    public Transform stepCheck;
    public float stepCheckRadius;

    public LayerMask groundLayer;
    //public bool wallAhead;

    public NavMeshAgent2D navAgent;

    public float jumpDelay;

    public bool delaying;

    public void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            npc = GetComponentInParent<OfInterest>().gameObject;

            states = npc.GetComponent<NPCStatesScript>();

            data = states.data;
            abilities = states.abilities;

            rigidBody = npc.GetComponent<Rigidbody2D>();

            if (abilities.canFly)
            {
                navAgent = npc.GetComponent<NavMeshAgent2D>();
            }

            wallCheckSize = wallCheck.gameObject.GetComponent<CapsuleCollider2D>().size;
        }
    }

    private void Start()
    {
        

        if (navAgent != null)
        {
            //ASSIGNS FLY SPEED AND ACCELERATION
            navAgent.speed = data.flySpeed;
            navAgent.acceleration = data.flyAcceleration;
            rigidBody.gravityScale = 0;
        }
    }


    public void FixedUpdate()
    {
        //STEP
        states.stepAhead = Physics2D.OverlapCircle(stepCheck.position, stepCheckRadius, groundLayer);
        //if (!delaying)
        //{
        //    StopCoroutine(JumpCo(0, 0));
        //    Debug.Log("Stopped Coroutine");
        //    delaying = false;
        //}
        if (!states.stepAhead)
        {
            StopCoroutine(JumpCo(0,0));
            states.wallAhead = false;
            states.isClimbing = false;
        }

        if (jumpDelay > 0)
        {
            jumpDelay -= Time.fixedDeltaTime;
        }

        //HANDLES NEXT TO WALL BOOLEAN        
        states.wallAhead = Physics2D.OverlapCapsule(wallCheck.position, wallCheckSize, CapsuleDirection2D.Vertical, 0, groundLayer);

        //FOR GROUND UNITS
        if (!abilities.canFly)
        {
            //HANDLES GROUNDED AND ON LEDGE BOOLEANS
            states.isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            states.ledgeAhead = !Physics2D.OverlapCircle(ledgeCheck.position, ledgeCheckRadius, groundLayer);

            //HANDLES STILL, WALKING, AND RUNNING BOOLEANS
            if (Mathf.Abs(rigidBody.velocity.x) < 0.25f && Mathf.Abs(rigidBody.velocity.y) < 0.25f)
            {
                states.isStill = true;
                states.isWalking = false;
                states.isRunning = false;
            }
            if (Mathf.Abs(rigidBody.velocity.x) > 0.25f && Mathf.Abs(rigidBody.velocity.x) <= data.moveSpeed)
            {
                states.isRunning = false;
                states.isWalking = true;
            }
            else if (Mathf.Abs(rigidBody.velocity.x) > data.moveSpeed)
            {
                states.isWalking = false;
                states.isRunning = true;
            }

            if (states.isGrounded)
            {
                states.isJumping = false;
                jumpDelay = 0;
            }

            if (states.isClimbing)
            {
                states.isWalking = false;
                states.isRunning = false;
            }

            //HANDLES LEDGES
            if (abilities.avoidsLedges)
            {
                if (states.ledgeAhead)
                {
                    StopMoving();
                }
            }

            //HANDLES CLIMBING
            //if (abilities.canClimb)
            //{
            //    if (!states.wallAhead && !states.stepAhead)
            //    {
            //        states.isClimbing = false;
            //    }
            //}
        }

        //TURNS STILL BOOLEAN OFF
        if (states.isClimbing || states.isWalking || states.isRunning || states.isFlying)
        {
            states.isStill = false;
        }

        if (!states.isGrounded && !states.isClimbing)
        {
            states.isAirborne = true;
        }
        else
        {
            states.isAirborne = false;
        }
    }

    public void StopMoving()
    {
        //GROUND UNITS
        if (!abilities.canFly)
        {
            if (!states.isJumping)                      //JUMP STUFF
            {
                if (states.isClimbing)
                {
                    rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    //rigidBody.velocity = new Vector2(0, 0);
                }
                else
                {
                    if (Mathf.Abs(rigidBody.velocity.x) < 2.25f)
                    {
                        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                        return;
                    }
                    else
                    {
                        float slowDown = rigidBody.velocity.x;
                        slowDown += slowDown * Time.fixedDeltaTime * -1 * 5;
                        rigidBody.velocity = new Vector2(slowDown, rigidBody.velocity.y);
                    }
                }
            }
        }
        //FLYING UNITS
        else
        {
            if (navAgent != null)
            {
                //Debug.Log("Stopping");
                if (!states.isChasing)
                {
                    Vector2 direction = new Vector2(navAgent.destination.x - npc.transform.position.x, navAgent.destination.y - npc.transform.position.y).normalized;
                    navAgent.speed = data.flySpeed / 5;
                    navAgent.acceleration = data.flyAcceleration / 5;
                    if (Vector2.Distance(npc.transform.position, navAgent.destination) > data.visionRange + data.visionExpansion)
                    {
                        //direction.x += data.visionRange;
                        //direction.y += data.visionRange;
                        navAgent.destination = new Vector2(npc.transform.position.x + (direction.x * data.visionRange), npc.transform.position.y + (direction.y * data.visionRange));
                    }

                    //if (Vector2.Distance(npc.transform.position, navAgent.destination) < data.visionRange)
                    //{
                    //    navAgent.speed = data.flySpeed / 5;
                    //    navAgent.acceleration = data.flyAcceleration / 5;
                    //    //navAgent.destination = npc.transform.position;
                    //}

                    if (Vector2.Distance(npc.transform.position, navAgent.destination) < data.visionRange / 2)
                    {
                        navAgent.speed = data.flySpeed / 10;
                        navAgent.acceleration = data.flyAcceleration / 10;
                        //navAgent.destination = npc.transform.position;
                    }

                    if (Vector2.Distance(npc.transform.position, navAgent.destination) < data.visionRange / 5)
                    {
                        //navAgent.speed = data.flySpeed / 10;
                        //navAgent.acceleration = data.flyAcceleration / 10;
                        navAgent.destination = npc.transform.position;
                    }
                }
                else
                {
                    navAgent.destination = npc.transform.position;
                }

                //navAgent.destination = npc.transform.position;
            }
        }
    }


    public void Move(float speed, Vector2 position)
    {
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        //GROUND UNITS
        if (!abilities.canFly)
        {
            //IF CONNECTED TO A SURFACE                                     //WILL PROBS ADD 'IF CAN MOVE' AND 'IF NOT HURT' OR SOMETHING OF THIS NATURE
            if (states.isGrounded || states.isClimbing || states.isJumping)
            {
                //IF NOT CHASING ANYTHING
                if (!states.isChasing)
                {
                    //HANDLES LEDGES
                    if (states.ledgeAhead)
                    {
                        if (abilities.avoidsLedges)
                        {
                            FlipNPC(states.facingDirection * -1);
                        }
                    }

                    if (states.stepAhead)
                    {
                        if (!states.wallAhead)
                        {
                            if (!states.isClimbing)
                            {
                                if (abilities.canJump)
                                {
                                    StopMoving();
                                    StartCoroutine(JumpCo(data.jumpDelay, data.jumpForce));
                                    return;
                                }
                            }
                            else
                            {
                                Climb(data.climbSpeed);
                            }
                        }
                        else
                        {
                            if (abilities.canClimb)
                            {
                                Climb(data.climbSpeed);
                            }
                            else
                            {
                                FlipNPC(states.facingDirection * -1);
                            }
                        }
                    }

                    //HANDLES WALLS
                    //if (states.wallAhead)
                    //{
                    //    if (abilities.canClimb)
                    //    {
                    //        Climb(data.climbSpeed);
                    //    }
                    //    else
                    //    {
                    //        FlipNPC(states.facingDirection * -1);
                    //    }
                    //}

                    //ASSIGN VELOCITY
                    rigidBody.velocity = new Vector2(speed * states.facingDirection, rigidBody.velocity.y);
                }
                //IF CHASING SOMETHING
                else
                {
                    if (!states.isTelegraphing && !states.isAttacking)
                    {
                        //HANDLES LEDGES
                        if (abilities.avoidsLedges)
                        {
                            if (states.ledgeAhead)
                            {
                                StopMoving();
                                return;
                            }
                        }

                        if (states.stepAhead)
                        {
                            if (!states.wallAhead)
                            {
                                if (!states.isClimbing)
                                {
                                    if (abilities.canJump)
                                    {
                                        StartCoroutine(JumpCo(0, data.jumpForce));
                                    }
                                }
                                else
                                {
                                    Climb(data.climbSpeed);
                                }
                            }
                            else
                            {
                                if (abilities.canClimb)
                                {
                                    Climb(data.climbSpeed);
                                }
                                else
                                {
                                    StopMoving();
                                }
                            }
                        }

                        //HANDLES WALLS
                        //if (states.wallAhead)
                        //{
                        //    if (abilities.canClimb)
                        //    {                            
                        //        Climb(data.climbSpeed);
                        //    }
                        //    else
                        //    {
                        //        StopMoving();
                        //    }
                        //}

                        //ASIGN VELOCITY
                        rigidBody.velocity = new Vector2(speed * states.facingDirection, rigidBody.velocity.y);
                    }
                }
            }
        }
        //FLYING UNITS
        else
        {
            navAgent.speed = data.flySpeed;
            navAgent.acceleration = data.flyAcceleration;
            //IF NOT HURT 
            if (!states.isHurt)
            {
                navAgent.destination = position;
            }
        }
    }

    public void Climb(float speed)
    {
        states.isClimbing = true;
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, speed);
    }

    public void Jump(float force)
    {
        //if (jumpDelay == 0)
        //{
        //    jumpDelay = data.jumpDelay;
        //}
        //else
        //{
        //    rigidBody.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
        //    if (!states.isGrounded)
        //    {
        //        states.isJumping = true;
        //        jumpDelay = 0;
        //    }
        //}
        states.isJumping = true;
        rigidBody.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
    }

    //public IEnumerator JumpCo(float delay, float force)
    //{
    //    if (!delaying)
    //    {
    //        delaying = true;
    //        Debug.Log("Started");
    //        while (delay > 0)
    //        {
    //            StopMoving();
    //            delay -= Time.deltaTime;
    //            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
    //            if (delay <= 0)
    //            {
    //                Debug.Log("Shoot up");
    //                rigidBody.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
    //                break;
    //            }
    //            else if (!states.stepAhead)
    //            {
    //                break;
    //                delaying = false;
    //            }
    //        }

    //        while (states.isGrounded)
    //        {
    //            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
    //            if (!states.isGrounded)
    //            {
    //                states.isJumping = true;
    //                break;
    //            }
    //        }

    //        while (states.isJumping)
    //        {
    //            Move(data.runSpeed, Vector2.zero);
    //            yield return new WaitForSecondsRealtime(Time.deltaTime);
    //            if (!states.isJumping)
    //            {
    //                break;
    //            }
    //            //if (states.isGrounded)
    //            //{
    //            //    break;
    //            //}
    //        }

    //        delaying = false;
    //    }
    //}

    public IEnumerator JumpCo(float delay, float force)
    {
        if (!delaying)
        {
            delaying = true;
            while (delay >= 0)
            {
                StopMoving();
                delay -= Time.deltaTime;
                yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
                if (delay < 0)
                {
                    if (states.stepAhead)
                    {
                        rigidBody.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
                    }
                    break;
                }
                else if (!states.stepAhead)
                {
                    delaying = false;
                    break;
                }
            }

            int unStuckCounter = 0;
            while (states.isGrounded)
            {
                yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
                if (!states.isGrounded)
                {
                    states.isJumping = true;
                    break;
                }
                unStuckCounter++;
                //Debug.Log(unStuckCounter.ToString());
                if (unStuckCounter > 25)
                {
                    delaying = false;
                    break;
                }
            }

            while (states.isJumping)
            {
                if (!states.isChasing)
                {
                    Move(data.moveSpeed, Vector2.zero);
                }
                else
                {
                    Move(data.runSpeed, Vector2.zero);
                }
                yield return new WaitForSecondsRealtime(Time.deltaTime);
                if (!states.isJumping)
                {
                    break;
                }
                //if (states.isGrounded)
                //{
                //    break;
                //}
            }
            delaying = false;
        }
    }

    public void FlipNPC(int direction)
    {
        Vector2 npcScale = npc.transform.localScale;
        npcScale.x = direction;
        npc.transform.localScale = npcScale;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckRadius > 0)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (ledgeCheckRadius > 0)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(ledgeCheck.position, ledgeCheckRadius);
        }

        if (stepCheckRadius > 0)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(stepCheck.position, stepCheckRadius);
        }

        //if (navAgent != null)
        //{
        //    if (states.isIdle || states.isChasing)
        //    {
        //        Gizmos.color = Color.red;
        //        Gizmos.DrawWireSphere(navAgent.destination, 5);
        //    }
        //}
    }
}
