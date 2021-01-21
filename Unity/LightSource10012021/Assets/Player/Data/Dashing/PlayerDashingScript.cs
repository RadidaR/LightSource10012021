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
        if (cooldown > 0)
        {
            cooldown -= Time.fixedDeltaTime;
        }
        if (cooldown < 0)
        {
            cooldown = 0;
        }
        //Vector2 velocity = rigidBody.velocity;
        //if (playerStatesData.isDashing)
        //{
        //    duration = playerDashingData.dashDuration;
        //    if (duration > 0f)
        //    {
        //        duration -= Time.fixedDeltaTime;
        //        Debug.Log(duration.ToString());
        //        rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        //        velocity.x = playerDashingData.dashSpeed * playerMovementData.facingDirection;
        //        rigidBody.velocity = velocity;
        //        return;
        //    }
        //    else
        //    {
        //        eDashEnded.Raise();
        //    }
        //}
        //else
        //{
        //    velocity.x = 0f;
        //    rigidBody.velocity = velocity;
        //    rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        //}
    }

    public void Dash()
    {
        if (playerStaminaData.currentStamina > playerDashingData.dashCost)
        {
            if (cooldown == 0)
            {
                if (duration > 0)
                {
                    if (!playerStatesData.isDashing)
                    {
                        playerStaminaData.staminaCost = playerDashingData.dashCost;
                        eUseStamina.Raise();
                    }
                    eDashStarted.Raise();
                    duration -= Time.fixedDeltaTime;
                    Vector2 velocity = rigidBody.velocity;
                    rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    velocity.x = playerDashingData.dashSpeed * playerMovementData.facingDirection;
                    rigidBody.velocity = velocity;
                    return;
                }
                else
                {
                    eDashEnded.Raise();
                    EndDash();
                }
            }
        }
        else
        {
            eInsufficientStamina.Raise();
        }
    }

    public void EndDash()
    {
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (cooldown == 0)
        {
            cooldown = playerDashingData.dashCooldown;
        }

        if (playerStatesData.isDashing)
        {
            Vector2 velocity = rigidBody.velocity;
            velocity.x = 0;
            rigidBody.velocity = velocity;
        }

        duration = playerDashingData.dashDuration;
        eDashEnded.Raise();
    }
}
