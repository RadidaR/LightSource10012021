using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{
    public PlayerInputData playerInputData;
    public PlayerStatesData playerStatesData;
    public PlayerMovementData playerMovementData;
    public Collider2D[] interactablesInReach = new Collider2D[2];
    public LayerMask interactableLayers;
    public GameObject parentHand;
    public Transform weaponPosition;
    public GameEvent eGotArmed;
    public GameEvent eDropWeapon;

    public GameObject currentWeapon;

    private void Update()
    {
        if (playerInputData.leftTrigger == 0)
        {
            interactablesInReach = null;
        }
    }
    public void Interact()
    {
        //GATHER INTERACTABLES
        interactablesInReach = Physics2D.OverlapCapsuleAll(gameObject.transform.position, GetComponent<CapsuleCollider2D>().size, CapsuleDirection2D.Horizontal, 0, interactableLayers);

        //AVOID ERROR
        if (interactablesInReach.Length == 0)
        {
            if (playerStatesData.isArmed)
            {
                Debug.Log("Dropped");
                DropWeapon();
            }
            return;
        }
        //IF ONLY ONE INTERACTABLE IN REACH
        else if (interactablesInReach.Length == 1)
        {
            //ASSIGN IT TO A VARIABLE
            GameObject interactable = interactablesInReach[0].gameObject;
            //CHECK IF TAG IS WEAPON
            if (interactable.tag == "Weapon")
            {
                //IF PLAYER IS UNARMED - PICK IT UP
                if (!playerStatesData.isArmed)
                {
                    //PASS VARIABLE TO METHOD
                    PickUpWeapon(interactable);
                }
                //IF PLAYER IS ARMED
                else
                {
                    //DROP CURRENT WEAPON
                    DropWeapon();

                    //AND PICK UP NEW WEAPON
                    PickUpWeapon(interactable);
                }
            }
        }
        //IF TWO INTERACTABLES IN REACH
        else if (interactablesInReach.Length == 2)
        {
            //GET PLAYER AND BOTH INTERACTABLES
            GameObject player = GetComponentInParent<OfInterest>().gameObject;
            GameObject interactable1 = interactablesInReach[0].gameObject;
            GameObject interactable2 = interactablesInReach[1].gameObject;

            //MEASURE DISTANCE BETWEEN PLAYER AND INTERACTABLES
            float distanceTo1 = Vector2.Distance(player.transform.position, interactable1.transform.position);
            float distanceTo2 = Vector2.Distance(player.transform.position, interactable2.transform.position);

            //COMPARE DISTANCES - IF 1ST INTERACTABLE IS CLOSER
            if (distanceTo1 <= distanceTo2)
            {
                //CHECK IF TAG IS WEAPON
                if (interactable1.tag == "Weapon")
                {
                    //IF PLAYER IS UNARMED
                    if (!playerStatesData.isArmed)
                    {
                        //PICK CLOSER WEAPON
                        PickUpWeapon(interactable1);
                    }
                    else
                    {
                        //DROP WEAPON
                        DropWeapon();

                        //PICK UP NEW WEAPON
                        PickUpWeapon(interactable1);
                    }
                }
            }
            //IF 2ND IS CLOSER - DO THE SAME THING
            else
            {
                if (interactable2.tag == "Weapon")
                {
                    if (!playerStatesData.isArmed)
                    {
                        PickUpWeapon(interactable2);
                    }
                    else
                    {
                        //DROP WEAPON
                        DropWeapon();

                        //PICK UP NEW WEAPON
                        PickUpWeapon(interactable2);
                    }
                }
            }
        }
    }

    public void PickUpWeapon(GameObject weapon)
    {
        //ASSIGN WEAPON TO PARENT HAND
        weapon.transform.parent = parentHand.transform;
        //CHANGE WEAPONS TRANSFORM
        weapon.transform.position = weaponPosition.position;
        weapon.transform.localScale = weaponPosition.localScale;
        weapon.transform.rotation = weaponPosition.rotation;
        //ASSIGN CURRENT WEAPON
        currentWeapon = weapon;
        //RAISE GOT ARMED
        eGotArmed.Raise();
    }

    public void DropWeapon()
    {
        //REMOVE CURRENT WEAPON FROM PLAYER'S GAME OBJECT
        currentWeapon.transform.SetParent(null);
        //PLACE IT INFRONT OF PLAYER
        Vector2 dropPosition = currentWeapon.transform.position;
        dropPosition.x += 1.75f * playerMovementData.facingDirection;
        dropPosition.y -= 0.75f;
        currentWeapon.transform.position = dropPosition;
        currentWeapon.GetComponentInChildren<BoxCollider2D>().enabled = false;
        eDropWeapon.Raise();
    }
}
