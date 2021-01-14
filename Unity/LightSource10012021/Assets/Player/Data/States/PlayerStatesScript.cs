using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatesScript : MonoBehaviour
{
    public PlayerStates playerStatesData;

    bool isStill;
    bool isGrounded;
    bool isJumping;
    bool isAirborne;
    bool isFalling;
    bool isHurt;
    bool isAttacking;
    bool isParrying;
    bool isFloating;
    bool isDashing;
    bool inDialogue;

    private void FixedUpdate()
    {
        playerStatesData.isStill = isStill;
        playerStatesData.isGrounded = isGrounded;
        playerStatesData.isJumping = isJumping;
    }

    public void Still()
    {
        isStill = true;
    }

    public void Moving()
    {
        isStill = false;
    }

    public void Jumping()
    {
        isJumping = true;
    }

    public void StopJumping()
    {
        isJumping = false;
    }

    public void Grounded()
    {
        isGrounded = true;
    }

    public void NotGrounded()
    {
        isGrounded = false;
    }

}
