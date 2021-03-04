using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAwarenessScript : MonoBehaviour
{
    public GameObject npc;
    public NPCStatesScript states;

    public NPCData data;
    public NPCAbilities abilities;

    public NavMeshAgent2D navAgent;

    public LayerMask groundLayer;

    public GameObject target1;

    public bool seesTop;
    public bool seesTop1;
    public bool seesTop2;
    public bool seesTop3;
    public bool seesMid;
    public bool seesBot1;
    public bool seesBot2;
    public bool seesBot3;
    public bool seesBot;
    // Start is called before the first frame update

    public void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            npc = GetComponentInParent<OfInterest>().gameObject;

            states = npc.GetComponent<NPCStatesScript>();

            data = states.data;
            abilities = states.abilities;
                        
            if (abilities.canFly)
            {
                navAgent = npc.GetComponent<NavMeshAgent2D>();
            }                        
        }
    }



    public bool targetInSight(GameObject target, Vector2 position)
    {

        Vector2 eyeLevel = GetNamedChild(npc, "EyeLevel").transform.position;

        if (target != null)
        {
            target1 = target;

            Vector2 targetTop = GetNamedChild(target, "Top").transform.position;
            Vector2 targetMid = GetNamedChild(target, "Mid").transform.position;
            Vector2 targetBot = GetNamedChild(target, "Bot").transform.position;

            float topHeight = targetTop.y - targetMid.y;
            float botHeight = targetMid.y - targetBot.y;

            seesTop = !Physics2D.Raycast(eyeLevel, targetTop - eyeLevel, Vector2.Distance(eyeLevel, targetTop), groundLayer);

            seesTop3 = !Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.75f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y + topHeight * 0.75f)), groundLayer);
            seesTop2 = !Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.5f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y + topHeight * 0.5f)), groundLayer);
            seesTop1 = !Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.25f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y + topHeight * 0.25f)), groundLayer);

            seesMid = !Physics2D.Raycast(eyeLevel, targetMid - eyeLevel, Vector2.Distance(eyeLevel, targetMid), groundLayer);

            seesBot1 = !Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.25f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y - botHeight * 0.25f)), groundLayer);
            seesBot2 = !Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.5f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y - botHeight * 0.5f)), groundLayer);
            seesBot3 = !Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.75f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y - botHeight * 0.75f)), groundLayer);

            seesBot = !Physics2D.Raycast(eyeLevel, targetBot - eyeLevel, Vector2.Distance(eyeLevel, targetBot), groundLayer);

            if (seesTop || seesTop3 || seesTop2 || seesTop1 || seesMid || seesBot1 || seesBot2 || seesBot3 || seesBot)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            RaycastHit2D obstaclesToTarget = Physics2D.Raycast(eyeLevel, position - eyeLevel, Vector2.Distance(eyeLevel, position), groundLayer);

            if (!obstaclesToTarget)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public GameObject GetNamedChild(GameObject parentObject, string childName)
    {
        GameObject childObject = parentObject;

        if (parentObject.transform.childCount != 0)
        {
            foreach (Transform childT1 in parentObject.transform)
            {
                if (childT1.name == childName)
                {
                    childObject = childT1.gameObject;
                }
                else if (childT1.childCount != 0)
                {
                    foreach (Transform subChildT2 in childT1.transform)
                    {
                        if (subChildT2.name == childName)
                        {
                            childObject = subChildT2.gameObject;
                        }
                        else if (subChildT2.childCount != 0)
                        {
                            foreach (Transform subChildT3 in subChildT2.transform)
                            {
                                if (subChildT3.name == childName)
                                {
                                    childObject = subChildT3.gameObject;
                                }
                                else if (subChildT3.childCount != 0)
                                {
                                    foreach (Transform subChildT4 in subChildT3.transform)
                                    {
                                        if (subChildT4.name == childName)
                                        {
                                            childObject = subChildT4.gameObject;
                                        }
                                        else if (subChildT4.childCount != 0)
                                        {
                                            foreach (Transform subChildT5 in subChildT4.transform)
                                            {
                                                if (subChildT5.name == childName)
                                                {
                                                    childObject = subChildT5.gameObject;
                                                }
                                                else if (subChildT5.childCount != 0)
                                                {
                                                    foreach (Transform subChildT6 in subChildT5.transform)
                                                    {
                                                        if (subChildT6.name == childName)
                                                        {
                                                            childObject = subChildT6.gameObject;
                                                        }
                                                        else if (subChildT6.childCount != 0)
                                                        {
                                                            foreach (Transform subChildT7 in subChildT6.transform)
                                                            {
                                                                if (subChildT7.name == childName)
                                                                {
                                                                    childObject = subChildT7.gameObject;
                                                                }
                                                                else if (subChildT7.childCount != 0)
                                                                {
                                                                    foreach (Transform subChildT8 in subChildT7.transform)
                                                                    {
                                                                        if (subChildT8.name == childName)
                                                                        {
                                                                            childObject = subChildT8.gameObject;
                                                                        }
                                                                        else if (subChildT8.childCount != 0)
                                                                        {
                                                                            foreach (Transform subChildT9 in subChildT8.transform)
                                                                            {
                                                                                if (subChildT9.name == childName)
                                                                                {
                                                                                    childObject = subChildT9.gameObject;
                                                                                }
                                                                                else if (subChildT9.childCount != 0)
                                                                                {
                                                                                    foreach (Transform subChildT10 in subChildT9.transform)
                                                                                    {
                                                                                        if (subChildT10.name == childName)
                                                                                        {
                                                                                            childObject = subChildT10.gameObject;
                                                                                        }
                                                                                        else if (subChildT10.childCount != 0)
                                                                                        {
                                                                                            foreach (Transform subChildT11 in subChildT10.transform)
                                                                                            {
                                                                                                if (subChildT11.name == childName)
                                                                                                {
                                                                                                    childObject = subChildT11.gameObject;
                                                                                                }
                                                                                                else if (subChildT11.childCount != 0)
                                                                                                {
                                                                                                    foreach (Transform subChildT12 in subChildT11.transform)
                                                                                                    {
                                                                                                        if (subChildT12.name == childName)
                                                                                                        {
                                                                                                            childObject = subChildT12.gameObject;
                                                                                                        }
                                                                                                        else if (subChildT12.childCount != 0)
                                                                                                        {
                                                                                                            foreach (Transform subChildT13 in subChildT12.transform)
                                                                                                            {
                                                                                                                if (subChildT13.name == childName)
                                                                                                                {
                                                                                                                    childObject = subChildT13.gameObject;
                                                                                                                }
                                                                                                                else if (subChildT13.childCount != 0)
                                                                                                                {
                                                                                                                    foreach (Transform subChildT14 in subChildT13.transform)
                                                                                                                    {
                                                                                                                        if (subChildT14.name == childName)
                                                                                                                        {
                                                                                                                            childObject = subChildT14.gameObject;
                                                                                                                        }
                                                                                                                        else if (subChildT14.childCount != 0)
                                                                                                                        {
                                                                                                                            foreach (Transform subChildT15 in subChildT14.transform)
                                                                                                                            {
                                                                                                                                if (subChildT15.name == childName)
                                                                                                                                {
                                                                                                                                    childObject = subChildT15.gameObject;
                                                                                                                                }
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //for (int i = 0; i < parentObject.transform.childCount; i++)
        //{
        //    if (parentObject.transform.GetChild(i).gameObject.name == childName)
        //    {
        //        childObject = parentObject.transform.GetChild(i).gameObject;
        //        return childObject;
        //    }
        //    else if (parentObject.transform.GetChild(i).childCount != 0)
        //    {
        //        for (int j = 0; j < parentObject.transform.GetChild(i).childCount; j++)
        //        {
        //            if (parentObject.transform.GetChild(i).transform.GetChild(j).gameObject.name == childName)
        //            {
        //                childObject = parentObject.transform.GetChild(i).transform.GetChild(j).gameObject;
        //                return childObject;
        //            }
        //            else if (parentObject.transform.GetChild(i).transform.GetChild(j).childCount != 0)
        //            {
        //                for (int k = 0; k < parentObject.transform.GetChild(i).transform.GetChild(j).childCount; k++)
        //                {
        //                    if (parentObject.transform.GetChild(i).transform.GetChild(j).transform.GetChild(k).gameObject.name == childName)
        //                    {
        //                        childObject = parentObject.transform.GetChild(i).transform.GetChild(j).transform.GetChild(k).gameObject;
        //                        return childObject;
        //                    }
        //                    else if (parentObject.transform.GetChild(i).transform.GetChild(j).transform.GetChild(k).childCount != 0)
        //                    {
        //                        for (int l = 0; l < parentObject.transform.GetChild(i).transform.GetChild(j).transform.GetChild(k).childCount; l++)
        //                        {
        //                            if (parentObject.transform.GetChild(i).transform.GetChild(j).transform.GetChild(k).transform.GetChild(l).gameObject.name == childName)
        //                            {
        //                                childObject = parentObject.transform.GetChild(i).transform.GetChild(j).transform.GetChild(k).transform.GetChild(l).gameObject;
        //                                return childObject;
        //                            }
        //                            else if (parentObject.transform.GetChild(i).transform.GetChild(j).transform.GetChild(k).transform.GetChild(l).childCount != 0)
        //                            {
        //                                for (int o = 0; o < parentObject.transform.GetChild(i).transform.GetChild(j).transform.GetChild(k).transform.GetChild(l).childCount; o++)
        //                                {
        //                                    if (parentObject.transform.GetChild(i).transform.GetChild(j).transform.GetChild(k).transform.GetChild(l).transform.GetChild(o).gameObject.name == childName)
        //                                    {
        //                                        childObject = parentObject.transform.GetChild(i).transform.GetChild(j).transform.GetChild(k).transform.GetChild(l).transform.GetChild(o).gameObject;
        //                                        return childObject;
        //                                    }
        //                                    else if (parentObject.transform.GetChild(i).transform.GetChild(j).transform.GetChild(k).transform.GetChild(l).transform.GetChild(o).childCount != 0)
        //                                    {
        //                                        for (int p = 0; p < parentObject.transform.GetChild(i).transform.GetChild(j).transform.GetChild(k).transform.GetChild(l).transform.GetChild(o).childCount; p++)
        //                                        {
        //                                            if (parentObject.transform.GetChild(i).transform.GetChild(j).transform.GetChild(k).transform.GetChild(l).transform.GetChild(o).transform.GetChild(p).gameObject.name == childName)
        //                                            {
        //                                                childObject = parentObject.transform.GetChild(i).transform.GetChild(j).transform.GetChild(k).transform.GetChild(l).transform.GetChild(o).transform.GetChild(p).gameObject;
        //                                                return childObject;
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //        }
        //    }
        //}
        return childObject;
    }

    private void OnDrawGizmos()
    {
        //if (target1 != null)
        //{
        //    Vector2 eyeLevel = GetNamedChild(npc, "EyeLevel").transform.position;

        //    Vector2 targetTop = GetNamedChild(target1, "Top").transform.position;
        //    Vector2 targetMid = GetNamedChild(target1, "Mid").transform.position;
        //    Vector2 targetBot = GetNamedChild(target1, "Bot").transform.position;

        //    float topHeight = targetTop.y - targetMid.y;
        //    float botHeight = targetMid.y - targetBot.y;

        //    Gizmos.color = Color.blue;

        //    if (targetInSight(target1, target1.transform.position))
        //    {
        //        if (seesTop)
        //        {
        //            Gizmos.DrawRay(eyeLevel, targetTop - eyeLevel);
        //        }
        //        else
        //        {
        //            RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, targetTop - eyeLevel, Vector2.Distance(eyeLevel, targetTop), groundLayer);
        //            Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
        //        }

        //        if (seesTop3)
        //        {
        //            Gizmos.DrawRay(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.75f) - eyeLevel));
        //        }
        //        else
        //        {
        //            RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.75f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y + topHeight * 0.75f)), groundLayer);
        //            Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
        //        }

        //        if (seesTop2)
        //        {
        //            Gizmos.DrawRay(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.5f) - eyeLevel));
        //        }
        //        else
        //        {
        //            RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.5f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y + topHeight * 0.5f)), groundLayer);
        //            Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
        //        }

        //        if (seesTop1)
        //        {
        //            Gizmos.DrawRay(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.25f) - eyeLevel));
        //        }
        //        else
        //        {
        //            RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.25f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y + topHeight * 0.25f)), groundLayer);
        //            Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
        //        }

        //        if (seesMid)
        //        {
        //            Gizmos.DrawRay(eyeLevel, targetMid - eyeLevel);
        //        }
        //        else
        //        {
        //            RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, targetMid - eyeLevel, Vector2.Distance(eyeLevel, targetMid), groundLayer);
        //            Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
        //        }

        //        if (seesBot1)
        //        {
        //            Gizmos.DrawRay(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.25f) - eyeLevel));

        //        }
        //        else
        //        {
        //            RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.25f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y - botHeight * 0.25f)), groundLayer);
        //            Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
        //        }

        //        if (seesBot2)
        //        {
        //            Gizmos.DrawRay(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.5f) - eyeLevel));
        //        }
        //        else
        //        {
        //            RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.5f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y - botHeight * 0.5f)), groundLayer);
        //            Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
        //        }

        //        if (seesBot3)
        //        {
        //            Gizmos.DrawRay(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.75f) - eyeLevel));
        //        }
        //        else
        //        {
        //            RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.75f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y - botHeight * 0.75f)), groundLayer);
        //            Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
        //        }

        //        if (seesBot)
        //        {
        //            Gizmos.DrawRay(eyeLevel, targetBot - eyeLevel);
        //        }
        //        else
        //        {
        //            RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, targetBot - eyeLevel, Vector2.Distance(eyeLevel, targetBot), groundLayer);
        //            Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
        //        }
        //    }
        //}
    }
}
