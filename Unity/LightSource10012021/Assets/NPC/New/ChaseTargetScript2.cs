using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTargetScript2 : MonoBehaviour
{
    public GameObject npc;

    public NPCData data;
    public NPCAbilities abilities;

    public NPCStatesScript states;

    public SeekTargetScript2 seekTarget;

    public NPCAttackScript attacks;

    public NavMeshAgent2D navAgent;

    public Vector2 chasePosition;


    //[SerializeField] NPCStatsData npcStatsData;
    //[SerializeField] SeekTargetScript seekTarget;
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] int directionToTarget;


    //public float attackRange;
    // Start is called before the first frame update
    private void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            npc = GetComponentInParent<OfInterest>().gameObject;
            states = npc.GetComponent<NPCStatesScript>();

            data = states.data;
            abilities = states.abilities;

            seekTarget = npc.GetComponentInChildren<SeekTargetScript2>();
            attacks = npc.GetComponentInChildren<NPCAttackScript>();
            rigidBody = npc.GetComponent<Rigidbody2D>();

            if (abilities.canFly)
            {
                navAgent = npc.GetComponent<NavMeshAgent2D>();
            }
        }
    }

    private void Start()
    {
        if (navAgent != null)
        {
            navAgent.speed = data.flySpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {     
    }

    private void FixedUpdate()
    {
        if (seekTarget.currentTarget != null)
        {
            chasePosition = seekTarget.currentTarget.transform.position;

            if (abilities.canMove)
            {
                ChaseTarget();
            }
        }
        else if (seekTarget.lastKnownPosition != Vector2.zero)
        {
            chasePosition = seekTarget.lastKnownPosition;

            if (abilities.canMove)
            {
                GoToLastPosition();
            }
        }

        if (seekTarget.currentTarget == null && seekTarget.lastKnownPosition == Vector2.zero)
        {
            chasePosition = Vector2.zero;
            states.isChasing = false;
        }


        //if (seekTarget.currentTarget != null)
        //{
        //    states.isChasing = true;

        //    chasePosition = seekTarget.currentTarget.transform.position;

        //    if (abilities.canMove)
        //    {
        //        ChaseTarget();
        //    }
        //}
        //else
        //{
        //    if (seekTarget.lastKnownPosition != Vector2.zero)
        //    {
        //        chasePosition = seekTarget.lastKnownPosition;

        //        if (abilities.canMove)
        //        {
        //            ChaseTarget();
        //        }
        //    }
        //    else
        //    {
        //        states.isChasing = false;
        //    }
        //}

        if (chasePosition != Vector2.zero)
        {
            if (gameObject.transform.position.x - chasePosition.x < 0)
            {
                directionToTarget = 1;
            }
            if (gameObject.transform.position.x - chasePosition.x > 0)
            {
                directionToTarget = -1;
            }

            //Debug.Log("FlipNPC");

            FlipNPC();
        }
    }

    public void FlipNPC()
    {
        Vector2 npcScale = npc.transform.localScale;
        npcScale.x = directionToTarget;
        npc.transform.localScale = npcScale;
    }

    void ChaseTarget()
    {
        if (!abilities.canFly)
        {
            //if not attacking
            if (Mathf.Abs(npc.transform.position.x - chasePosition.x) > attacks.currentAttackRange)
            {
                states.isChasing = true;
                Vector2 velocity = rigidBody.velocity;
                velocity.x = data.runSpeed * directionToTarget;
                rigidBody.velocity = velocity;
            }
            //else - attack            
        }

        //if (!abilities.canFly)
        //{
        //    if (Mathf.Abs(npc.transform.position.x - chasePosition.x) > attacks.currentAttackRange)
        //    {
        //        Vector2 velocity = rigidBody.velocity;
        //        velocity.x = data.moveSpeed * directionToTarget;
        //        rigidBody.velocity = velocity;
        //    }
        //    else
        //    {
        //        chasePosition = Vector2.zero;
        //    }
        //}
        //else if (abilities.canFly)
        //{
        //    if (Vector2.Distance(npc.transform.position, chasePosition) > attacks.currentAttackRange)
        //    {
        //        if (!states.isHurt)
        //        {
        //            if (seekTarget.currentTarget != null && seekTarget.currentTarget.gameObject.tag == "Player")
        //            {
        //                Vector2 targetPosition = chasePosition;
        //                targetPosition.y += 4f;
        //                navAgent.destination = targetPosition;
        //            }
        //            else
        //            {
        //                navAgent.destination = chasePosition;
        //            }
        //        }
        //        else
        //        {
        //            navAgent.destination = npc.transform.position;
        //        }
        //    }
        //}
    }

    public void GoToLastPosition()
    {
        if (!abilities.canFly)
        {
            if (Mathf.Abs(npc.transform.position.x - chasePosition.x) > attacks.currentAttackRange)
            {
                states.isChasing = true;
                Vector2 velocity = rigidBody.velocity;
                velocity.x = data.moveSpeed * directionToTarget;
                rigidBody.velocity = velocity;
            }
        }
    }


}
