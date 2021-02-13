using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [Header("Data Types")]
    public PlayerInputData playerInputData;
    public PlayerMovementData playerMovementData;
    public PlayerStatesData playerStatesData;
    public PlayerStaminaData playerStaminaData;

    [Header("Events")]
    public GameEvent eVelocityZero;
    public GameEvent eWalking;
    public GameEvent eRunning;
    public GameEvent eFloatMove;

    public GameEvent eJumpStarted;
    public GameEvent eJumpEnded;

    public GameEvent eUseStamina;
    public GameEvent eInsufficientStamina;

    [Header("Local Variables")]
    [SerializeField] float jumpDuration;
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] GameObject player;


    void Start()
    {
        jumpDuration = playerMovementData.jumpDuration;
        rigidBody = gameObject.GetComponentInParent<Rigidbody2D>();
        player = rigidBody.gameObject;
    }

    private void Update()
    {        
        //if (playerStatesData.isThrowing)
        //{
        //    if (playerInputData.rightStickValue.x > 0)
        //    {
        //        FaceRight();
        //    }
        //    if (playerInputData.rightStickValue.x < 0)
        //    {
        //        FaceLeft();
        //    }
        //}
    }

    private void FixedUpdate()
    {
        playerMovementData.playerVelocity = rigidBody.velocity;

        //SIGNALS PLAYER IS STILL
        if (rigidBody.velocity == Vector2.zero)
        {
            eVelocityZero.Raise();
        }

        //RESETS JUMP DURATION
        if (jumpDuration < playerMovementData.jumpDuration && !playerStatesData.isJumping)
        {
            jumpDuration = playerMovementData.jumpDuration;
        }

        //STOPS MOVEMENT IF LEFT STICK IS 0
        if (playerInputData.leftStickValue == 0)
        {
            if (playerStatesData.isGrounded)
            {
                //GRADUALLY SLOWS DOWN OVER 0.1 SEC
                if (Mathf.Abs(rigidBody.velocity.x) > 0)
                {
                    Vector2 slowDown = rigidBody.velocity;
                    slowDown.x -= slowDown.x * 10 * Time.fixedDeltaTime;
                    rigidBody.velocity = slowDown;
                }
                //BRINGS TO A HALT
                if (Mathf.Abs(rigidBody.velocity.x) < 0.05f)
                {
                    rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                }
            }
            if (playerStatesData.isFloating)
            {
                //GRADUALLY SLOWS DOWN OVER 1 SEC
                if (Mathf.Abs(rigidBody.velocity.x) > 0)
                {
                    Vector2 slowDown = rigidBody.velocity;
                    slowDown.x -= slowDown.x * Time.fixedDeltaTime;
                    rigidBody.velocity = slowDown;
                }
                //BRINGS TO A HALT
                if (Mathf.Abs(rigidBody.velocity.x) < 0.05f)
                {
                    rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                }
            }
        }

        //PREVENTS FROM CONTINUING AT HIGH SPEED ONCE DASH IS OVER
        if (!playerStatesData.isDashing)
        {
            if (Mathf.Abs(rigidBody.velocity.x) > playerMovementData.maxSpeed)
            {
                Vector2 velocity = rigidBody.velocity;
                velocity.x = playerMovementData.maxSpeed * playerMovementData.facingDirection;
                rigidBody.velocity = velocity;
            }
        }
    }




    //FLIPS PLAYER LEFT
    public void FacingDirection()
    {
        if (!playerStatesData.isThrowing && playerInputData.rightStickValue == Vector2.zero)
        {
            if (playerInputData.leftStickValue < 0)
            {
                FaceLeft();
            }
            else if (playerInputData.leftStickValue > 0)
            {
                FaceRight();
            }
        }
        else
        {
            if (playerInputData.rightStickValue.x < 0)
            {
                FaceLeft();
            }
            else if (playerInputData.rightStickValue.x > 0)
            {
                FaceRight();
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

    //FLIPS PLAYER RIGHT
    public void FaceRight()
    {
        playerMovementData.facingDirection = 1;
        Vector2 playerScale = player.transform.localScale;
        Vector2 scale = new Vector2(1, playerScale.y);
        player.transform.localScale = scale;
    }

    public void Move()
    {
        //WHEN NOT FALLING OR HURT
        if (!playerStatesData.isFalling && !playerStatesData.isHurt)
        {
            Vector2 velocity = rigidBody.velocity;
            //WHEN NOT FLOATING
            if (!playerStatesData.isFloating)
            {
                //WHEN GROUNDED OR JUMPING
                if (playerStatesData.isGrounded || playerStatesData.isJumping)
                {
                    //START MOVEMENT
                    if (Mathf.Abs(rigidBody.velocity.x) < playerMovementData.moveSpeed)
                    {
                        velocity.x = playerMovementData.moveSpeed * playerInputData.leftStickValue;
                        rigidBody.velocity = velocity;
                        return;
                    }

                    //ACCELLERATE TO MAX SPEED
                    if (Mathf.Abs(playerMovementData.playerVelocity.x) >= playerMovementData.moveSpeed)
                    {
                        //CHECK DIRECTION OF MOTION
                        if (rigidBody.velocity.x > 0)
                        {
                            //CHECK FACING DIRECTION
                            if (playerMovementData.facingDirection == 1)
                            {
                                //ACCELERATE OVER TIME
                                velocity.x += playerMovementData.accelerationRate * Time.fixedDeltaTime;
                                //DONT EXCEED MAX SPEED
                                if (rigidBody.velocity.x > playerMovementData.maxSpeed)
                                {
                                    velocity.x = playerMovementData.maxSpeed;
                                }
                            }
                            //TURN AROUND
                            else
                            {
                                velocity.x = playerMovementData.moveSpeed * playerInputData.leftStickValue;
                                rigidBody.velocity = velocity;
                                return;
                            }
                        }
                        //CHECK DIRECTION OF MOTION
                        if (rigidBody.velocity.x < 0)
                        {
                            //CHECK FACING DIRECTION
                            if (playerMovementData.facingDirection == -1)
                            {
                                //ACCELERATE OVER TIME
                                velocity.x -= playerMovementData.accelerationRate * Time.fixedDeltaTime;
                                //DONT EXCEED MAX SPEED
                                if (rigidBody.velocity.x < -playerMovementData.maxSpeed)
                                {
                                    velocity.x = -playerMovementData.maxSpeed;
                                }
                            }
                            //TURN AROUND
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
                //WHEN AIRBORNE
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
            //FLOAT MOVE
            else
            {
                eFloatMove.Raise();
            }
        }

        //RAISES WALKING
        if (playerStatesData.isGrounded && Mathf.Abs(rigidBody.velocity.x) <= playerMovementData.moveSpeed)
        {
            eWalking.Raise();
        }
        //RAISES RUNNING
        else if (playerStatesData.isGrounded && Mathf.Abs(rigidBody.velocity.x) > playerMovementData.moveSpeed)
        {
            eRunning.Raise();
        }
    }

    public void Jump()
    {
        Vector2 velocity = rigidBody.velocity;

        //END JUMP IF DURATION OVER
        if (jumpDuration < 0)
        {
            eJumpEnded.Raise();
            return;
        }

        //IF GROUNDED
        if (playerStatesData.isGrounded && !playerStatesData.isHurt && !playerStatesData.isJumping && !playerStatesData.isAirborne)
        {
            //AND HAS ENOUGH STAMINA
            if (playerStaminaData.currentStamina > playerMovementData.jumpCost)
            {                
                //DRAIN STAMINA
                playerStaminaData.staminaCost = playerMovementData.jumpCost;
                eUseStamina.Raise();
                //AND JUMP
                eJumpStarted.Raise();
                velocity.y = playerMovementData.jumpForce;
                rigidBody.velocity = velocity;
            }
            //IF NOT ENOUGH STAMINA
            else
            {
                eInsufficientStamina.Raise();
            }
        }

        //HOLD TO JUMP HIGHER
        if (playerStatesData.isJumping)
        {
            velocity.y = playerMovementData.jumpForce;
            rigidBody.velocity = velocity;
        }

        //TICK AWAY AT DURATION
        jumpDuration -= Time.fixedDeltaTime;
    }
}
