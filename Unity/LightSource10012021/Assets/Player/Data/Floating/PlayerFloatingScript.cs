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
    public GameEvent eInsufficientStamina;
    public GameEvent eStopRecovery;

    public Rigidbody2D rigidBody;
    
    float baseGravity;

    private void Start()
    {
        //ASIGN BASE GRAVITY
        baseGravity = rigidBody.gravityScale;
    }

    private void Update()
    {
        //CALCULATE STABILIZATION TIMES
        playerFloatingData.mStabilizationTime = 1 / playerFloatingData.moveStabilization;
        playerFloatingData.rStabilizationTime = 1 / playerFloatingData.riseStabilization;
        playerFloatingData.fStabilizationTime = 1 / playerFloatingData.fallStabilization;
    }

    public void Float()
    {
        //IF HAS ENOUGH STAMINA
        if (playerStaminaData.currentStamina > playerFloatingData.floatCost)
        {
            //IF AIRBORNE
            if (playerStatesData.isAirborne)
            {
                //START FLOATING
                eFloatStarted.Raise();
                rigidBody.gravityScale = playerFloatingData.floatGravity;
            }

            //IF FLOATING
            if (playerStatesData.isFloating)
            {
                //DRAIN STAMINA
                playerStaminaData.staminaCost = playerFloatingData.floatCost * Time.fixedDeltaTime;
                eUseStamina.Raise();
                //IF DESCENDING
                if (playerMovementData.playerVelocity.y < 0)
                {
                    Vector2 slowDown = rigidBody.velocity;
                    //IF FALLING AT HIGH SPEED
                    if (playerStatesData.isFalling)
                    {
                        //DECREASE FALLING SPEED TO 0 IN FALL STABILIZATION TIME
                        slowDown.y -= slowDown.y * playerFloatingData.fallStabilization * Time.fixedDeltaTime;
                        rigidBody.velocity = slowDown;
                        return;
                    }
                    //ELSE
                    else
                    {
                        //DECREASE FALLING SPEED TO 0 IN 0.1 SECONDS
                        slowDown.y -= slowDown.y * 10 * Time.fixedDeltaTime;
                        rigidBody.velocity = slowDown;
                        rigidBody.velocity = new Vector2(rigidBody.velocity.x, playerFloatingData.floatForce);
                    }
                    return;
                }

                //IF ASCENDING
                if (rigidBody.velocity.y > 0)
                {
                    //AND RISING SPEED IS BIGGER THAN FLOATING SPEED OF ASCENTION
                    if (rigidBody.velocity.y > playerFloatingData.floatForce)
                    {
                        //DECCELERATE RISING SPEED OT FLOATING SPEED OF ASCENTION IN RISE STABILIZATION TIME
                        Vector2 slowDown = rigidBody.velocity;
                        slowDown.y -= slowDown.y * playerFloatingData.riseStabilization * Time.fixedDeltaTime;
                        rigidBody.velocity = slowDown;
                        //AND GO BACK
                        return;
                    }
                    //OR RISE BY FLOATING SPEED OF ASCENTION
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, playerFloatingData.floatForce);
                }
                //STOP STAMINA FROM RECOVERING
                eStopRecovery.Raise();
            }
        }
        //IF NOT ENOUGH STAMINA
        else
        {
            eInsufficientStamina.Raise();
            EndFloat();
        }
    }

    public void FloatMove()
    {
        //IF MOVING SPEED IS BIGGER THAN FLOATING MOVING SPEED
        if (Mathf.Abs(playerMovementData.playerVelocity.x) > playerFloatingData.floatSpeed)
        {
            //IF MOVING & LOOKING RIGHT
            if (playerMovementData.playerVelocity.x > 0 && playerMovementData.facingDirection == 1)
            {
                //GRADUALLY DECREASE SPEED TO MATCH FLOATING SPEED
                Vector2 slowDown = rigidBody.velocity;
                slowDown.x -= slowDown.x * playerFloatingData.moveStabilization * Time.fixedDeltaTime;
                rigidBody.velocity = slowDown;
                return;
            }
            //IF MOVING & LOOKING LEFT
            if (playerMovementData.playerVelocity.x < 0 && playerMovementData.facingDirection == -1)
            {
                //GRADUALLY DECREASE SPEED TO MATCH FLOATING SPEED
                Vector2 slowDown = rigidBody.velocity;
                slowDown.x -= slowDown.x * playerFloatingData.moveStabilization * Time.fixedDeltaTime;
                rigidBody.velocity = slowDown;
                return;
            }
        }

        //MOVE IN FACING DIRECTION
        Vector2 floatMove = rigidBody.velocity;
        floatMove.x = playerFloatingData.floatSpeed * playerMovementData.facingDirection;
        rigidBody.velocity = floatMove;
    }

    public void EndFloat()
    {
        //RESET GRAVITY
        eFloatEnded.Raise();
        rigidBody.gravityScale = baseGravity;
    }

}
