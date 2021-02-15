using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponThrowScript : MonoBehaviour
{
    [Header("Data Types")]
    public PlayerStatesData playerStatesData;
    public PlayerMovementData playerMovementData;
    public PlayerInputData playerInputData;
    public WeaponData playerWeaponData;

    [Header("Events")]
    public GameEvent eWeaponThrown;
    public GameEvent eChargeThrow;

    [Header("Local Variables")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject weapon;

    [Header("Weapon's Rigidbody")]
    [SerializeField] Rigidbody2D rigidBody;

    [Header("Charge Throw Variables")]
    [SerializeField] float chargeTime = 0;
    [SerializeField] float maxCharge;

    [Header("Aim Anchor and Launch Position")]
    [SerializeField] Transform throwPosition;
    [SerializeField] Transform aimAnchor;

    [Header("Line Renderer Variables")]
    [SerializeField] LineRenderer lineRenderer;
    [Range(2, 50)] [SerializeField] int resolution;

    [Header("Formula Variables")]
    [SerializeField] float yLimit;
    [SerializeField] float gravitationalPull;

    [Header("Linecast Variables")]
    [Range(2, 50)] [SerializeField] int linecastResolution;
    [SerializeField] LayerMask canHit;

    void Start()
    {
        player = GetComponentInParent<OfInterest>().gameObject;
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        //IF PLAYER ISN'T PRESSING LEFT TRIGGER
        if (playerInputData.leftTrigger == 0f)
        {
            //RESET CHARGE TIME
            chargeTime = 0;
        }

        if (weapon != null)
        {
            if (weapon.GetComponent<WeaponScript>().durability < 1)
            {
                RemoveWeapon();
            }
        }

    }

    public void ChargeThrow()
    {
        //IF PLAYER IS ARMED
        if (playerStatesData.isArmed)
        {
            //RAISE CHARGE THROW
            eChargeThrow.Raise();
            //INCREASE CHARGE TIME
            chargeTime += Time.deltaTime;

            //IF PLAYER IS AIMING WITH RIGHT STICK
            if (playerInputData.rightStickValue != Vector2.zero)
            {
                //UPDATE AIM POSITION TO MATCH RIGHT STICK
                Vector2 aimPosition = aimAnchor.position;
                aimPosition.x += playerInputData.rightStickValue.x + (2f * playerMovementData.facingDirection);
                aimPosition.y += playerInputData.rightStickValue.y * 2f;
                //ASSIGN THROW POSITION TO AIM POSITION
                throwPosition.position = aimPosition;
            }
            //ELSE IF PLAYER ISN'T AIMING WITH RIGHT STICK
            else
            {
                //ASSIGN AIM POSITION TO STATIC POSITION
                Vector2 aimPosition = aimAnchor.position;
                aimPosition.x += 2f * playerMovementData.facingDirection;
                aimPosition.y += 1f;
                //ASSIGN THROW POSITION TO AIM POSITION
                throwPosition.position = aimPosition;
            }

            //IF CHARGE TIME ISN'T 0
            if (chargeTime > 0)
            {
                //START LINE RENDERER
                StartCoroutine(RenderArc());
            }

            //PREVENTS FROM OVERCHARGING
            if (chargeTime >= maxCharge)
            {
                chargeTime = maxCharge;
            }
        }
    }

    public void WeaponCarried()
    {
        //TAKE WEAPON'S DATA
        playerWeaponData = player.GetComponentInChildren<WeaponScript>().weaponData;
        //ASSIGN WEAPON
        weapon = player.GetComponentInChildren<WeaponScript>().gameObject;
        //TAKE IT'S RIGIDBODY
        rigidBody = weapon.GetComponent<Rigidbody2D>();

        //ASSIGN GRAVITATIONAL PULL TO BE POSITIVE NUMBER * WEAPON'S RIGIDBODY GRAVITY
        gravitationalPull = Mathf.Abs(Physics2D.gravity.y) * rigidBody.gravityScale;
    }

    public void ThrowWeapon()
    {
        //IF PLAYER IS PREPARING TO THROW WEAPON
        if (playerStatesData.isThrowing && weapon != null)
        {
            //RAISE WEAPON THROWN
            eWeaponThrown.Raise();
            //DETATCH WEAPON FROM PLAYER OBJECT
            weapon.transform.SetParent(null);

            //ASSIGN WEAPON POSITION TO LAUNCH POSITION
            weapon.transform.position = throwPosition.position;
            //MAKE WEAPON DYNAMIC AGAIN
            rigidBody.isKinematic = false;

            //IF PLAYER ISN'T USING RIGHT STICK TO AIM
            if (playerInputData.rightStickValue == Vector2.zero)
            {
                //THROW IN FACING DIRECTION
                rigidBody.velocity = new Vector2(playerWeaponData.throwForce.x * playerMovementData.facingDirection * chargeTime, playerWeaponData.throwForce.y);
            }
            //ELSE IF PLAYER IS USING RIGHT STICK
            else
            {
                //THROW IN DIRECTION OF STICK
                rigidBody.velocity = new Vector2(playerWeaponData.throwForce.x * playerMovementData.facingDirection * chargeTime, playerWeaponData.throwForce.y * chargeTime * playerInputData.rightStickValue.y);
            }

            //ADD ROTATION
            rigidBody.AddTorque(playerWeaponData.throwTorque * -playerMovementData.facingDirection, ForceMode2D.Impulse);
            //UNASSIGN WEAPON DETAILS
            RemoveWeapon();
            //RESET CHARGE TIME
            chargeTime = 0;
            //RAISE WEAPON THROWN
            eWeaponThrown.Raise();
        }
    }

    //UNASSIGNS WEAPON AND WEAPON DATA
    public void RemoveWeapon()
    {
        ResetLine();
        playerWeaponData = null;
        weapon = null;
    }

    ////////////TRAJECTORY PATH////////////

    private IEnumerator RenderArc()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = resolution;
        lineRenderer.SetPositions(CalculateLineArray());
        yield return null;
    }

    private Vector3[] CalculateLineArray()
    {
        eChargeThrow.Raise();
        Vector3[] lineArray = new Vector3[resolution];

        float lowestTimeValue = MaxTimeX() / resolution;

        for (int i = 0; i < lineArray.Length; i++)
        {
            float t = lowestTimeValue * i;
            lineArray[i] = CalculateLinePoint(t);
        }

        return lineArray;
    }

    private Vector2 HitPosition()
    {
        float lowestTimeValue = MaxTimeY() / linecastResolution;

        for (int i = 0; i < linecastResolution + 1; i++)
        {
            float t = lowestTimeValue * i;
            float tt = lowestTimeValue * (i + 1);

            RaycastHit2D hit = Physics2D.Linecast(CalculateLinePoint(t), CalculateLinePoint(tt), canHit);

            if (hit)
            {
                return hit.point;
            }
        }

        return CalculateLinePoint(MaxTimeY());
    }

    private Vector2 CalculateLinePoint(float t)
    {
        float x = playerWeaponData.throwForce.x * chargeTime * playerMovementData.facingDirection * t;
        float y;

        if (playerInputData.rightStickValue == Vector2.zero)
        {
            y = (playerWeaponData.throwForce.y * t) - (gravitationalPull * Mathf.Pow(t, 2) / 2);
        }
        else
        {
            y = (playerWeaponData.throwForce.y * chargeTime * playerInputData.rightStickValue.y * t) - (gravitationalPull * Mathf.Pow(t, 2) / 2); 
        }

        return new Vector3(x + throwPosition.position.x, y + throwPosition.position.y);
    }

    private float MaxTimeY()
    {
        float y = playerWeaponData.throwForce.y;
        float yy = y * y;

        float t = (y + Mathf.Sqrt(yy + 2 * gravitationalPull * (throwPosition.position.y - yLimit))) / gravitationalPull;
        return t;
    }

    private float MaxTimeX()
    {
        float x = playerWeaponData.throwForce.x * playerMovementData.facingDirection * chargeTime;
        float t = (HitPosition().x - throwPosition.position.x) / x;
        return t;
    }

    public void ResetLine()
    {
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
    }


    ////////////TRAJECTORY PATH////////////




}
