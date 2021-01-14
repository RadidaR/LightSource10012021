using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerStaminaScript : MonoBehaviour
{
    //Take data from Scriptable Object
    public PlayerStamina playerStaminaData;
    public float maxStamina;
    public float currentStamina;
    float recoveryRate;
    float pantingThreshold;
    float pantingModifier;

    //public float stillRecoveryRate;
    //public float motionRecoveryRate;

    //public float almostFullModifier;
    //public float meditateModifier;

    //public float jumpCost;
    //public float attackCost;
    //public float floatCost;
    //public float abilityCost;
    //public float dashCost;

    private float staminaCost;

    // Start is called before the first frame update
    void Start()
    {

        maxStamina = playerStaminaData.maxStamina;
        currentStamina = playerStaminaData.currentStamina;

        pantingThreshold = playerStaminaData.pantingThreshold;
        pantingModifier = playerStaminaData.pantingModifier;
        //currentRecoveryRate = playerStaminaData.currentRecoveryRate;

        //stillRecoveryRate = playerStaminaData.stillRecoveryRate;
        //motionRecoveryRate = playerStaminaData.motionRecoveryRate;

        //almostFullModifier = playerStaminaData.almostFullModifier;
        //meditateModifier = playerStaminaData.meditateModifier;

        //jumpCost = playerStaminaData.jumpCost;
        //attackCost = playerStaminaData.attackCost;
        //floatCost = playerStaminaData.floatCost;
        //abilityCost = playerStaminaData.abilityCost;
        //dashCost = playerStaminaData.dashCost;
    }

    // Update is called once per frame
    void Update()
    {
        staminaCost = playerStaminaData.staminaCost;
        recoveryRate = playerStaminaData.recoveryRate;

        if (currentStamina < maxStamina)
        {
            if (currentStamina < maxStamina * pantingThreshold)
            {
                currentStamina += recoveryRate * Time.deltaTime;
            }
            else
            {
                currentStamina += recoveryRate * pantingModifier * Time.deltaTime;
            }
        }

        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }

        if (currentStamina < 0)
        {
            currentStamina = 0;
        }

        playerStaminaData.currentStamina = currentStamina;

    }

    public void UseStamina()
    {
        currentStamina -= staminaCost;
    }

    public void StillRecovery()
    {
        playerStaminaData.recoveryRate = playerStaminaData.stillRecovery;
    }

    public void MotionRecovery()
    {
        playerStaminaData.recoveryRate = playerStaminaData.motionRecovery;
    }
}
