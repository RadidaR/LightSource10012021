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
    public float wallCheckRadius;

    public LayerMask groundLayer;
    //public bool wallAhead;

    public NavMeshAgent2D navAgent;

    public float lostChaseTimer;

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
        }
    }

    private void Start()
    {
        if (navAgent != null)
        {
            //ASSIGNS FLY SPEED AND ACCELERATION
            navAgent.speed = data.flySpeed;
            navAgent.acceleration = data.flyAcceleration;
        }
    }


    public void FixedUpdate()
    {
        //HANDLES NEXT TO WALL BOOLEAN
        states.nextToWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, groundLayer);

        //FOR GROUND UNITS
        if (!abilities.canFly)
        {
            //HANDLES GROUNDED AND ON LEDGE BOOLEANS
            states.isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            states.onLedge = !Physics2D.OverlapCircle(ledgeCheck.position, ledgeCheckRadius, groundLayer);

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

            if (abilities.avoidsLedges)
            {
                if (states.onLedge)
                {
                    StopMoving();
                }
            }

            if (abilities.canClimb)
            {
                if (!states.nextToWall)
                {
                    states.isClimbing = false;
                }
            }
        }

        if (states.isClimbing || states.isWalking || states.isRunning || states.isFlying)
        {
            states.isStill = false;
        }
    }

    public void StopMoving()
    {
        if (!abilities.canFly)
        {
            //velocity = rigidBody.velocity;
            //velocity.x = 0;
            //rigidBody.velocity = velocity;
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            //states.isStill = true;
        }
        else
        {
            navAgent.destination = npc.transform.position;
        }
    }

    //public void Walk()
    //{
    //    velocity = rigidBody.velocity;
    //    velocity.x = data.moveSpeed * states.facingDirection;
    //    rigidBody.velocity = velocity;
    //}

    //public void Run()
    //{
    //    velocity = rigidBody.velocity;
    //    velocity.x = data.runSpeed * states.facingDirection;
    //    rigidBody.velocity = velocity;
    //}

    public void Move(float speed, Vector2 position)
    {
        //velocity = rigidBody.velocity;
        //velocity.x = speed * states.facingDirection;
        if (!abilities.canFly)
        {
            if (states.isGrounded || states.isClimbing)
            {
                if (!states.isChasing)
                {
                    if (abilities.avoidsLedges)
                    {
                        if (states.onLedge)
                        {
                            FlipNPC(states.facingDirection * -1);
                        }
                    }

                    if (states.nextToWall)
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

                    rigidBody.velocity = new Vector2(speed * states.facingDirection, rigidBody.velocity.y);
                }
                else
                {
                    if (abilities.avoidsLedges)
                    {
                        if (states.onLedge)
                        {
                            //if (lostChaseTimer > 0)
                            //{
                            //    lostChaseTimer -= Time.fixedDeltaTime;
                            //}
                            //else if (lostChaseTimer < 0)
                            //{
                            //    lostChaseTimer = 0;
                            //    states.isChasing = false;
                            //    StopMoving();
                            //    return;
                            //}
                            //else if (lostChaseTimer == 0)
                            //{
                            //    lostChaseTimer = 3;
                            //}
                            StopMoving();
                            return;
                        }
                    }

                    if (states.nextToWall)
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

                    rigidBody.velocity = new Vector2(speed * states.facingDirection, rigidBody.velocity.y);
                }
                //states.isStill = false;
                //if (Mathf.Abs(rigidBody.velocity.x) > 0.25f && Mathf.Abs(rigidBody.velocity.x) <= data.moveSpeed)
                //{
                //    states.isRunning = false;
                //    states.isWalking = true;
                //}
                //else if (Mathf.Abs(rigidBody.velocity.x) > data.moveSpeed)
                //{
                //    states.isWalking = false;
                //    states.isRunning = true;
                //}
            }
        }
        else
        {
            if (!states.isHurt)
            {
                //states.isFlying = true;
                navAgent.destination = position;
            }
        }
    }

    public void Climb(float speed)
    {
        //states.isStill = false;
        states.isClimbing = true;
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, speed);
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
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (ledgeCheckRadius > 0)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(ledgeCheck.position, ledgeCheckRadius);
        }

        if (wallCheckRadius > 0)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
        }
    }
}
