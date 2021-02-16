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

    public void StopMoving()
    {
        if (!abilities.canFly)
        {
            velocity = rigidBody.velocity;
            velocity.x = 0;
            rigidBody.velocity = velocity;
        }
    }

    public void Walk()
    {
        velocity = rigidBody.velocity;
        velocity.x = data.moveSpeed * states.facingDirection;
        rigidBody.velocity = velocity;
    }

    public void Run()
    {
        velocity = rigidBody.velocity;
        velocity.x = data.runSpeed * states.facingDirection;
        rigidBody.velocity = velocity;
    }


}
