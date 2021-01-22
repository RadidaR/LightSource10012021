using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashingScript : MonoBehaviour
{
    public PlayerDashing playerDashingData;
    public PlayerMovement playerMovementData;
    public PlayerStates playerStatesData;
    public PlayerStamina playerStaminaData;

    public GameEvent eDashStarted;
    public GameEvent eDashEnded;

    public GameEvent eUseStamina;
    public GameEvent eInsufficientStamina;

    public Rigidbody2D rigidBody;

    public float duration;
    public float cooldown;

    private void Start()
    {
        duration = playerDashingData.dashDuration;
        cooldown = 0;
    }

    private void FixedUpdate()
    {
        //COUNTS DOWN COOLDOWN
        if (cooldown > 0)
        {
            cooldown -= Time.fixedDeltaTime;
        }
        //SETS COOLDOWN TO 0
        if (cooldown < 0)
        {
            cooldown = 0;
        }
    }

    public void Dash()
    {
        //IF COOLDOWN IS 0
        if (cooldown == 0)
        {
            //AND DASH DURATION IS MORE THAN 0
            if (duration > 0)
            {
                //TICK AWAY DASH DURATION
                duration -= Time.fixedDeltaTime;
                //IF HAS ENOUGH STAMINA
                if (playerStaminaData.currentStamina > playerDashingData.dashCost)
                {
                    //AND ISN'T DASHING (SO IT'S DRAINED ONLY ONCE)
                    if (!playerStatesData.isDashing)
                    {
                        //DRAIN STAMINA
                        playerStaminaData.staminaCost = playerDashingData.dashCost;
                        eUseStamina.Raise();
                        //AND START DASH
                        eDashStarted.Raise();
                    }
                    Vector2 velocity = rigidBody.velocity;
                    //LOCK POSITION IN AIR
                    rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    //DASH
                    velocity.x = playerDashingData.dashSpeed * playerMovementData.facingDirection;
                    rigidBody.velocity = velocity;
                    return;
                }
                //IF NOT ENOUGH STAMINA
                else
                {
                    eInsufficientStamina.Raise();
                }
            }
            //IF DURATION ENDED
            else
            {
                eDashEnded.Raise();
                EndDash();
            }
        }

    }

    public void EndDash()
    {
        //UNLOCK POSITION ON Y AXIS
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        //SET COOLDOWN
        if (cooldown == 0)
        {
            cooldown = playerDashingData.dashCooldown;
        }
        //IF PLAYER IS DASHING
        if (playerStatesData.isDashing)
        {
            //STOP MOTION
            Vector2 velocity = rigidBody.velocity;
            velocity.x = 0;
            rigidBody.velocity = velocity;
        }
        //RESET DURATION
        duration = playerDashingData.dashDuration;
        eDashEnded.Raise();
    }
}
