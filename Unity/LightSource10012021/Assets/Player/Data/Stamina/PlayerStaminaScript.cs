using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStaminaScript : MonoBehaviour
{
    [Header("Data Types")]
    public PlayerStaminaData playerStaminaData;

    void Update()
    {
        //IF STAMINA ISN'T FULL
        if (playerStaminaData.currentStamina < playerStaminaData.maxStamina)
        {
            //IF STAMINA IS BENEATH PANTING THRESHOLD
            if (playerStaminaData.currentStamina < playerStaminaData.maxStamina * playerStaminaData.pantingThreshold)
            {
                //RECOVER STAMINA FULLY
                playerStaminaData.currentStamina += playerStaminaData.recoveryRate * Time.deltaTime;
            }
            //ELSE
            else
            {
                //APPLY PANTING MODIFIER TO RECOVERY
                playerStaminaData.currentStamina += playerStaminaData.recoveryRate * playerStaminaData.pantingModifier * Time.deltaTime;
            }
        }

        //PREVENT STAMINA FROM EXCEEDING MAX STAMINA
        if (playerStaminaData.currentStamina > playerStaminaData.maxStamina)
        {
            playerStaminaData.currentStamina = playerStaminaData.maxStamina;
        }

        //PREVENT STAMINA FROM BEING DRAINED BELOW 0
        if (playerStaminaData.currentStamina < 0)
        {
            playerStaminaData.currentStamina = 0;
        }

    }

    public void UseStamina()
    {
        //DRAIN STAMINA
        playerStaminaData.currentStamina -= playerStaminaData.staminaCost;
    }

    public void StillRecovery()
    {
        //SET RECOVERY TO STIL RECOVERY
        playerStaminaData.recoveryRate = playerStaminaData.stillRecovery;
    }

    public void MotionRecovery()
    {
        //SET RECOVERY TO MOTION RECOVERY
        playerStaminaData.recoveryRate = playerStaminaData.motionRecovery;
    }

    public void NoRecovery()
    {
        //STOP RECOVERY
        playerStaminaData.recoveryRate = 0f;
    }
}
