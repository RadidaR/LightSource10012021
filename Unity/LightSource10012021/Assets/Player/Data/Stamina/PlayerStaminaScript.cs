using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStaminaScript : MonoBehaviour
{
    public PlayerStamina playerStaminaData;

    void Update()
    {
        if (playerStaminaData.currentStamina < playerStaminaData.maxStamina)
        {
            if (playerStaminaData.currentStamina < playerStaminaData.maxStamina * playerStaminaData.pantingThreshold)
            {
                playerStaminaData.currentStamina += playerStaminaData.recoveryRate * Time.deltaTime;
            }
            else
            {
                playerStaminaData.currentStamina += playerStaminaData.recoveryRate * playerStaminaData.pantingModifier * Time.deltaTime;
            }
        }

        if (playerStaminaData.currentStamina > playerStaminaData.maxStamina)
        {
            playerStaminaData.currentStamina = playerStaminaData.maxStamina;
        }

        if (playerStaminaData.currentStamina < 0)
        {
            playerStaminaData.currentStamina = 0;
        }

    }

    public void UseStamina()
    {
        playerStaminaData.currentStamina -= playerStaminaData.staminaCost;
    }

    public void StillRecovery()
    {
        playerStaminaData.recoveryRate = playerStaminaData.stillRecovery;
    }

    public void MotionRecovery()
    {
        playerStaminaData.recoveryRate = playerStaminaData.motionRecovery;
    }

    public void NoRecovery()
    {
        playerStaminaData.recoveryRate = 0f;
    }
}
