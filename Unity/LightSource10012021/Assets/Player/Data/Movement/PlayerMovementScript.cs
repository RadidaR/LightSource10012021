using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public PlayerInput playerInputData;
    public PlayerMovement playerMovementData;
    public PlayerStates playerStatesData;
    public PlayerStamina playerStaminaData;
    public PlayerFloating playerFloatingData;

    public GameEvent eVelocityZero;
    public GameEvent eWalking;
    public GameEvent eRunning;
    public GameEvent eFloatMove;

    public GameEvent eJumpStarted;
    public GameEvent eJumpEnded;

    public GameEvent eUseStamina;
    public GameEvent eInsufficientStamina;

    public Rigidbody2D rigidBody;
    //public Vector2 velocity;

    [SerializeField] float jumpDuration;
    GameObject player;


    void Start()
    {
        jumpDuration = playerMovementData.jumpDuration;
        player = GameObject.FindWithTag("Player");
    }

    private void FixedUpdate()
    {
        //velocity = rigidBody.velocity;
        playerMovementData.playerVelocity = rigidBody.velocity;

        if (rigidBody.velocity == Vector2.zero)
        {
            eVelocityZero.Raise();
        }

        if (jumpDuration < playerMovementData.jumpDuration && !playerStatesData.isJumping)
        {
            jumpDuration = playerMovementData.jumpDuration;
        }

        if (playerInputData.leftStickValue == 0)
        {
            if (playerStatesData.isGrounded)
            {
                if (Mathf.Abs(rigidBody.velocity.x) > 0)
                {
                    Vector2 slowDown = rigidBody.velocity;
                    slowDown.x -= slowDown.x * 10 * Time.fixedDeltaTime;
                    rigidBody.velocity = slowDown;
                }
                if (Mathf.Abs(rigidBody.velocity.x) < 0.05f)
                {
                    rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                }
            }
            if (playerStatesData.isFloating)
            {
                if (Mathf.Abs(rigidBody.velocity.x) > 0)
                {
                    Vector2 slowDown = rigidBody.velocity;
                    slowDown.x -= slowDown.x * Time.fixedDeltaTime;
                    rigidBody.velocity = slowDown;
                }
                if (Mathf.Abs(rigidBody.velocity.x) < 0.05f)
                {
                    rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                }
            }
        }
    }

    public void FaceLeft()
    {
        playerMovementData.facingDirection = -1;
        Vector2 playerScale = player.transform.localScale;
        Vector2 scale = new Vector2(-1, playerScale.y);
        player.transform.localScale = scale;
    }

    public void FaceRight()
    {
        playerMovementData.facingDirection = 1;
        Vector2 playerScale = player.transform.localScale;
        Vector2 scale = new Vector2(1, playerScale.y);
        player.transform.localScale = scale;
    }

    public void Move()
    {
        if (!playerStatesData.isFalling && !playerStatesData.isHurt)
        {
            Vector2 velocity = rigidBody.velocity;
            if (!playerStatesData.isFloating)
            {
                if (playerStatesData.isGrounded || playerStatesData.isJumping)
                {
                    if (Mathf.Abs(rigidBody.velocity.x) < playerMovementData.moveSpeed)
                    {
                        velocity.x = playerMovementData.moveSpeed * playerInputData.leftStickValue;
                        rigidBody.velocity = velocity;
                        return;
                    }

                    if (Mathf.Abs(playerMovementData.playerVelocity.x) >= playerMovementData.moveSpeed)
                    {
                        if (rigidBody.velocity.x > 0)
                        {
                            if (playerMovementData.facingDirection == 1)
                            {
                                velocity.x += playerMovementData.accelerationRate * Time.fixedDeltaTime;
                                if (rigidBody.velocity.x > playerMovementData.maxSpeed)
                                {
                                    velocity.x = playerMovementData.maxSpeed;
                                }
                            }
                            else
                            {
                                velocity.x = playerMovementData.moveSpeed * playerInputData.leftStickValue;
                                rigidBody.velocity = velocity;
                                return;
                            }
                        }

                        if (rigidBody.velocity.x < 0)
                        {
                            if (playerMovementData.facingDirection == -1)
                            {
                                velocity.x -= playerMovementData.accelerationRate * Time.fixedDeltaTime;
                                if (rigidBody.velocity.x < -playerMovementData.maxSpeed)
                                {
                                    velocity.x = -playerMovementData.maxSpeed;
                                }
                            }
                            else
                            {
                                velocity.x = playerMovementData.moveSpeed * playerInputData.leftStickValue;
                                rigidBody.velocity = velocity;
                                return;
                            }
                        }
                        rigidBody.velocity = velocity;
                    }
                }

                if (playerStatesData.isAirborne)
                {                    
                    if (Mathf.Abs(rigidBody.velocity.x) > playerMovementData.moveSpeed * 0.75f)
                    {
                        Vector2 slowDown = rigidBody.velocity;
                        slowDown.x -= slowDown.x * 0.4f * Time.deltaTime;
                        rigidBody.velocity = slowDown;
                        return;
                    }
                    velocity.x = playerMovementData.moveSpeed * playerInputData.leftStickValue * 0.75f;
                    rigidBody.velocity = velocity;
                }
            }
            else
            {
                eFloatMove.Raise();
            }

        }

        if (playerStatesData.isGrounded && Mathf.Abs(rigidBody.velocity.x) < playerMovementData.moveSpeed)
        {
            eWalking.Raise();
        }
        else if (playerStatesData.isGrounded && Mathf.Abs(rigidBody.velocity.x) > playerMovementData.moveSpeed)
        {
            eRunning.Raise();
        }
    }

    public void Jump()
    {
        Vector2 velocity = rigidBody.velocity;

        if (jumpDuration < 0)
        {
            playerInputData.buttonSouth = 0f;
            eJumpEnded.Raise();
            return;
        }

        if (playerStatesData.isGrounded)
        {
            if (playerStaminaData.currentStamina > playerMovementData.jumpCost)
            {
                eJumpStarted.Raise();
                playerStaminaData.staminaCost = playerMovementData.jumpCost;
                eUseStamina.Raise();
                velocity.y = playerMovementData.jumpForce;
                rigidBody.velocity = velocity;
            }
            else
            {
                eInsufficientStamina.Raise();
            }
        }

        if (playerStatesData.isJumping)
        {
            velocity.y = playerMovementData.jumpForce;
            rigidBody.velocity = velocity;
        }

        jumpDuration -= Time.fixedDeltaTime;
    }
}
