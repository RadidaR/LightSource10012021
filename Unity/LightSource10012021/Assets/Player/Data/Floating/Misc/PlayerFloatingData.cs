using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Floating Data", menuName = "PlayerData/Floating")]
public class PlayerFloatingData : ScriptableObject
{
    public float floatSpeed;
    public float floatForce;
    public float floatGravity;
    public float moveStabilization;
    public float mStabilizationTime;
    public float riseStabilization;
    public float rStabilizationTime;
    public float fallStabilization;
    public float fStabilizationTime;
    public float floatCost;
}
