using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingScript : MonoBehaviour
{
    public Floating floatingData;
    public PlayerStates playerStatesData;
    public PlayerMovement playerMovementData;

    public GameEvent eFloatStarted;
    public GameEvent eFloatEnded;
    public GameEvent eStabilizingFall;

    public Rigidbody2D rigidBody;

    float baseGravity;
    float floatGravity;

    float floatSpeed;
    float floatForce;
    float fallStabilization;
    float floatCost;

    private void Start()
    {
        baseGravity = rigidBody.gravityScale;
    }

    private void Update()
    {
        fallStabilization = floatingData.fallStabilization;
    }

    public void Float()
    {
        if (!playerStatesData.isGrounded && !playerStatesData.isJumping)
        {
            //if (rigidBody.velocity.y < 0)
            //{
            eFloatStarted.Raise();
            if (rigidBody.velocity.y < 3)
            {
                rigidBody.gravityScale = 0f;
                Vector2 slowDown = rigidBody.velocity;
                slowDown.y -= slowDown.y * fallStabilization * Time.fixedDeltaTime;
                rigidBody.velocity = slowDown;
                return;
            }

        }
    }

    public void EndFloat()
    {
        eFloatEnded.Raise();
        rigidBody.gravityScale = baseGravity;
    }

}
