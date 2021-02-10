using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public WeaponData weaponData;
    public CircleCollider2D interactionCollider;
    public BoxCollider2D damageCollider;
    public CapsuleCollider2D groundCollider;

    public Rigidbody2D rigidBody;

    public bool isCarried;
    public GameObject wielder;

    public int durability;
    // Start is called before the first frame update
    void Start()
    {
        durability = weaponData.durability;
        rigidBody = GetComponent<Rigidbody2D>();
        damageCollider = GetComponentInChildren<BoxCollider2D>();
        interactionCollider = GetComponent<CircleCollider2D>();
        groundCollider = GetComponentInChildren<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.parent == null)
        {
            IsNotWielded();
        }
        else
        {
            IsWielded();
        }

        //STOP DAMAGE COLLIDER ON STATIC WEAPON
        if (!isCarried && Mathf.Abs(rigidBody.velocity.x) < 1 && Mathf.Abs(rigidBody.velocity.y) < 1)
        {
            damageCollider.enabled = false;
        }

        //ASSIGN DAMAGE COLLIDER TO CORRECT LAYER IF PLAYER IS WIELDER
        if (wielder != null)
        {
            if (wielder.tag == "Player")
            {
                damageCollider.gameObject.layer = 10;
                if (wielder.GetComponentInChildren<PlayerStatesScript>().playerStatesData.isAttacking)
                {
                    damageCollider.enabled = true;
                }
                else
                {
                    damageCollider.enabled = false;
                }
            }
            else
            {
                damageCollider.gameObject.layer = 9;
            }
        }
        else
        {
            damageCollider.gameObject.layer = 9;
        }
    }
    public void IsWielded()
    {
        //ASSIGN WIELDER
        wielder = GetComponentInParent<OfInterest>().gameObject;
        isCarried = true;
        //STOP WEAPON'S MOTION
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0;
        rigidBody.isKinematic = true;
        //TURN OFF COLLIDERS
        interactionCollider.enabled = false;
        groundCollider.enabled = false;
        damageCollider.enabled = false;
    }
    public void IsNotWielded()
    {
        //REMOVE WIELDER
        wielder = null;
        isCarried = false;
        //REVER TO DYNAMIC
        rigidBody.isKinematic = false;
        //ENABLE COLLIDERS
        interactionCollider.enabled = true;
        groundCollider.enabled = true;
        if (Mathf.Abs(rigidBody.velocity.x) > 3 && Mathf.Abs(rigidBody.velocity.y) > 3)
        {
            damageCollider.enabled = true;
        }
        //PUT DAMAGE COLLIDER ON WEAPON DAMAGE LAYER
        damageCollider.gameObject.layer = 9;
    }


}
