using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloatingScript : MonoBehaviour
{
    public PlayerFloating playerFloatingData;
    public PlayerStates playerStatesData;
    public PlayerMovement playerMovementData;
    public PlayerStamina playerStaminaData;

    public GameEvent eFloatStarted;
    public GameEvent eFloatEnded;
    public GameEvent eStabilizingFall;

    public GameEvent eUseStamina;
    public GameEvent eStopRecovery;

    public Rigidbody2D rigidBody;

    public float afterDashCD;
    float baseGravity;

    private void Start()
    {
        baseGravity = rigidBody.gravityScale;
        afterDashCD = 0.5f;
    }

    private void Update()
    {
        playerFloatingData.mStabilizationTime = 1 / playerFloatingData.moveStabilization;
        playerFloatingData.rStabilizationTime = 1 / playerFloatingData.riseStabilization;
        playerFloatingData.fStabilizationTime = 1 / playerFloatingData.fallStabilization;
    }

    private void FixedUpdate()
    {
        if (playerStatesData.isDashing)
        {
            afterDashCD = 0f;
        }
        else
        {
            if (afterDashCD < 0.5f)
            {
                afterDashCD += Time.fixedDeltaTime;
            }
            else
            {
                afterDashCD = 0.5f;
            }
        }
    }

    public void Float()
    {
        if (afterDashCD == 0.5f && playerStaminaData.currentStamina > playerFloatingData.floatCost)
        {
            if (playerStatesData.isAirborne)
            {
                eFloatStarted.Raise();
                rigidBody.gravityScale = playerFloatingData.floatGravity;
            }

            if (playerStatesData.isFloating)
            {
                playerStaminaData.staminaCost = playerFloatingData.floatCost * Time.fixedDeltaTime;
                eUseStamina.Raise();
                if (playerMovementData.playerVelocity.y < 0)
                {
                    Vector2 slowDown = rigidBody.velocity;
                    if (playerStatesData.isFalling)
                    {
                        slowDown.y -= slowDown.y * playerFloatingData.fallStabilization * Time.fixedDeltaTime;
                        rigidBody.velocity = slowDown;
                        return;
                    }
                    else
                    {
                        slowDown.y -= slowDown.y * 10 * Time.fixedDeltaTime;
                        rigidBody.velocity = slowDown;
                        rigidBody.velocity = new Vector2(rigidBody.velocity.x, playerFloatingData.floatForce);
                    }
                    return;
                }

                if (rigidBody.velocity.y > 0)
                {
                    if (rigidBody.velocity.y > playerFloatingData.floatForce)
                    {
                        Vector2 slowDown = rigidBody.velocity;
                        slowDown.y -= slowDown.y * playerFloatingData.riseStabilization * Time.fixedDeltaTime;
                        rigidBody.velocity = slowDown;
                        return;
                    }
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, playerFloatingData.floatForce);
                }
                eStopRecovery.Raise();
            }
        }
        else
        {
            EndFloat();
        }
    }

    public void FloatMove()
    {
        if (Mathf.Abs(playerMovementData.playerVelocity.x) > playerFloatingData.floatSpeed)
        {
            if (playerMovementData.playerVelocity.x > 0 && playerMovementData.facingDirection == 1)
            {
                Vector2 slowDown = rigidBody.velocity;
                slowDown.x -= slowDown.x * playerFloatingData.moveStabilization * Time.fixedDeltaTime;
                rigidBody.velocity = slowDown;
                return;
            }
            if (playerMovementData.playerVelocity.x < 0 && playerMovementData.facingDirection == -1)
            {
                Vector2 slowDown = rigidBody.velocity;
                slowDown.x -= slowDown.x * playerFloatingData.moveStabilization * Time.fixedDeltaTime;
                rigidBody.velocity = slowDown;
                return;
            }
        }

        Vector2 floatMove = rigidBody.velocity;
        floatMove.x = playerFloatingData.floatSpeed * playerMovementData.facingDirection;
        rigidBody.velocity = floatMove;
    }

    public void EndFloat()
    {
        eFloatEnded.Raise();
        rigidBody.gravityScale = baseGravity;
    }

}
