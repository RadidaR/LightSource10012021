using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Floating Data", menuName = "PlayerData/Floating")]
public class Floating : ScriptableObject
{
    public float floatSpeed;
    public float floatForce;
    public float floatGravity;
    public float fallStabilization;
    public float floatCost;
}
