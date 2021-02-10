using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{
    //public PlayerInputData playerInputData;
    public PlayerStatesData playerStatesData;
    public PlayerMovementData playerMovementData;
    public Collider2D[] interactablesInReach = new Collider2D[2];
    public LayerMask interactableLayers;
    public GameObject parentHand;
    public Transform weaponPosition;
    public GameEvent eGotArmed;

    public GameObject currentWeapon;

    public void Interact()
    {
        interactablesInReach = Physics2D.OverlapCapsuleAll(gameObject.transform.position, GetComponent<CapsuleCollider2D>().size, CapsuleDirection2D.Horizontal, 0, interactableLayers);

        if (interactablesInReach == null)
        {
            return;
        }
        else if (interactablesInReach.Length == 1)
        {
            GameObject interactable = interactablesInReach[0].gameObject;
            if (interactable.tag == "Weapon")
            {
                if (!playerStatesData.isArmed)
                {
                    PickUpWeapon(interactable);
                }
                else
                {
                    //DROP WEAPON
                    DropWeapon();

                    //PICK UP NEW WEAPON
                    PickUpWeapon(interactable);
                }
            }
        }
        else if (interactablesInReach.Length == 2)
        {
            GameObject player = GetComponentInParent<OfInterest>().gameObject;
            GameObject interactable1 = interactablesInReach[0].gameObject;
            GameObject interactable2 = interactablesInReach[1].gameObject;

            float distanceTo1 = Vector2.Distance(player.transform.position, interactable1.transform.position);
            float distanceTo2 = Vector2.Distance(player.transform.position, interactable2.transform.position);

            if (distanceTo1 <= distanceTo2)
            {
                if (interactable1.tag == "Weapon")
                {
                    if (!playerStatesData.isArmed)
                    {
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
        Rigidbody2D rigidBody = weapon.GetComponent<Rigidbody2D>();
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0;
        rigidBody.isKinematic = true;
        weapon.transform.parent = parentHand.transform;
        weapon.transform.position = weaponPosition.position;
        weapon.transform.localScale = weaponPosition.localScale;
        weapon.transform.rotation = weaponPosition.rotation;
        weapon.layer = 0;
        currentWeapon = weapon;
        eGotArmed.Raise();
    }

    public void DropWeapon()
    {
        currentWeapon.transform.SetParent(null);
        Vector2 dropPosition = currentWeapon.transform.position;
        dropPosition.x += playerMovementData.facingDirection;
        dropPosition.y -= 0.5f;
        currentWeapon.transform.position = dropPosition;
        currentWeapon.GetComponent<Rigidbody2D>().isKinematic = false;
        currentWeapon.layer = 8;
    }
}
