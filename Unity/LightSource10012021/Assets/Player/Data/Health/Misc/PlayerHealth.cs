using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Health", menuName = "PlayerData/PlayerHealth")]
public class PlayerHealth : ScriptableObject
{
    public float maxHealth;
    public float currentHealth;

    [Range(0, 1)] public float criticalThreshold;
    public float healthRegained;
    public float healthLost;

    public float hurtDuration;
}
