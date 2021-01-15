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

    float baseGravity;

    private void Start()
    {
        baseGravity = rigidBody.gravityScale;
    }

    private void Update()
    {
        playerFloatingData.mStabilizationTime = 1 / playerFloatingData.moveStabilization;
        playerFloatingData.rStabilizationTime = 1 / playerFloatingData.riseStabilization;
        playerFloatingData.fStabilizationTime = 1 / playerFloatingData.fallStabilization;

    }

    public void Float()
    {
        if (playerStatesData.isAirborne)
        {
            eFloatStarted.Raise();
            rigidBody.gravityScale = playerFloatingData.floatGravity;
        }

        if (playerStatesData.isFloating)
        {
            playerStaminaData.staminaCost = playerFloatingData.floatCost * Time.fixedDeltaTime;
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
            eUseStamina.Raise();
        }
    }

    public void FloatMove()
    {
        if (Mathf.Abs(playerMovementData.playerVelocity.x) > playerFloatingData.floatSpeed)
        {
            Vector2 slowDown = rigidBody.velocity;
            slowDown.x -= slowDown.x * playerFloatingData.moveStabilization * Time.fixedDeltaTime;
            rigidBody.velocity = slowDown;
            return;
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
