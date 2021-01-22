using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    ActionMap actionMap;

    public PlayerInput playerInputData;


    public GameEvent eLeftPressed;
    //public GameEvent eLeftReleased;
    public GameEvent eRightPressed;
    //public GameEvent eRightReleased;
    public GameEvent eJumpPressed;
    public GameEvent eJumpReleased;
    public GameEvent eDashPressed;
    public GameEvent eDashReleased;

    private void Awake()
    {
        actionMap = new ActionMap();

        actionMap.Gameplay.Move.performed += ctx => playerInputData.leftStickValue = actionMap.Gameplay.Move.ReadValue<float>();
        //actionMap.Gameplay.Move.canceled += ctx => RaiseMoveReleased();
        actionMap.Gameplay.Move.canceled += ctx => playerInputData.leftStickValue = 0;

        actionMap.Gameplay.Aim.performed += ctx => playerInputData.rightStickValue = actionMap.Gameplay.Aim.ReadValue<Vector2>();
        //actionMap.Gameplay.Aim.canceled += ctx => aim = new Vector2(movement.direction, 0);
        actionMap.Gameplay.Aim.canceled += ctx => playerInputData.rightStickValue = Vector2.zero;

        actionMap.Gameplay.Jump.performed += ctx => playerInputData.buttonSouth = actionMap.Gameplay.Jump.ReadValue<float>();
        actionMap.Gameplay.Jump.canceled += ctx => playerInputData.buttonSouth = 0;
        actionMap.Gameplay.Jump.canceled += ctx => eJumpReleased.Raise();

        actionMap.Gameplay.Parry.performed += ctx => playerInputData.buttonEast = actionMap.Gameplay.Parry.ReadValue<float>();
        actionMap.Gameplay.Parry.canceled += ctx => playerInputData.buttonEast = 0;

        actionMap.Gameplay.Attack.performed += ctx => playerInputData.buttonWest = actionMap.Gameplay.Attack.ReadValue<float>();
        actionMap.Gameplay.Attack.canceled += ctx => playerInputData.buttonWest = 0;

        actionMap.Gameplay.Interact.performed += ctx => playerInputData.buttonNorth = actionMap.Gameplay.Interact.ReadValue<float>();
        actionMap.Gameplay.Interact.canceled += ctx => playerInputData.buttonNorth = 0;

        actionMap.Gameplay.Dash.performed += ctx => playerInputData.leftBumper = actionMap.Gameplay.Dash.ReadValue<float>();
        actionMap.Gameplay.Dash.canceled += ctx => RaiseDashReleased();
        actionMap.Gameplay.Dash.canceled += ctx => playerInputData.leftBumper = 0;

        actionMap.Gameplay.AbilityWheel.performed += ctx => playerInputData.rightBumper = actionMap.Gameplay.AbilityWheel.ReadValue<float>();
        actionMap.Gameplay.AbilityWheel.canceled += ctx => playerInputData.rightBumper = 0;

        actionMap.Gameplay.Throw.performed += ctx => playerInputData.leftTrigger = actionMap.Gameplay.Throw.ReadValue<float>();
        actionMap.Gameplay.Throw.canceled += ctx => playerInputData.leftTrigger = 0;

        actionMap.Gameplay.ChargeAbility.performed += ctx => playerInputData.rightTrigger = actionMap.Gameplay.ChargeAbility.ReadValue<float>();
        actionMap.Gameplay.ChargeAbility.canceled += ctx => playerInputData.rightTrigger = 0;
    }

    private void OnEnable()
    {
        actionMap.Enable();
    }

    private void OnDisable()
    {
        actionMap.Disable();
    }
    
    void FixedUpdate()
    {

        if (playerInputData.leftStickValue != 0)
        {
            RaiseMovePressed();
        }

        if (playerInputData.buttonSouth != 0)
        {
            RaiseJumpPressed();
        }

        if (playerInputData.leftBumper != 0)
        {
            RaiseDashPressed();
        }
    }

    private void RaiseMovePressed()
    {
        if (playerInputData.leftStickValue < 0)
        {
            eLeftPressed.Raise();
        }
        if (playerInputData.leftStickValue > 0)
        {
            eRightPressed.Raise();
        }
    }

    //private void RaiseMoveReleased()
    //{
    //    if (playerInputData.leftStickValue > 0)
    //    {
    //        eRightReleased.Raise();
    //    }
    //    if (playerInputData.leftStickValue < 0)
    //    {
    //        eLeftReleased.Raise();
    //    }
    //}

    private void RaiseJumpPressed()
    {
        eJumpPressed.Raise();
    }

    private void RaiseJumpReleased()
    {
        eJumpReleased.Raise();
    }

    public void ResetJump()
    {
        playerInputData.buttonSouth = 0;
    }

    private void RaiseDashPressed()
    {
        eDashPressed.Raise();
    }

    private void RaiseDashReleased()
    {
        eDashReleased.Raise();
    }
}
