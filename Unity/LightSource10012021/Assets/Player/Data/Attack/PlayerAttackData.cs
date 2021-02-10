using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Attack Data", menuName = "PlayerData/Attack")]
public class PlayerAttackData : ScriptableObject
{
    public Vector2 throwForce;
}