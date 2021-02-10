using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Weapons/Type")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public int durability;
    public int damage;

    public Vector2 throwForce;
    public Quaternion throwRotation;
}