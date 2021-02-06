using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Collision", menuName = "PlayerData/PlayerCollision")]
public class PlayerCollision: ScriptableObject
{
    public float hurtDuration;
    public float invincibilityDuration;
    public NPCStats collisionStats;
}
