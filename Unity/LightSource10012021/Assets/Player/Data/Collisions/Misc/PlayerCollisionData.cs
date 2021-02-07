using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Collision Data", menuName = "PlayerData/Collision")]
public class PlayerCollisionData: ScriptableObject
{
    public float hurtDuration;
    public float invincibilityDuration;
    public NPCStats collisionStats;
}
