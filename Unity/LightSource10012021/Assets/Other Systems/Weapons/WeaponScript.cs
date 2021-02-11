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
    public Transform wieldingHand;

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

        //IF THERE IS A WIELDER
        if (wielder != null)
        {
            //UPDATE SWORD POSITION                         //NEEDS WORKSHOPPING
            transform.position = wieldingHand.position;
            //IF THEIR TAG IS PLAYER
            if (wielder.tag == "Player")
            {
                //PUT DAMAGE COLLIDER ON PLAYER WEAPON LAYER
                damageCollider.gameObject.layer = 10;
                //IF PLAYER IS ATTACKING
                if (wielder.GetComponentInChildren<PlayerStatesScript>().playerStatesData.isAttacking)
                {
                    //TURN ON DAMAGE COLLIDER
                    damageCollider.enabled = true;
                }
                //IF NOT
                else
                {
                    //TURN OFF DAMAGE COLLIDER
                    damageCollider.enabled = false;
                }
            }
            //IF WIELDER ISN'T PLAYER
            else
            {
                //PUT DAMAGE COLLIDER ON WEAPON DAMAGE LAYER
                damageCollider.gameObject.layer = 9;
            }
        }
        //IF THERE IS NO WIELDER
        else
        {
            //PUT DAMAGE COLLIDER ON WEAPON DAMAGE LAYER
            damageCollider.gameObject.layer = 9;
        }
    }
    public void IsWielded()
    {
        //ASSIGN WIELDER
        wielder = GetComponentInParent<OfInterest>().gameObject;
        wieldingHand = wielder.GetComponentInChildren<MainHandScript>().gameObject.transform;
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
