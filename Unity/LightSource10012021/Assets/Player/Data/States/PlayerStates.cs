using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//////////IF VALUE WILL BE MODIFIED EXTERNALLY AT RUNTIME, USE THIS//////////
[CreateAssetMenu(fileName = "Player States", menuName = "PlayerData/PlayerStates")]
public class PlayerStates: ScriptableObject, ISerializationCallbackReceiver
{
    bool reset = false;

    public bool isStill;
    public bool isGrounded;
    public bool isJumping;
    public bool isAirborne;
    public bool isFalling;
    public bool isHurt;
    public bool isAttacking;
    public bool isParrying;
    public bool isFloating;
    public bool isDashing;
    public bool inDialogue;

    public void OnAfterDeserialize()
    {
        isStill = reset;
        isGrounded = reset;
        isJumping = reset;
        isAirborne = reset;
        isFalling = reset;
        isHurt = reset;
        isAttacking = reset;
        isParrying = reset;
        isFloating = reset;
        isDashing = reset;
        inDialogue = reset;
    }

    public void OnBeforeSerialize()
    {

    }
}