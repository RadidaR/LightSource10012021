using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stamina", menuName = "PlayerData/PlayerStamina")]
public class PlayerStamina: ScriptableObject
{
    public float maxStamina;
    public float currentStamina;
    public float currentRecoveryRate;
    public float stillRecoveryRate;
    public float motionRecoveryRate;
    [Range(0, 1)] public float almostFullModifier;
    public float meditateModifier;
    public float jumpCost;
    public float attackCost;
    public float floatCost;
    public float abilityCost;
    public float dashCost;
}
