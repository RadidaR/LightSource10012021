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
    public float currentRecoveryRate;

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
        
        currentRecoveryRate = playerStaminaData.stillRecoveryRate;
}

    // Update is called once per frame
    void Update()
    {      
       
        if (currentStamina < maxStamina)
        {
            currentStamina += currentRecoveryRate * Time.deltaTime;
        }

        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }

        playerStaminaData.currentStamina = currentStamina;
    }

    public void DrainStamina()
    {
        currentStamina -= staminaCost;
    }
}
