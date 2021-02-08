using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekTargetScript : MonoBehaviour
{
    public NPCStatsData npcStatsData;
    public LayerMask targetLayers;

    public Collider2D[] targetsInSight = new Collider2D[10];
    public GameObject currentTarget;
    public float visionRange;
    public bool visionExpanded = false;

    void Awake()
    {
        npcStatsData = GetComponent<NPCStatsScript>().npcStatsData;
        visionRange = npcStatsData.visionRange;
    }

    void Update()
    {
        //GATHERS ALL TARGETS WITHIN VISION RANGE
        targetsInSight = Physics2D.OverlapCircleAll(gameObject.transform.position, visionRange, targetLayers);

        //IF THERE'S ANY TARGETS IN SIGHT - PICK ONE
        if (targetsInSight != null)
        {
            FindTarget();
        }

        //IF NO TARGETS IN SIGHT - LOSE TARGET
        if (targetsInSight.Length == 0)
        {
            LoseTarget();
        }

        //EXPAND VISION RANGE WHEN YOU HAVE A TARGET
        if (currentTarget != null)
        {
            ExpandVision();
        }
    }

    void OnDrawGizmos()
    {
        if (visionRange == 0)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }

    void FindTarget()
    {
        //CHECK TARGETS IN SIGHT
        for (int i = 0; i < targetsInSight.Length; i++)
        {
            //STORE TARGET BEING CHECKED IN VARIABLE
            GameObject checkTarget = targetsInSight[i].GetComponentInParent<OfInterest>().gameObject;
            //IF THERE IS NO TARGET AT THE MOMENT
            if (currentTarget == null)
            {
                //IF CHECKED TARGET HAS TAG OF PLAYER
                if (checkTarget.tag == "Player")
                {
                    //ASSIGN PLAYER AS NEW TARGET
                    currentTarget = checkTarget;
                    return;
                }
                //ELSE IF TAG IS LIGHT
                else if (checkTarget.tag == "Light")
                {
                    //PICK IT
                    currentTarget = checkTarget;
                    return;
                }
            }
            //IF THERE IS A TARGET AT THE MOMENT
            else if (currentTarget != null)
            {
                //CHECK IF THE CHECKED ONE HAS PLAYER TAG
                if (checkTarget.tag == "Player")
                {
                    //AND CHECK IF CURRENT TARGET HAS PLAYER TAG
                    if (currentTarget.tag == "Player")
                    {
                        //IF SO - RETURN
                        return;
                    }
                    //ELSE IF CURRENT TARGET ISN'T PLAYER
                    else
                    {
                        //MAKE PLAYER THE TARGET
                        currentTarget = checkTarget;
                        return;
                    }
                }
                //ELSE IF CHECKED TARGET HAS LIGHT TAG
                else if (checkTarget.tag == "Light")
                {
                    //CHECK IF CURRENT TARGET HAS PLAYER TAG
                    if (currentTarget.tag == "Player")
                    {
                        //CYCLE THROUGH TARGETS IN VISION RANGE
                        for (int k = 0; k < targetsInSight.Length; k++)
                        {
                            //TO CHECK IF ONE OF THEM IS THE PLAYER
                            if (targetsInSight[k].GetComponentInParent<OfInterest>().gameObject == currentTarget)
                            {
                                //IF SO - RETURN
                                return;
                            }
                            //IF NOT 
                            else
                            {
                                //CHECK IF THE PLAYER'S LIGHT IS AT A FURTHER DISTANCE THAN VISION RANGE
                                if (Vector2.Distance(gameObject.transform.position, currentTarget.gameObject.transform.position) - currentTarget.GetComponentInChildren<CircleCollider2D>().radius > visionRange)
                                {
                                    //IF SO - LOSE TARGET
                                    LoseTarget();
                                    return;
                                }
                            }
                        }
                    }
                    //IF CURRENT TARGET ISN'T PLAYER && THE CHECKED TARGET ISN'T THE SAME AS THE CURRENT ONE
                    else if (checkTarget != currentTarget)
                    {
                        //CALCULATE DISTANCES TO CURRENT AND CHECKED TARGETS
                        float distanceToTarget = Vector2.Distance(gameObject.transform.position, currentTarget.gameObject.transform.position);
                        float distanceToCheckTarget = Vector2.Distance(gameObject.transform.position, checkTarget.gameObject.transform.position);
                        //IF DISTANCE TO TARGET IS GREATER THAN DISTANCE TO CHECKED TARGET
                        if (distanceToTarget > distanceToCheckTarget)
                        {
                            //CHANGE CURRENT TARGET TO CHECKED TARGET
                            currentTarget = checkTarget;
                            return;
                        }
                    }
                }
            }
        }
    }

    void LoseTarget()
    {
        //NO CURRENT TARGET
        currentTarget = null;
        //RESET VISION RANGE
        visionExpanded = false;
        visionRange = npcStatsData.visionRange;
    }

    void ExpandVision()
    {
        if (!visionExpanded)
        {
            visionRange += npcStatsData.visionExpansion;
            visionExpanded = true;
        }
    }
}
