using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviourScript : MonoBehaviour
{
    public GameObject npc;

    public NPCData data;
    public NPCAbilities abilities;

    public NPCStatesScript states;

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
            if (!idleRunning)
            {
                idleRunning = true;

                Vector2 velocity = rigidBody.velocity;
                velocity.x = 0;
                rigidBody.velocity = velocity;
                stayTimer = Random.Range(stayTime1, stayTime2);
            }
            else
            {
                if (stayTimer > 0)
                {
                    stayTimer -= Time.fixedDeltaTime;
                    rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                    return;
                }
                else
                {
                    stayTimer = 0;
                    if (walkTimer == 0)
                    {
                        walkTimer = Random.Range(walkTime1, walkTime2);

                        int direction = Random.Range(-1, 1);
                        if (direction == -1)
                        {
                            states.facingDirection = -1;
                        }
                        else if (direction == 0)
                        {
                            states.facingDirection = 1;
                        }

                        FlipNPC();
                    }
                }

                if (walkTimer > 0)
                {
                    walkTimer -= Time.fixedDeltaTime;
                    rigidBody.velocity = new Vector2(data.moveSpeed * states.facingDirection, 0);
                }
                else
                {
                    walkTimer = 0;
                    idleRunning = false;
                }


            }
        }
        else
        {
            idleRunning = false;
        }



    }

    public void FlipNPC()
    {
        Vector2 npcScale = npc.transform.localScale;
        npcScale.x = states.facingDirection;
        npc.transform.localScale = npcScale;
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    if (states.isIdle)
    //    {
    //        if (!idleRunning)
    //        {
    //            StartCoroutine(IdleBehaviour());
    //        }
    //    }

    //    if (actionTimer <= 0)
    //    {
    //        actionTimer = 0;
    //    }

    //    //if (actionTimer > 0)
    //    //{
    //    //    actionTimer -= Time.fixedDeltaTime;
    //    //}
    //}

    //IEnumerator IdleBehaviour()
    //{
    //    idleRunning = true;
    //    Vector2 velocity = rigidBody.velocity;
    //    velocity.x = 0;
    //    rigidBody.velocity = velocity;

    //    float randomTime = Random.Range(5, 10);
    //    yield return new WaitForSecondsRealtime(randomTime);

    //    int randomDirection = Random.Range(-1, 2);
    //    if (randomDirection == -1)
    //    {
    //        states.facingDirection = randomDirection;
    //    }
    //    else if (randomDirection == 1)
    //    {
    //        states.facingDirection = randomDirection;
    //    }

    //    float randomWalkTime = Random.Range(3, 8);
    //    actionTimer = randomWalkTime;

    //    while (actionTimer > 0)
    //    {
    //        actionTimer -= Time.deltaTime;
    //        velocity.x = data.moveSpeed * states.facingDirection;
    //        rigidBody.velocity = velocity;
    //        if (actionTimer < 0.05f)
    //        {
    //            break;
    //        }
    //    }
    //    idleRunning = false;
    //}
}
