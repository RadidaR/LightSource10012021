using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckScript : MonoBehaviour
{
    public PlayerStates playerStatesData;

    public Transform groundCheckPosition;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    public GameEvent eOverGround;
    public GameEvent eNoGround;

    private void Update()
    {
        if (Physics2D.OverlapCircle(groundCheckPosition.position, groundCheckRadius, groundLayer))
        {
            //Debug.Log("Grounded");
            eOverGround.Raise();
        }
        else
        {
            //Debug.Log("Not Grounded");
            eNoGround.Raise();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheckRadius == 0)
        {
            return;
        }

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(groundCheckPosition.position, groundCheckRadius);
    }
}
