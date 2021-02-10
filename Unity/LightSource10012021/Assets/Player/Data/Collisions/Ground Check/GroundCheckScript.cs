using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckScript : MonoBehaviour
{
    public PlayerStatesData playerStatesData;
    public Transform groundCheckPosition;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    public GameEvent eOverGround;
    public GameEvent eNoGround;

    private void Update()
    {
        if (!playerStatesData.isFloating)
        {
            if (Physics2D.OverlapCircle(groundCheckPosition.position, groundCheckRadius, groundLayer))
            {
                eOverGround.Raise();
            }
            else
            {
                eNoGround.Raise();
            }
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
