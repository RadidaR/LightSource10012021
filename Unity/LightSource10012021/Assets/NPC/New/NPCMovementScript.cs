using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementScript : MonoBehaviour
{
    public GameObject npc;

    public NPCData data;
    public NPCAbilities abilities;

    public NPCStatesScript states;

    public Rigidbody2D rigidBody;

    public Vector2 velocity;

    public Transform ledgeCheck;
    public float ledgeCheckRadius;
    public bool ledgeAhead;

    public Transform wallCheck;
    public float wallCheckRadius;
    public bool wallAhead;


    public void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            npc = GetComponentInParent<OfInterest>().gameObject;

            states = npc.GetComponent<NPCStatesScript>();

            data = states.data;
            abilities = states.abilities;

            rigidBody = npc.GetComponent<Rigidbody2D>();
        }
    }

    public void Update()
    {
        ledgeAhead = !Physics2D.OverlapCircle(ledgeCheck.position, ledgeCheckRadius);
        wallAhead = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius);

        if (abilities.avoidsLedges)
        {
            if (ledgeAhead)
            {
                rigidBody.constraints = RigidbodyConstraints2D.FreezePositionX;
            }
            else
            {
                rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }


        if (wallAhead)
        {
            //rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            //rigidBody.AddForce(new Vector2(0, 8), ForceMode2D.Force);
        }
    }

    public void StopMoving()
    {
        if (!abilities.canFly)
        {
            velocity = rigidBody.velocity;
            velocity.x = 0;
            rigidBody.velocity = velocity;
        }
    }

    //public void Walk()
    //{
    //    velocity = rigidBody.velocity;
    //    velocity.x = data.moveSpeed * states.facingDirection;
    //    rigidBody.velocity = velocity;
    //}

    //public void Run()
    //{
    //    velocity = rigidBody.velocity;
    //    velocity.x = data.runSpeed * states.facingDirection;
    //    rigidBody.velocity = velocity;
    //}

    public void Move(float speed)
    {
        //velocity = rigidBody.velocity;
        //velocity.x = speed * states.facingDirection;
        rigidBody.velocity = new Vector2(speed * states.facingDirection, rigidBody.velocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        if (ledgeCheckRadius == 0)
        {
            return;
        }

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(ledgeCheck.position, ledgeCheckRadius);

        if (wallCheckRadius == 0)
        {
            return;
        }

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
    }
}
