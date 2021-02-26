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
    bool seesTop1;
    bool seesTop2;
    bool seesTop3;
    bool seesMid;
    bool seesBot1;
    bool seesBot2;
    bool seesBot3;
    bool seesBot;
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
            for (int i = 0; i < parentObject.transform.childCount; i++)
            {
                if (parentObject.transform.GetChild(i).gameObject.name == childName)
                {
                    childObject = parentObject.transform.GetChild(i).gameObject;
                    return childObject;
                }
                else if (parentObject.transform.GetChild(i).childCount != 0)
                {
                    for (int j = 0; j < parentObject.transform.GetChild(i).childCount; j++)
                    {
                        if (parentObject.transform.GetChild(i).transform.GetChild(j).gameObject.name == childName)
                        {
                            childObject = parentObject.transform.GetChild(i).transform.GetChild(j).gameObject;
                            return childObject;
                        }
                        else if (parentObject.transform.GetChild(i).transform.GetChild(j).childCount != 0)
                        {
                            for (int k = 0; k < parentObject.transform.GetChild(i).transform.GetChild(j).childCount; k++)
                            {
                                if (parentObject.transform.GetChild(i).transform.GetChild(j).transform.GetChild(k).gameObject.name == childName)
                                {
                                    childObject = parentObject.transform.GetChild(i).transform.GetChild(j).transform.GetChild(k).gameObject;
                                    return childObject;
                                }
                            }
                        }
                    }
                }
            }
        }
        return childObject;
    }

    private void OnDrawGizmos()
    {
        if (target1 != null)
        {
            Vector2 eyeLevel = GetNamedChild(npc, "EyeLevel").transform.position;

            Vector2 targetTop = GetNamedChild(target1, "Top").transform.position;
            Vector2 targetMid = GetNamedChild(target1, "Mid").transform.position;
            Vector2 targetBot = GetNamedChild(target1, "Bot").transform.position;

            float topHeight = targetTop.y - targetMid.y;
            float botHeight = targetMid.y - targetBot.y;

            Gizmos.color = Color.blue;

            if (targetInSight(target1, target1.transform.position))
            {
                if (seesTop)
                {
                    Gizmos.DrawRay(eyeLevel, targetTop - eyeLevel);
                }
                else
                {
                    RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, targetTop - eyeLevel, Vector2.Distance(eyeLevel, targetTop), groundLayer);
                    Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                }

                if (seesTop3)
                {
                    Gizmos.DrawRay(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.75f) - eyeLevel));
                }
                else
                {
                    RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.75f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y + topHeight * 0.75f)), groundLayer);
                    Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                }

                if (seesTop2)
                {
                    Gizmos.DrawRay(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.5f) - eyeLevel));
                }
                else
                {
                    RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.5f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y + topHeight * 0.5f)), groundLayer);
                    Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                }

                if (seesTop1)
                {
                    Gizmos.DrawRay(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.25f) - eyeLevel));
                }
                else
                {
                    RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.25f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y + topHeight * 0.25f)), groundLayer);
                    Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                }

                if (seesMid)
                {
                    Gizmos.DrawRay(eyeLevel, targetMid - eyeLevel);
                }
                else
                {
                    RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, targetMid - eyeLevel, Vector2.Distance(eyeLevel, targetMid), groundLayer);
                    Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                }

                if (seesBot1)
                {
                    Gizmos.DrawRay(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.25f) - eyeLevel));

                }
                else
                {
                    RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.25f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y - botHeight * 0.25f)), groundLayer);
                    Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                }

                if (seesBot2)
                {
                    Gizmos.DrawRay(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.5f) - eyeLevel));
                }
                else
                {
                    RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.5f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y - botHeight * 0.5f)), groundLayer);
                    Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                }

                if (seesBot3)
                {
                    Gizmos.DrawRay(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.75f) - eyeLevel));
                }
                else
                {
                    RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.75f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y - botHeight * 0.75f)), groundLayer);
                    Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                }

                if (seesBot)
                {
                    Gizmos.DrawRay(eyeLevel, targetBot - eyeLevel);
                }
                else
                {
                    RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, targetBot - eyeLevel, Vector2.Distance(eyeLevel, targetBot), groundLayer);
                    Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                }
            }
        }
    }
}
