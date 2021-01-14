using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatesScript : MonoBehaviour
{
    public PlayerStates playerStatesData;

    //bool isStill;
    //bool isGrounded;
    //bool isJumping;
    //bool isAirborne;
    //bool isFalling;
    //bool isHurt;
    //bool isAttacking;
    //bool isParrying;
    //bool isFloating;
    //bool isDashing;
    //bool inDialogue;

    private void FixedUpdate()
    {
        //playerStatesData.isStill = isStill;
        //playerStatesData.isGrounded = isGrounded;
        //playerStatesData.isJumping = isJumping;
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

}
