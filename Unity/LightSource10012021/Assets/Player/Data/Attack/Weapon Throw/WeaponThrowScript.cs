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
    public Rigidbody2D rigidBody;

    public GameEvent eWeaponThrown;
    public GameEvent eChargeThrow;

    public float chargeTime = 0;
    public float maxCharge;

    //public bool reeling;

    //public Vector2 throwPosition;
    public Transform throwPosition;

    [Header("Line renderer variables")]
    public LineRenderer lineRenderer;
    [Range(2, 30)] public int resolution;

    [Header("Formula variables")]
    //public Vector2 velocity;
    public float yLimit;
    public float g;

    [Header("Linecast variables")]
    [Range(2, 30)] public int linecastResolution;
    public LayerMask canHit;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<OfInterest>().gameObject;

        //ASSIGN GRAVITY TO POSITIVE NUMBER
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInputData.leftTrigger == 0)
        {
            chargeTime = 0;
        }
    }

    public void ChargeThrow()
    {
        if (playerStatesData.isArmed)
        {
            eChargeThrow.Raise();
            chargeTime += Time.deltaTime;

            if (chargeTime > 0)
            {
                StartCoroutine(RenderArc());
            }

            if (chargeTime >= maxCharge)
            {
                chargeTime = maxCharge;
            }
        }
    }

    public void WeaponCarried()
    {
        playerWeaponData = player.GetComponentInChildren<WeaponScript>().weaponData;
        weapon = player.GetComponentInChildren<WeaponScript>().gameObject;
        rigidBody = weapon.GetComponent<Rigidbody2D>();

        //ASSIGN GRAVITY TO BE POSITIVE NUMBER * RIGIDBODY GRAVITY
        g = Mathf.Abs(Physics2D.gravity.y) * rigidBody.gravityScale;
    }

    public void ThrowWeapon()
    {
        if (playerStatesData.isThrowing)
        {
            eWeaponThrown.Raise();
            //Rigidbody2D rigidBody = weapon.GetComponent<Rigidbody2D>();
            weapon.transform.SetParent(null);
            //throwPosition = weapon.transform.position;
            //throwPosition.x += 2 * playerMovementData.facingDirection;
            //throwPosition.y += 0.5f;
            weapon.transform.position = throwPosition.position;
            rigidBody.isKinematic = false;
            rigidBody.AddForce(new Vector2(playerWeaponData.throwForce.x * playerMovementData.facingDirection * chargeTime, playerWeaponData.throwForce.y), ForceMode2D.Impulse);
            rigidBody.AddTorque(playerWeaponData.throwTorque * -playerMovementData.facingDirection, ForceMode2D.Impulse);
            RemoveWeapon();
            chargeTime = 0;
            eWeaponThrown.Raise();
        }
    }

    public void RemoveWeapon()
    {
        playerWeaponData = null;
        weapon = null;
    }


















    //TRAJECTORY PATH STUFF
    private IEnumerator RenderArc()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = resolution + 1;
        lineRenderer.SetPositions(CalculateLineArray());
        yield return null;
    }

    private Vector3[] CalculateLineArray()
    {
        eChargeThrow.Raise();
        Vector3[] lineArray = new Vector3[resolution + 1];

        var lowestTimeValue = MaxTimeX() / resolution;

        for (int i = 0; i < lineArray.Length; i++)
        {
            var t = lowestTimeValue * i;
            //var t = i / (float)lineArray.Length;
            lineArray[i] = CalculateLinePoint(t);
        }

        return lineArray;
    }

    private Vector2 HitPosition()
    {
        var lowestTimeValue = MaxTimeY() / linecastResolution;

        for (int i = 0; i < linecastResolution + 1; i++)
        {
            var t = lowestTimeValue * i;
            var tt = lowestTimeValue * (i + 1);

            var hit = Physics2D.Linecast(CalculateLinePoint(t), CalculateLinePoint(tt), canHit);

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
        float y = (playerWeaponData.throwForce.y * t) - (g * Mathf.Pow(t, 2) / 2);
        return new Vector3(x + throwPosition.position.x, y + throwPosition.position.y);
    }

    private float MaxTimeY()
    {
        var y = playerWeaponData.throwForce.y;
        var yy = y * y;

        var t = (y + Mathf.Sqrt(yy + 2 * g * (throwPosition.position.y - yLimit))) / g;
        return t;
    }

    private float MaxTimeX()
    {
        var x = playerWeaponData.throwForce.x * playerMovementData.facingDirection * chargeTime;
        var t = (HitPosition().x - throwPosition.position.x /** playerMovementData.facingDirection*/) / x;
        return t;
    }

    public void ResetLine()
    {
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
        //chargeTime = 0;
    }
    //TRAJECTORY PATH STUFF
}
