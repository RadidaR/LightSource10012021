using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekTargetScript : MonoBehaviour
{
    public NPCStats npcStats;
    public LayerMask targetLayers;

    public Collider2D[] targetsInSight = new Collider2D[10];
    public GameObject currentTarget;
    //public static int OverlapCircle(Vector2 point, float radius, ContactFilter2D contactFilter, Collider2D[] results);

    // Start is called before the first frame update
    void OnValidate()
    {
        npcStats = GetComponent<NPCStatsScript>().npcStats;
    }

    // Update is called once per frame
    void Update()
    {
        targetsInSight = Physics2D.OverlapCircleAll(gameObject.transform.position, npcStats.visionRange, targetLayers);

        if (targetsInSight != null)
        {
            PrintName();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (npcStats.visionRange == 0)
        {
            return;
        }

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, npcStats.visionRange);
    }

    void PrintName()
    {
        for (int i = 0; i < targetsInSight.Length; i++)
        {
            GameObject checkTarget = targetsInSight[i].GetComponentInParent<OfInterest>().gameObject;
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

            //if (targetsInSight[i].gameObject.tag == "Light")
            //{
            //    currentTarget = targetsInSight[i].gameObject;
            //}
        }
    }
}
