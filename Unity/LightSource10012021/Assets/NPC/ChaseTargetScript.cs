using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTargetScript : MonoBehaviour
{
    [SerializeField] NPCStatsData npcStatsData;
    [SerializeField] SeekTargetScript seekTarget;
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] int directionToTarget;

    public float attackRange;
    // Start is called before the first frame update
    private void OnValidate()
    {
        npcStatsData = GetComponent<NPCStatsScript>().npcStatsData;
        seekTarget = GetComponent<SeekTargetScript>();
        rigidBody = GetComponent<Rigidbody2D>();
        attackRange = npcStatsData.attackRange;
    }

    // Update is called once per frame
    void Update()
    {        
        if (seekTarget.currentTarget != null)
        {
            if (gameObject.transform.position.x - seekTarget.currentTarget.transform.position.x < 0)
            {
                directionToTarget = 1;
            }
            if (gameObject.transform.position.x - seekTarget.currentTarget.transform.position.x > 0)
            {
                directionToTarget = -1;
            }
            FlipObject();

            if (!npcStatsData.canFly)
            {
                ChaseTarget();
            }
            //rigidBody.AddForce(new Vector2(npcStatsData.movementSpeed * directionToTarget * Time.deltaTime, 0));
            
            //rigidBody.AddForce(new Vector2(Vector2.Distance(gameObject.transform.position, seekTarget.currentTarget.transform.position), 0));
            //rigidBody.MovePosition(new Vector2(Mathf.Lerp(gameObject.transform.position.x, seekTarget.currentTarget.transform.position.x, 10), Mathf.Lerp(gameObject.transform.position.y, seekTarget.currentTarget.transform.position.y, 10)));
        }
    }

    public void FlipObject()
    {
        //playerMovementData.facingDirection = -1;
        Vector2 npcScale = gameObject.transform.localScale;
        npcScale.x = directionToTarget;
        gameObject.transform.localScale = npcScale;
    }

    void ChaseTarget()
    {
        if (Vector2.Distance(transform.position, seekTarget.currentTarget.transform.position) > attackRange)
        {
            //Debug.Log("Chase target");  
            //rigidBody.AddForce(new Vector2(npcStatsData.movementSpeed * directionToTarget * Time.deltaTime, 0));
            Vector2 velocity = rigidBody.velocity;
            velocity.x = npcStatsData.movementSpeed * directionToTarget;
            rigidBody.velocity = velocity;
        }
    }

    void OnDrawGizmos()
    {
        if (attackRange == 0)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
