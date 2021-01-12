using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//////////IF VALUE WILL BE MODIFIED EXTERNALLY AT RUNTIME, USE THIS//////////
[CreateAssetMenu(fileName = "Player Input", menuName = "PlayerData/PlayerInput")]
public class PlayerInput : ScriptableObject, ISerializationCallbackReceiver
{
    int reset = 0;

    //[NonSerialized]
    [Range(-1, 1)] public float leftStickValue;
    public Vector2 rightStickValue;
    [Range(0, 1)] public float buttonSouth;
    [Range(0, 1)] public float buttonEast;
    [Range(0, 1)] public float buttonWest;
    [Range(0, 1)] public float buttonNorth;
    [Range(0, 1)] public float leftBumper;
    [Range(0, 1)] public float rightBumper;
    [Range(0, 1)] public float leftTrigger;
    [Range(0, 1)] public float rightTrigger;

    public void OnAfterDeserialize()
    {
        leftStickValue = reset;
        rightStickValue.x = reset;
        rightStickValue.y = reset;
        buttonSouth = reset;
        buttonEast = reset;
        buttonWest = reset;
        buttonNorth = reset;
        leftBumper = reset;
        rightBumper = reset;
        leftTrigger = reset;
        rightTrigger = reset;
    }

    public void OnBeforeSerialize()
    {

    }
}
