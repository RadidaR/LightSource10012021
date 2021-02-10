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

                if (chargeTime >= 1.25f)
                {
                    chargeTime = 0;
                    Debug.Log("Charged");
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
        weapon.transform.SetParent(null);
        Vector2 dropPosition = weapon.transform.position;
        dropPosition.x += 2 * playerMovementData.facingDirection;
        dropPosition.y += 0.5f;
        weapon.transform.position = dropPosition;
        weapon.GetComponent<Rigidbody2D>().isKinematic = false;
        weapon.GetComponent<Rigidbody2D>().AddForce(new Vector2(playerWeaponData.throwForce.x * playerMovementData.facingDirection, playerWeaponData.throwForce.y), ForceMode2D.Impulse);
        weapon.GetComponent<Rigidbody2D>().AddTorque(playerWeaponData.throwTorque * -playerMovementData.facingDirection, ForceMode2D.Impulse);
        weapon.layer = 8;
        Debug.Log("Weapon Thrown");
    }
}
