using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatesScript : MonoBehaviour
{
    public PlayerStates playerStatesData;
    public PlayerMovement playerMovementData;
    

    private void FixedUpdate()
    {

        if (!playerStatesData.isGrounded && !playerStatesData.isJumping && !playerStatesData.isFloating && !playerStatesData.isDashing)
        {
            Airborne();
        }
        else
        {
            NotAirborne();
        }


        if (playerMovementData.playerVelocity.y < -80f)
        {
            Falling();
        }

        if (playerStatesData.isFalling)
        {
            if (playerMovementData.playerVelocity.y > -20f)
            {
                NotFalling();
            }
        }
        

        if (playerStatesData.isFloating || playerStatesData.isDashing)
        {
            NotGrounded();
            StopJumping();
        }
    }

    public void Still()
    {
        playerStatesData.isStill = true;
    }

    public void Moving()
    {
        playerStatesData.isStill = false;
    }

    public void Jumping()
    {
        playerStatesData.isJumping = true;
    }

    public void StopJumping()
    {
        playerStatesData.isJumping = false;
    }

    public void Grounded()
    {
        playerStatesData.isGrounded = true;
    }

    public void NotGrounded()
    {
        playerStatesData.isGrounded = false;
    }

    public void Floating()
    {
        playerStatesData.isFloating = true;
    }

    public void StopFloating()
    {
        playerStatesData.isFloating = false;
    }

    public void Airborne()
    {
        playerStatesData.isAirborne = true;
    }

    public void NotAirborne()
    {
        playerStatesData.isAirborne = false;
    }

    public void Falling()
    {
        playerStatesData.isFalling = true;
    }

    public void NotFalling()
    {
        playerStatesData.isFalling = false;
    }

    public void Dashing()
    {
        playerStatesData.isDashing = true;
    }

    public void StopDashing()
    {
        playerStatesData.isDashing = false;
    }

    public void Hurting()
    {
        playerStatesData.isHurt = true;
    }

    public void StopHurting()
    {
        playerStatesData.isHurt = false;
    }

    public void Invincible()
    {
        playerStatesData.isInvincible = true;
    }

    public void NotInvincible()
    {
        playerStatesData.isInvincible = false;
    }

}
