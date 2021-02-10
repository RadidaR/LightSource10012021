using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponThrowScript : MonoBehaviour
{
    public PlayerStatesData playerStatesData;
    public PlayerMovementData playerMovementData;
    public PlayerInputData playerInputData;
    public WeaponData playerWeaponData;

    public GameObject player;
    public GameObject weapon;

    public GameEvent eWeaponThrown;

    public float chargeTime = 0;
    public float maxCharge;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<OfInterest>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStatesData.isArmed)
        {
            if (playerInputData.leftTrigger > 0)
            {
                chargeTime += Time.deltaTime;

                if (chargeTime >= maxCharge)
                {
                    chargeTime = 0;
                    eWeaponThrown.Raise();
                }
            }
            if (playerInputData.leftTrigger == 0)
            {
                chargeTime = 0;
            }
        }
    }

    public void WeaponCarried()
    {
        playerWeaponData = player.GetComponentInChildren<WeaponScript>().weaponData;
        weapon = player.GetComponentInChildren<WeaponScript>().gameObject;
    }

    public void ThrowWeapon()
    {
        Rigidbody2D rigidBody = weapon.GetComponent<Rigidbody2D>();
        weapon.transform.SetParent(null);
        Vector2 throwPosition = weapon.transform.position;
        throwPosition.x += 2 * playerMovementData.facingDirection;
        throwPosition.y += 0.5f;
        weapon.transform.position = throwPosition;
        rigidBody.isKinematic = false;
        rigidBody.AddForce(new Vector2(playerWeaponData.throwForce.x * playerMovementData.facingDirection, playerWeaponData.throwForce.y), ForceMode2D.Impulse);
        rigidBody.AddTorque(playerWeaponData.throwTorque * -playerMovementData.facingDirection, ForceMode2D.Impulse);
    }
}
