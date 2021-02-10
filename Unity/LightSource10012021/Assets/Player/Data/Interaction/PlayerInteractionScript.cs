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

    public GameObject weapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Collider2D[] interactables = Physics2D.OverlapCapsuleAll(transform.position, GetComponent<CapsuleCollider2D>().size, CapsuleDirection2D.Horizontal, 0);
        //if (interactables != null)
        //{
        //    for (int i = 0; i < interactables.Length; i++)
        //    {
        //        if (interactables[i].tag == "Weapon" && playerInputData.buttonNorth != 0)
        //        {
        //            Debug.Log("Pickup!");
        //        }
        //    }
        //}

    }

    public void InteractionPressed()
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
                    interactable.transform.parent = parentHand.transform;
                    interactable.GetComponent<Rigidbody2D>().isKinematic = true;
                    interactable.transform.position = weaponPosition.position;
                    interactable.transform.localScale = weaponPosition.localScale;
                    //Vector2 scale = interactable.transform.localScale;
                    //scale.x = playerMovementData.facingDirection;
                    //interactable.transform.localScale = scale;
                    interactable.layer = 0;
                    eGotArmed.Raise();
                }
                else
                {
                    //DROP WEAPON                    
                    GameObject player = GetComponentInParent<OfInterest>().gameObject;
                    weapon = player.GetComponentInChildren<WeaponScript>().gameObject;
                    weapon.transform.SetParent(null);
                    Vector2 dropPosition = weapon.transform.position;
                    dropPosition.x += playerMovementData.facingDirection;
                    dropPosition.y -= 0.5f;
                    weapon.transform.position = dropPosition;
                    weapon.GetComponent<Rigidbody2D>().isKinematic = false;
                    //Transform weaponPosition = weapon.transform;
                    //weapon.transform.SetParent(null);
                    //Vector2 dropPosition = weaponPosition.position;
                    //dropPosition.x += 3 * playerMovementData.facingDirection;
                    //weaponPosition.position = dropPosition;
                    //weapon.transform.position = weaponPosition.position;
                    weapon.layer = 8;

                    //PICK UP NEW WEAPON
                    interactable.transform.parent = parentHand.transform;
                    interactable.GetComponent<Rigidbody2D>().isKinematic = true;
                    interactable.transform.position = weaponPosition.position;
                    interactable.transform.localScale = weaponPosition.localScale;
                    interactable.layer = 0;
                    return;
                }
            }
        }
        else if (interactablesInReach.Length == 2)
        {
            GameObject interactable1 = interactablesInReach[0].gameObject;
            GameObject interactable2 = interactablesInReach[1].gameObject;                
        }



        //    if (!playerStatesData.isArmed)
        //{
        //    //interactablesInReach = Physics2D.OverlapCapsuleAll(gameObject.transform.position, GetComponent<CapsuleCollider2D>().size, CapsuleDirection2D.Horizontal, 0, interactableLayers);
        //    for (int i = 0; i < interactablesInReach.Length; i++)
        //    {
        //        if (interactablesInReach[i].tag == "Weapon")
        //        {
        //            //GameObject weapon = interactablesInReach[i].gameObject;
        //            interactablesInReach[i].gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        //            interactablesInReach[i].gameObject.transform.position = weaponPosition.position;
        //            interactablesInReach[i].gameObject.transform.parent = parentHand.transform;
        //            eGotArmed.Raise();
        //        }
        //    }
        //}
    }
}
