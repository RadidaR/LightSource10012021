using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    ActionMap actionMap;

    public PlayerInput playerInput;
    float move;
    Vector2 aim;
    float jump;
    float parry;
    float attack;
    float interact;
    float dash;
    float wheel;
    float chuck;
    float charge;

    public GameEvent movePressed;
    public GameEvent jumpPressed;
    public GameEvent jumpReleased;

    //public Movement movement;

    // Start is called before the first frame update

    private void Awake()
    {
        actionMap = new ActionMap();

        actionMap.Gameplay.Move.performed += ctx => move = actionMap.Gameplay.Move.ReadValue<float>();
        actionMap.Gameplay.Move.canceled += ctx => move = 0;

        actionMap.Gameplay.Aim.performed += ctx => aim = actionMap.Gameplay.Aim.ReadValue<Vector2>();
        //actionMap.Gameplay.Aim.canceled += ctx => aim = new Vector2(movement.direction, 0);
        actionMap.Gameplay.Aim.canceled += ctx => aim = Vector2.zero;

        actionMap.Gameplay.Jump.performed += ctx => jump = actionMap.Gameplay.Jump.ReadValue<float>();
        actionMap.Gameplay.Jump.performed += ctx => jumpPressed.Raise();
        actionMap.Gameplay.Jump.canceled += ctx => jumpReleased.Raise();
        actionMap.Gameplay.Jump.canceled += ctx => jump = 0;

        actionMap.Gameplay.Parry.performed += ctx => parry = actionMap.Gameplay.Parry.ReadValue<float>();
        actionMap.Gameplay.Parry.canceled += ctx => parry = 0;

        actionMap.Gameplay.Attack.performed += ctx => attack = actionMap.Gameplay.Attack.ReadValue<float>();
        actionMap.Gameplay.Attack.canceled += ctx => attack = 0;

        actionMap.Gameplay.Interact.performed += ctx => interact = actionMap.Gameplay.Interact.ReadValue<float>();
        actionMap.Gameplay.Interact.canceled += ctx => interact = 0;

        actionMap.Gameplay.Dash.performed += ctx => dash = actionMap.Gameplay.Dash.ReadValue<float>();
        actionMap.Gameplay.Dash.canceled += ctx => dash = 0;

        actionMap.Gameplay.AbilityWheel.performed += ctx => wheel = actionMap.Gameplay.AbilityWheel.ReadValue<float>();
        actionMap.Gameplay.AbilityWheel.canceled += ctx => wheel = 0;

        actionMap.Gameplay.Throw.performed += ctx => chuck = actionMap.Gameplay.Throw.ReadValue<float>();
        actionMap.Gameplay.Throw.canceled += ctx => chuck = 0;

        actionMap.Gameplay.ChargeAbility.performed += ctx => charge = actionMap.Gameplay.ChargeAbility.ReadValue<float>();
        actionMap.Gameplay.ChargeAbility.canceled += ctx => charge = 0;
    }

    private void OnEnable()
    {
        actionMap.Enable();
    }

    private void OnDisable()
    {
        actionMap.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerInput.leftStickValue = move;
        playerInput.rightStickValue = aim;
        playerInput.buttonSouth = jump;
        playerInput.buttonEast = parry;
        playerInput.buttonWest = attack;
        playerInput.buttonNorth = interact;
        playerInput.leftBumper = dash;
        playerInput.rightBumper = wheel;
        playerInput.leftTrigger = chuck;
        playerInput.rightTrigger = charge;

        if (move != 0)
        {
            movePressed.Raise();
        }

        if (jump != 0)
        {
            jumpPressed.Raise();
        }
    }
}
