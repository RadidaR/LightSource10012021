using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekTargetScript : MonoBehaviour
{
    public NPCStats npcStats;
    public LayerMask targetLayers;

    public Collider2D[] targetsInSight = new Collider2D[10];
    public GameObject currentTarget;
    public float visionRange;
    public bool visionExpanded = false;
    //public float visionExpansion;
    //public static int OverlapCircle(Vector2 point, float radius, ContactFilter2D contactFilter, Collider2D[] results);

    // Start is called before the first frame update
    void OnValidate()
    {
        npcStats = GetComponent<NPCStatsScript>().npcStats;
        visionRange = npcStats.visionRange;
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

    void LoseTarget()
    {
        //Debug.Log("Target Lost");
        currentTarget = null;
        visionExpanded = false;
        visionRange = npcStats.visionRange;
    }

    void ExpandVision()
    {
        if (!visionExpanded)
        {
            visionRange += npcStats.visionExpansion;
            visionExpanded = true;
        }
    }
}
