using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public PlayerInput playerInput;
    public PlayerMovement playerMovement;

    public GameEvent velocityZero;

    public Rigidbody2D rigidBody;
    public Vector2 velocity;

    float moveSpeed;
    float maxSpeed;
    float jumpForce;
    float jumpDuration;
    float jumpCost;

    [Range(-1, 1)] public float direction;

    // Start is called before the first frame update
    void Start()
    {

        //rigidBody = playerMovement.rigidBody;
    }

    // Update is called once per frame
    void Update()
    {
        //////Move these to Start(), in here now for playtesting and balance tweaks
        moveSpeed = playerMovement.moveSpeed;
        jumpForce = playerMovement.jumpForce;
        //////Move these to Start(), in here now for playtesting and balance tweaks

        direction = playerInput.leftStickValue;
        if (direction < 0)
        {
            playerMovement.direction = -1;
        }
        if (direction > 0)
        {
            playerMovement.direction = 1;
        }

        velocity = rigidBody.velocity;
        playerMovement.playerVelocity = velocity;

        if (velocity == Vector2.zero)
        {
            velocityZero.Raise();
        }
    }

    public void Move()
    {
        velocity.x = moveSpeed * playerInput.leftStickValue;
        rigidBody.velocity = velocity;
    }

    public void Jump()
    {
        velocity.y = jumpForce;
        rigidBody.velocity = velocity;
        //rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }
    //private void FixedUpdate()
    //{

    //    Vector2 velocity = rigidBody.velocity;
    //    velocity.x += playerMovement.moveSpeed * direction;
    //    rigidBody.velocity = velocity;        
    //}
}
