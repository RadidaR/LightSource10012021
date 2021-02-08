using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTargetScript : MonoBehaviour
{
    [SerializeField] NPCStatsData npcStatsData;
    [SerializeField] SeekTargetScript seekTarget;
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] int directionToTarget;
    // Start is called before the first frame update
    void Start()
    {
        npcStatsData = GetComponent<NPCStatsScript>().npcStatsData;
        seekTarget = GetComponent<SeekTargetScript>();
        rigidBody = GetComponent<Rigidbody2D>();
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

               Debug.Log(directionToTarget.ToString());
            rigidBody.AddForce(new Vector2(npcStatsData.movementSpeed * directionToTarget * Time.deltaTime, 0));
        }
    }
}
