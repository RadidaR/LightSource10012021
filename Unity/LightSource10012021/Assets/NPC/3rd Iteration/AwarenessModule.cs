using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwarenessModule : MonoBehaviour
{
    public NPCScript npc;
    public NPCType type;
    public Rigidbody2D body;

    public LayerMask groundLayer;

    public Collider2D[] interestsInSight;
    public LayerMask interestLayers;
    public float currentVision;
    public Transform eyeLevel;

    public bool seesLight;


    private void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            npc = GetComponentInParent<NPCScript>();
            type = npc.type;
            body = npc.gameObject.GetComponentInChildren<Rigidbody2D>();

            eyeLevel = npc.GetNamedChild(npc.gameObject, "EyeLevel").transform;
        }
    }

    private void Update()
    {
        if (npc.isIdle)
        {
            currentVision = type.idleVision;
        }
        else
        {
            currentVision = type.focusedVision;
        }

        interestsInSight = Physics2D.OverlapCircleAll(body.gameObject.transform.position, currentVision, interestLayers);

        if (interestsInSight.Length != 0)
        {
            CheckInterests(interestsInSight);
        }
        else
        {
            seesLight = false;
        }
    }

    private void CheckInterests(Collider2D[] interests)
    {
        foreach (Collider2D interest in interests)
        {
            if (interest.gameObject.tag == "Light")
            {
                List<Transform> rayEnds = new List<Transform>();
                List<float> distances = new List<float>();


                for (int i = 0; i < interest.gameObject.transform.childCount; i++)
                {
                    rayEnds.Add(interest.gameObject.transform.GetChild(i).transform);
                    distances.Add(Vector2.Distance(transform.position, rayEnds[i].position));
                }

                for (int j = 0; j < interest.gameObject.transform.childCount / 3; j++)
                {
                    for (int i = 0; i < distances.Count; i++)
                    {
                        if (distances[i] == Mathf.Max(distances.ToArray()))
                        {
                            rayEnds.Remove(rayEnds[i]);
                            distances.Remove(distances[i]);
                        }
                    }
                }
                int lightInSight = 0;
                foreach (Transform rayEnd in rayEnds)
                {
                    Vector3 first = rayEnd.GetChild(0).position;
                    Vector3 second = rayEnd.GetChild(1).position;

                    RaycastHit2D obstacleE = Physics2D.Raycast(eyeLevel.position, rayEnd.position - eyeLevel.position, Vector2.Distance(rayEnd.position, eyeLevel.position), groundLayer);
                    RaycastHit2D obstacleM1 = Physics2D.Raycast(eyeLevel.position, first - eyeLevel.position, Vector2.Distance(first, eyeLevel.position), groundLayer);
                    RaycastHit2D obstacleM2 = Physics2D.Raycast(eyeLevel.position, second - eyeLevel.position, Vector2.Distance(second, eyeLevel.position), groundLayer);
                    if (!obstacleE || !obstacleM1 || !obstacleM2)
                    {
                        lightInSight++;
                    }
                }

                if (lightInSight == 0)
                {
                    seesLight = false;
                }
                else
                {
                    seesLight = true;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (currentVision != 0)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(body.gameObject.transform.position, currentVision);
        }

        if (interestsInSight.Length != 0)
        {
            foreach (Collider2D interest in interestsInSight)
            {
                if (interest.gameObject.tag == "Light")
                {
                    List<Transform> rayEnds = new List<Transform>();
                    List<float> distances = new List<float>();


                    for (int i = 0; i < interest.gameObject.transform.childCount; i++)
                    {
                        rayEnds.Add(interest.gameObject.transform.GetChild(i).transform);
                        distances.Add(Vector2.Distance(transform.position, rayEnds[i].position));
                    }

                    for (int j = 0; j < interest.gameObject.transform.childCount / 3; j++)
                    {
                        for (int i = 0; i < distances.Count; i++)
                        {
                            if (distances[i] == Mathf.Max(distances.ToArray()))
                            {
                                rayEnds.Remove(rayEnds[i]);
                                distances.Remove(distances[i]);
                            }
                        }
                    }

                    foreach (Transform rayEnd in rayEnds)
                    {
                        Gizmos.color = Color.red;

                        RaycastHit2D obstacleE = Physics2D.Raycast(eyeLevel.position, rayEnd.position - eyeLevel.position, Vector2.Distance(rayEnd.position, eyeLevel.position), groundLayer);
                        if (!obstacleE)
                        {
                            Gizmos.DrawRay(eyeLevel.position, rayEnd.position - eyeLevel.position);
                        }
                        else
                        {
                            Gizmos.DrawRay(eyeLevel.position, new Vector3(obstacleE.point.x, obstacleE.point.y, 0) - eyeLevel.position);
                        }

                        Vector3 first = rayEnd.GetChild(0).position;
                        Vector3 second = rayEnd.GetChild(1).position;

                        Gizmos.color = Color.yellow;

                        RaycastHit2D obstacleM1 = Physics2D.Raycast(eyeLevel.position, first - eyeLevel.position, Vector2.Distance(first, eyeLevel.position), groundLayer);
                        if (!obstacleM1)
                        {
                            Gizmos.DrawRay(eyeLevel.position, first - eyeLevel.position);
                        }
                        else
                        {
                            Gizmos.DrawRay(eyeLevel.position, new Vector3(obstacleM1.point.x, obstacleM1.point.y, 0) - eyeLevel.position);
                        }

                        RaycastHit2D obstacleM2 = Physics2D.Raycast(eyeLevel.position, second - eyeLevel.position, Vector2.Distance(second, eyeLevel.position), groundLayer);
                        if (!obstacleM2)
                        {
                            Gizmos.DrawRay(eyeLevel.position, second - eyeLevel.position);
                        }
                        else
                        {
                            Gizmos.DrawRay(eyeLevel.position, new Vector3(obstacleM2.point.x, obstacleM2.point.y, 0) - eyeLevel.position);
                        }

                    }
                }
            }
        }
    }
}
