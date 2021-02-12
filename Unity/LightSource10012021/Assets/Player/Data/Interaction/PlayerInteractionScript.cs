using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{
    [Header("Data Types")]
    public PlayerInputData playerInputData;
    public PlayerStatesData playerStatesData;
    public PlayerMovementData playerMovementData;

    [Header("Events")]
    public GameEvent eGotArmed;
    public GameEvent eDropWeapon;
    public GameEvent eStopThrow;

    [Header("Local Variables")]
    [SerializeField] GameObject player;
    [SerializeField] Transform weaponPosition;
    [SerializeField] GameObject currentWeapon;

    [Header("Interactables Variables")]
    [SerializeField] LayerMask interactableLayers;
    [SerializeField] Collider2D[] interactablesInReach;
    [SerializeField] float[] distances;
    [SerializeField] float distanceToClosest;
    [SerializeField] GameObject closestInteractable;

    private void Start()
    {
        player = GetComponentInParent<OfInterest>().gameObject;
        weaponPosition = player.GetComponentInChildren<MainHandScript>().gameObject.transform;
    }

    public void Interact()
    {
        //IF PLAYER IS PREPARING TO THROW WEAPON
        if (playerStatesData.isThrowing)
        {
            //RAISE STOP THROW
            eStopThrow.Raise();
            //AND RETURN
            return;
        }

        //GATHER INTERACTABLES IN REACH IN ARRAY
        interactablesInReach = Physics2D.OverlapCapsuleAll(gameObject.transform.position, GetComponent<CapsuleCollider2D>().size, CapsuleDirection2D.Horizontal, 0, interactableLayers);
        //MAKE DISTANCES ARRAY SAME SIZE AS INTERACTABLES IN REACH
        distances = new float[interactablesInReach.Length];
        //LOCAL VARIABLE DISTANCE TO INTERACTABLE THAT IS CURRENTLY BEING CHECKED
        float distanceToInteractable;

        //LOOP THROUGH ALL INTERACTABLES
        for (int i = 0; i < interactablesInReach.Length; i++)
        {
            //CALCULATE DISTANCE TO INTERACTABLE THAT IS CURRENTLY BEING CHECKED
            distanceToInteractable = Vector2.Distance(player.transform.position, interactablesInReach[i].transform.position);
            //ASSIGN VALUE TO SAME NUMBERED ELEMENT IN DISTANCES ARRAY
            distances[i] = distanceToInteractable;

            //IF LAST INTERACTABLE IN THE ARRAY IS BEING CHECKED
            if (i == interactablesInReach.Length - 1)
            {
                //CHECK WHICH INTERACTABLE IS CLOSEST
                distanceToClosest = Mathf.Min(distances);
            }
        }

        //LOOP THROUGH INTERACTABLES AGAIN
        for (int i = 0; i < interactablesInReach.Length; i++)
        {
            //CHECK DISTANCE TO INTERACTABLE THAT IS CURRENTLY BEING CHECKED
            distanceToInteractable = Vector2.Distance(player.transform.position, interactablesInReach[i].transform.position);

            //IF IT IS THE SAME AS THE CLOSEST INTERACTABLE
            if (distanceToClosest == distanceToInteractable)
            {
                //ASSIGN THAT INTERACTABLE AS THE ONE TO BE INTERACTED WITH
                closestInteractable = interactablesInReach[i].gameObject;
            }
        }

        //IF NO INTERACTABLES IN REACH
        if (interactablesInReach.Length == 0)
        {
            //AND IF PLAYER IS ARMED
            if (playerStatesData.isArmed)
            {
                //DROP CURRENT WEAPON
                DropWeapon();
            }
            //RETURN TO AVOID ERRORS
            return;
        }

        //IF CLOSEST INTERACTABLE HAS WEAPON TAG
        if (closestInteractable.tag == "Weapon")
        {
            //AND IF PLAYER IS UNARMED
            if (!playerStatesData.isArmed)
            {
                //PICK UP CLOSEST INTERACTABLE
                PickUpWeapon(closestInteractable);
            }
            //ELSE IF PLAYER IS ARMED
            else
            {
                //DROP CURRENT WEAPON
                DropWeapon();
                //AND PICK UP CLOSEST INTERACTABLE
                PickUpWeapon(closestInteractable);
            }
        }
    }

    //RESETS ALL INFO GATHERED FOR INTERACTION
    public void ResetInteractables()
    {
        interactablesInReach = null;
        distances = null;
        distanceToClosest = 0;
        closestInteractable = null;
    }

    public void PickUpWeapon(GameObject weapon)
    {
        //ASSIGN WEAPON TO PARENT HAND
        weapon.transform.parent = weaponPosition.transform;
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
        //DISABLE WEAPON'S DAMAGING COLLIDER
        currentWeapon.GetComponentInChildren<BoxCollider2D>().enabled = false;
        //REMOVE CURRENT WEAPON
        currentWeapon = null;
        //RAISE DROP WEAPON
        eDropWeapon.Raise();
    }
}
