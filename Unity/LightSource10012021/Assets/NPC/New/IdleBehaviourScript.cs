using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviourScript : MonoBehaviour
{
    public GameObject npc;

    public NPCData data;
    public NPCAbilities abilities;

    public NPCStatesScript states;

    public NPCMovementScript movement;

    public Rigidbody2D rigidBody;

    public float stayTime1;
    public float stayTime2;
    
    public float stayTimer;

    public float walkTime1;
    public float walkTime2;

    public float walkTimer;

    public bool idleRunning;



    private void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            npc = GetComponentInParent<OfInterest>().gameObject;
            states = npc.GetComponent<NPCStatesScript>();

            data = states.data;
            abilities = states.abilities;

            movement = npc.GetComponentInChildren<NPCMovementScript>();

            //seekTarget = npc.GetComponentInChildren<SeekTargetScript2>();
            //attacks = npc.GetComponentInChildren<NPCAttackScript>();
            rigidBody = npc.GetComponent<Rigidbody2D>();

            //if (abilities.canFly)
            //{
            //    navAgent = npc.GetComponent<NavMeshAgent2D>();
            //}
        }
    }


    private void FixedUpdate()
    {
        if (states.isIdle)
        {
            IdleBehaviour();
        }
        else
        {
            stayTimer = 0;
            walkTimer = 0;
            idleRunning = false;
        }



    }

    public void FlipNPC()
    {
        Vector2 npcScale = npc.transform.localScale;
        npcScale.x = states.facingDirection;
        npc.transform.localScale = npcScale;
    }

    public void IdleBehaviour()
    {
        if (!idleRunning)
        {
            idleRunning = true;
            movement.StopMoving();
            stayTimer = Random.Range(stayTime1, stayTime2);
        }
        else
        {
            if (stayTimer > 0)
            {
                stayTimer -= Time.fixedDeltaTime;
                movement.StopMoving();
                return;
            }
            else
            {
                stayTimer = 0;
                if (walkTimer == 0)
                {

                    int direction = Random.Range(-1, 2);
                    if (direction == -1)
                    {
                        states.facingDirection = -1;
                    }
                    else if (direction == 1)
                    {
                        states.facingDirection = 1;
                    }
                    else if (direction == 0)
                    {
                        idleRunning = false;
                        return;
                    }

                    walkTimer = Random.Range(walkTime1, walkTime2);

                    FlipNPC();
                }
            }

            if (walkTimer > 0)
            {
                walkTimer -= Time.fixedDeltaTime;
                movement.Move(data.moveSpeed);
            }
            else
            {
                walkTimer = 0;
                idleRunning = false;
            }
        }
    }
}
