using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionScript : MonoBehaviour
{
    public PlayerStates playerStatesData;

    public Transform playerBodyPosition;
    public Vector2 playerBodySize;
    public LayerMask damagingLayers;

    //public List<Collider2D> collisions;
    //public Collider2D[] collisions;
    //float a;

    public GameEvent eCollided;

    private void Update()
    {
        ////IF NOT HURT
        //if (!playerStatesData.isHurt)
        //{
        //    //IF BODY OVERLAPS WITH DAMAGING LAYER
        //    if (Physics2D.OverlapBox(playerBodyPosition.position, playerBodySize, 0f, damagingLayers))
        //    {
        //        RaiseCollision();
        //    }
        //}
    }

    public void RaiseCollision()
    {
        eCollided.Raise();
    }

    void OnDrawGizmosSelected()
    {
        if (playerBodySize == Vector2.zero)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerBodyPosition.position, playerBodySize);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerStatesData.isHurt)
        {
            if (collision.gameObject.layer == 6)
            {
                RaiseCollision();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("Collision" + collision.gameObject.layer.ToString());
        if (!playerStatesData.isHurt)
        {
            if (collision.gameObject.layer == 6)
            {
                RaiseCollision();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision" + collision.gameObject.layer.ToString());
    }
}
