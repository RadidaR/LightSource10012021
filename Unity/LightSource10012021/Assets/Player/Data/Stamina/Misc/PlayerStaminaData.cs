using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stamina Data", menuName = "PlayerData/Stamina")]
public class PlayerStaminaData: ScriptableObject
{
    public float maxStamina;
    public float currentStamina;
    public float staminaCost;
    [Range(0,1)] public float pantingThreshold;
    public float recoveryRate;
    public float stillRecovery;
    public float motionRecovery;
    [Range(0,1)] public float pantingModifier;
    public float meditatingModifier;
}
