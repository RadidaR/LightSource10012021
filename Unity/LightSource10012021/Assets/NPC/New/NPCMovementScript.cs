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

            //HANDLES LEDGES
            if (abilities.avoidsLedges)
            {
                if (states.onLedge)
                {
                    StopMoving();
                }
            }

            //HANDLES CLIMBING
            if (abilities.canClimb)
            {
                if (!states.nextToWall)
                {
                    states.isClimbing = false;
                }
            }
        }

        //TURNS STILL BOOLEAN OFF
        if (states.isClimbing || states.isWalking || states.isRunning || states.isFlying)
        {
            states.isStill = false;
        }
    }

    public void StopMoving()
    {
        //GROUND UNITS
        if (!abilities.canFly)
        {
            if (states.isClimbing)
            {
                rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                //rigidBody.velocity = new Vector2(0, 0);
            }
            else
            {
                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            }
        }
        //FLYING UNITS
        else
        {
            navAgent.destination = npc.transform.position;
        }
    }

    public void Move(float speed, Vector2 position)
    {
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        //GROUND UNITS
        if (!abilities.canFly)
        {
            //IF CONNECTED TO A SURFACE                                     //WILL PROBS ADD 'IF CAN MOVE' AND 'IF NOT HURT' OR SOMETHING OF THIS NATURE
            if (states.isGrounded || states.isClimbing)
            {
                //IF NOT CHASING ANYTHING
                if (!states.isChasing)
                {
                    //HANDLES LEDGES
                    if (abilities.avoidsLedges)
                    {
                        if (states.onLedge)
                        {
                            FlipNPC(states.facingDirection * -1);
                        }
                    }

                    //HANDLES WALLS
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

                    //ASSIGN VELOCITY
                    rigidBody.velocity = new Vector2(speed * states.facingDirection, rigidBody.velocity.y);
                }
                //IF CHASING SOMETHING
                else
                {
                    //HANDLES LEDGES
                    if (abilities.avoidsLedges)
                    {
                        if (states.onLedge)
                        {
                            StopMoving();
                            return;
                        }
                    }

                    //HANDLES WALLKS
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

                    //ASIGN VELOCITY
                    rigidBody.velocity = new Vector2(speed * states.facingDirection, rigidBody.velocity.y);
                }
            }
        }
        //FLYING UNITS
        else
        {
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
