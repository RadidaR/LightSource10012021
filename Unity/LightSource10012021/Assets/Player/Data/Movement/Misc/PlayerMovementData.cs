using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Movement Data", menuName = "PlayerData/Movement")]
public class PlayerMovementData : ScriptableObject
{
    [Range(-1, 1)] public int facingDirection;
    public float moveSpeed;
    public float maxSpeed;
    public float accelerationRate;
    public float jumpForce;
    public float jumpDuration;
    public float jumpCost;
    public Vector2 playerVelocity;
}