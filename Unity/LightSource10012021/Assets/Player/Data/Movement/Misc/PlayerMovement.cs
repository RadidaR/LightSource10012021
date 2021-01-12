using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Movement", menuName = "PlayerData/PlayerMovement")]
public class PlayerMovement : ScriptableObject
{
    [Range(-1, 1)] public int direction;
    public float moveSpeed;
    public float maxSpeed;
    public float jumpForce;
    public float jumpDuration;
    public float jumpCost;
    public Vector2 playerVelocity;
}