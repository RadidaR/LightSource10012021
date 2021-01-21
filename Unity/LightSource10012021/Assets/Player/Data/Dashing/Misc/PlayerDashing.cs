using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Dashing Data", menuName = "PlayerData/PlayerDashing")]
public class PlayerDashing : ScriptableObject
{
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;
    public float dashCost;
}
