using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public PlayerInput playerInputData;
    public PlayerMovement playerMovementData;
    public PlayerStates playerStatesData;
    public PlayerStamina playerStaminaData;

    public GameEvent eVelocityZero;
    public GameEvent eJumpStarted;
    public GameEvent eJumpEnded;
    public GameEvent eWalking;
    public GameEvent eRunning;

    public GameEvent eUseStamina;
    public GameEvent eInsufficientStamina;

    public Rigidbody2D rigidBody;
    public Vector2 velocity;

    //float moveSpeed;
    //float maxSpeed;
    //float jumpForce;
    [SerializeField] float jumpDuration;
    GameObject player;
    //public float jumpCost;

    //[Range(-1, 1)] public float direction;

    // Start is called before the first frame update
    void Start()
    {
        jumpDuration = playerMovementData.jumpDuration;
        player = GameObject.FindWithTag("Player");
        //rigidBody = playerMovement.rigidBody;
    }

    // Update is called once per frame
    void Update()
    {
        //////Move these to Start(), in here now for playtesting and balance tweaks
        //moveSpeed = playerMovementData.moveSpeed;
        //jumpForce = playerMovementData.jumpForce;
        //jumpCost = playerMovementData.jumpCost;
        //////Move these to Start(), in here now for playtesting and balance tweaks
    }

    private void FixedUpdate()
    {
        //Pass velocity value to Data
        velocity = rigidBody.velocity;
        playerMovementData.playerVelocity = velocity;

        if (velocity == Vector2.zero)
        {
            eVelocityZero.Raise();
        }

        if (jumpDuration < playerMovementData.jumpDuration && !playerStatesData.isJumping)
        {
            jumpDuration = playerMovementData.jumpDuration;
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
        velocity.x = playerMovementData.moveSpeed * playerInputData.leftStickValue;
        rigidBody.velocity = velocity;

        if (playerStatesData.isGrounded && Mathf.Abs(rigidBody.velocity.x) < 8)
        {
            eWalking.Raise();
        }
        else if (playerStatesData.isGrounded && Mathf.Abs(rigidBody.velocity.x) > 8)
        {
            eRunning.Raise();
        }
    }

    public void Jump()
    {
        if (jumpDuration < 0)
        {
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
