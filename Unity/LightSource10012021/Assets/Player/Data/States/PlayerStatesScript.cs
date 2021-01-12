using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatesScript : MonoBehaviour
{
    public PlayerStates playerStates;

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
        playerStates.isStill = isStill;
        playerStates.isJumping = isJumping;
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



}
