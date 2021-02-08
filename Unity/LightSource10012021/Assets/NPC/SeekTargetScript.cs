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
    //public float visionExpansion;
    //public static int OverlapCircle(Vector2 point, float radius, ContactFilter2D contactFilter, Collider2D[] results);

    // Start is called before the first frame update
    void Awake()
    {
        npcStatsData = GetComponent<NPCStatsScript>().npcStatsData;
        visionRange = npcStatsData.visionRange;
    }

    // Update is called once per frame
    void Update()
    {
        targetsInSight = Physics2D.OverlapCircleAll(gameObject.transform.position, visionRange, targetLayers);

        if (targetsInSight != null)
        {
            FindTarget();
        }

        if (targetsInSight.Length == 0)
        {
            LoseTarget();
        }

        if (currentTarget != null)
        {
            ExpandVision();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (visionRange == 0)
        {
            return;
        }

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }

    void FindTarget()
    {
        for (int i = 0; i < targetsInSight.Length; i++)
        {
            GameObject checkTarget = targetsInSight[i].GetComponentInParent<OfInterest>().gameObject;
            if (currentTarget == null)
            {
                if (checkTarget.tag == "Player")
                {
                    currentTarget = checkTarget;
                    return;
                }
                else if (checkTarget.tag == "Light")
                {
                    currentTarget = checkTarget;
                    return;
                }
            }
            else if (currentTarget != null)
            {
                if (checkTarget.tag == "Player")
                {
                    if (currentTarget.tag == "Player")
                    {
                        return;
                    }
                    else
                    {
                        currentTarget = checkTarget;
                        return;
                    }
                }
                else if (checkTarget.tag == "Light")
                {
                    if (currentTarget.tag == "Player")
                    {
                        for (int k = 0; k < targetsInSight.Length; k++)
                        {
                            if (targetsInSight[k].GetComponentInParent<OfInterest>().gameObject == currentTarget)
                            {
                                return;
                            }
                            else
                            {
                                if (Vector2.Distance(gameObject.transform.position, currentTarget.gameObject.transform.position) - currentTarget.GetComponentInChildren<CircleCollider2D>().radius > visionRange)
                                {
                                    LoseTarget();
                                    return;
                                }
                            }
                        }
                    }
                    else if (checkTarget != currentTarget)
                    {
                        float distanceToTarget = Vector2.Distance(gameObject.transform.position, currentTarget.gameObject.transform.position);
                        //Debug.Log("Distance to current target is " + distanceToTarget.ToString());
                        float distanceToCheckTarget = Vector2.Distance(gameObject.transform.position, checkTarget.gameObject.transform.position);
                        //Debug.Log("Distance to target being check is " + distanceToCheckTarget.ToString());
                        if (distanceToTarget > distanceToCheckTarget)
                        {
                            currentTarget = checkTarget;
                            return;
                        }
                    }
                }
            }

            //if (targetsInSight[i].gameObject.tag == "Light")
            //{
            //    currentTarget = targetsInSight[i].gameObject;
            //}
        }
    }

    void LoseTarget()
    {
        //Debug.Log("Target Lost");
        currentTarget = null;
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
