using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectLightScript : MonoBehaviour
{
    public GameObject npc;
    public NPCData data;
    public NPCAbilities abilities;
    public NPCAwarenessScript awareness;

    public bool seesLight;
    //public bool isInterested;
    public Collider2D[] interestsInSight;
    public LayerMask interestLayers;
    public LayerMask groundLayer;


    // Start is called before the first frame update
    void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            npc = GetComponentInParent<OfInterest>().gameObject;
            data = npc.GetComponent<NPCStatesScript>().data;
            abilities = npc.GetComponent<NPCStatesScript>().abilities;
            awareness = npc.GetComponentInChildren<NPCAwarenessScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        interestsInSight = Physics2D.OverlapCircleAll(transform.position, data.visionRange, interestLayers);

        //if (interestsInSight.Length == 0)
        //{
        //    seesLight = false;
        //}

        //for (int i = 0; i < interestsInSight.Length; i++)
        //{
        //    GameObject parent = interestsInSight[i].gameObject.transform.parent.gameObject;

        //    if (i > 0 && parent == interestsInSight[i - 1].gameObject.transform.parent.gameObject)
        //    {
        //        return;
        //    }

        //    foreach (Transform interest in parent.transform)
        //    {
        //        if (interest.gameObject.CompareTag("LightRayEnd"))
        //        {
        //            List<Transform> rayEnds = new List<Transform>();
        //            List<float> distances = new List<float>();


        //            for (int k = 0; k < interest.gameObject.transform.childCount; k++)
        //            {
        //                rayEnds.Add(interest.gameObject.transform.GetChild(k).transform);
        //                distances.Add(Vector2.Distance(transform.position, rayEnds[k].position));
        //            }

        //            for (int j = 0; j < interest.gameObject.transform.childCount / 2 - 1; j++)
        //            {
        //                for (int p = 0; p < distances.Count; p++)
        //                {
        //                    if (distances[p] == Mathf.Max(distances.ToArray())) /* && rayEnds.Count > interest.gameObject.transform.childCount / 2 + 1)*/
        //                    {

        //                        rayEnds.Remove(rayEnds[p]);
        //                        distances.Remove(distances[p]);
        //                    }
        //                }
        //            }

        //            int sight = 0;
        //            foreach (Transform rayEnd in rayEnds)
        //            {
        //                Transform first = rayEnd.GetChild(0);
        //                Transform second = rayEnd.GetChild(1);

        //                bool noObstacles = !Physics2D.Raycast(transform.position, rayEnd.position - transform.position, groundLayer) || !Physics2D.Raycast(transform.position, first.position - transform.position, groundLayer) || !Physics2D.Raycast(transform.position, second.position - transform.position, groundLayer);

        //                if (noObstacles)
        //                {
        //                    sight++;
        //                    seesLight = true;
        //                }

        //            }

        //            Debug.Log(rayEnds.Count.ToString());

        //            if (sight == 0)
        //            {
        //                Debug.Log("No sight");
        //                seesLight = false;
        //            }
        //        }
        //    }
        //}



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

                for (int j = 0; j < interest.gameObject.transform.childCount / 2 - 1; j++)
                {
                    for (int i = 0; i < distances.Count; i++)
                    {
                        if (distances[i] == Mathf.Max(distances.ToArray())) /* && rayEnds.Count > interest.gameObject.transform.childCount / 2 + 1)*/
                        {

                            rayEnds.Remove(rayEnds[i]);
                            distances.Remove(distances[i]);
                        }
                    }
                }

                int sight = 0;
                foreach (Transform rayEnd in rayEnds)
                {
                    Transform first = rayEnd.GetChild(0);
                    Transform second = rayEnd.GetChild(1);

                    bool noObstacles = !Physics2D.Raycast(transform.position, rayEnd.position - transform.position, groundLayer) || !Physics2D.Raycast(transform.position, first.position - transform.position, groundLayer) || !Physics2D.Raycast(transform.position, second.position - transform.position, groundLayer);

                    if (noObstacles)
                    {
                        sight++;
                        seesLight = true;
                    }

                }

                if (sight == 0)
                {
                    seesLight = false;
                }
            }
            //else
            //{
            //    Debug.Log("Not Light");
            //}
            //}







            ////foreach (Collider2D interest in interestsInSight)
            ////{
            ////    if (interest.gameObject.tag == "Light")
            ////    {
            ////        List<Transform> rayEnds = new List<Transform>();
            ////        //float[] distances = new float[interest.gameObject.transform.childCount];
            ////        List<float> distances = new List<float>();

            ////        List<Transform> closestRays = new List<Transform>();

            ////        //Transform[] closestRayEnd = new Transform[interest.gameObject.transform.childCount / 2 + 1];
            ////        //int posToDel;

            ////        for (int i = 0; i < interest.gameObject.transform.childCount; i++)
            ////        {
            ////            rayEnds.Add(interest.gameObject.transform.GetChild(i).transform);
            ////            distances.Add(Vector2.Distance(transform.position, rayEnds[i].position));


            ////            //rayEnds[i] = interest.gameObject.transform.GetChild(i).transform;
            ////            //distances[i] = Vector2.Distance(transform.position, rayEnds[i].position);
            ////            //closestRayEnd[i] = rayEnds[i];

            ////            int o = 0;
            ////            if (i == interest.gameObject.transform.childCount - 1)
            ////            {

            ////            }
            ////            //distances.Add(Vector2.Distance(transform.position, rayEnds[i].position));
            ////        }


            ////        for (int i = 0; i < interest.gameObject.transform.childCount / 2 + 1; i++)
            ////        {
            ////            if (distances[i] == Mathf.Min(distances.ToArray()) && rayEnds.Count > interest.gameObject.transform.childCount / 2 + 1)
            ////            {
            ////                closestRays.Add(rayEnds[i]);

            ////                rayEnds.Remove(rayEnds[i]);
            ////                distances.Remove(distances[i]);

            ////                i = -1;
            ////            }
            ////        }


            ////        //float closest = Mathf.Min(distances);

            ////        //float[] distances2 = new float[distances.Length - 1];
            ////        //for (int i = 0; i < rayEnds.Length; i++)
            ////        //{
            ////        //    if (distances[i] != closest)
            ////        //    {
            ////        //        distances2[i] = distances[i];                        
            ////        //    }
            ////        //}

            ////        //float closest2 = Mathf.Min(distances2);

            ////        //float[] distances3 = new float[distances2.Length - 1];
            ////        //for (int i = 0; i < rayEnds.Length; i++)
            ////        //{
            ////        //    if (distances2[i] != closest2)
            ////        //    {
            ////        //        distances3[i] = distances2[i];
            ////        //    }
            ////        //}

            ////        //float closest3 = Mathf.Min(distances3);

            ////        //interest.gameObject.transform.
            ////    }
            ////    //else
            ////    //{
            ////    //    Debug.Log("Not Light");
            ////    //}
            ////}
        }
    }

    private void OnDrawGizmos()
    {
        if (data != null)
        {
            Vector2 eyeLevel = awareness.GetNamedChild(npc, "EyeLevel").transform.position;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, data.visionRange);

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

                    for (int j = 0; j < interest.gameObject.transform.childCount / 2 - 1; j++)
                    {
                        for (int i = 0; i < distances.Count; i++)
                        {
                            if (distances[i] == Mathf.Max(distances.ToArray())) /* && rayEnds.Count > interest.gameObject.transform.childCount / 2 + 1)*/
                            {

                                rayEnds.Remove(rayEnds[i]);
                                distances.Remove(distances[i]);
                            }
                        }
                    }

                    foreach (Transform rayEnd in rayEnds)
                    {
                        Vector2 endPos = rayEnd.transform.position;

                        Transform first = rayEnd.GetChild(0);
                        Transform second = rayEnd.GetChild(1);

                        Vector2 firstPos = first.position;
                        Vector2 secondPos = second.position;


                        //Ray ray = new Ray(eyeLevel, endPos - eyeLevel);
                        if (seesLight)
                        {
                            Gizmos.color = Color.green;

                            if (!Physics2D.Raycast(eyeLevel, endPos - eyeLevel, groundLayer))
                            {
                                //Gizmos.DrawRay(ray);
                                Gizmos.DrawRay(eyeLevel, endPos - eyeLevel);
                            }
                            else
                            {
                                RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, endPos - eyeLevel, Vector2.Distance(eyeLevel, endPos), groundLayer);
                                Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                            }

                            Gizmos.color = Color.white;

                            if (!Physics2D.Raycast(eyeLevel, firstPos - eyeLevel, groundLayer))
                            {
                                Gizmos.DrawRay(eyeLevel, firstPos - eyeLevel);
                            }
                            else
                            {
                                RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, firstPos - eyeLevel, Vector2.Distance(eyeLevel, firstPos), groundLayer);
                                Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                            }

                            if (!Physics2D.Raycast(eyeLevel, secondPos - eyeLevel, groundLayer))
                            {
                                Gizmos.DrawRay(eyeLevel, secondPos - eyeLevel);
                            }
                            else
                            {
                                RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, secondPos - eyeLevel, Vector2.Distance(eyeLevel, secondPos), groundLayer);
                                Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                            }
                        }
                    }

                    GameObject source = interest.GetComponentInParent<OfInterest>().gameObject;

                    if (source != null)
                    {

                        Vector2 targetTop = awareness.GetNamedChild(source, "Top").transform.position;
                        Vector2 targetMid = awareness.GetNamedChild(source, "Mid").transform.position;
                        Vector2 targetBot = awareness.GetNamedChild(source, "Bot").transform.position;

                        float topHeight = targetTop.y - targetMid.y;
                        float botHeight = targetMid.y - targetBot.y;

                        Gizmos.color = Color.blue;

                        if (awareness.targetInSight(source, source.transform.position))
                        {
                            if (awareness.seesTop)
                            {
                                Gizmos.DrawRay(eyeLevel, targetTop - eyeLevel);
                            }
                            else
                            {
                                RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, targetTop - eyeLevel, Vector2.Distance(eyeLevel, targetTop), groundLayer);
                                Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                            }

                            if (awareness.seesTop3)
                            {
                                Gizmos.DrawRay(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.75f) - eyeLevel));
                            }
                            else
                            {
                                RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.75f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y + topHeight * 0.75f)), groundLayer);
                                Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                            }

                            if (awareness.seesTop2)
                            {
                                Gizmos.DrawRay(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.5f) - eyeLevel));
                            }
                            else
                            {
                                RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.5f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y + topHeight * 0.5f)), groundLayer);
                                Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                            }

                            if (awareness.seesTop1)
                            {
                                Gizmos.DrawRay(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.25f) - eyeLevel));
                            }
                            else
                            {
                                RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y + topHeight * 0.25f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y + topHeight * 0.25f)), groundLayer);
                                Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                            }

                            if (awareness.seesMid)
                            {
                                Gizmos.DrawRay(eyeLevel, targetMid - eyeLevel);
                            }
                            else
                            {
                                RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, targetMid - eyeLevel, Vector2.Distance(eyeLevel, targetMid), groundLayer);
                                Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                            }

                            if (awareness.seesBot1)
                            {
                                Gizmos.DrawRay(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.25f) - eyeLevel));

                            }
                            else
                            {
                                RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.25f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y - botHeight * 0.25f)), groundLayer);
                                Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                            }

                            if (awareness.seesBot2)
                            {
                                Gizmos.DrawRay(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.5f) - eyeLevel));
                            }
                            else
                            {
                                RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.5f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y - botHeight * 0.5f)), groundLayer);
                                Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                            }

                            if (awareness.seesBot3)
                            {
                                Gizmos.DrawRay(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.75f) - eyeLevel));
                            }
                            else
                            {
                                RaycastHit2D obstacle = Physics2D.Raycast(eyeLevel, (new Vector2(targetMid.x, targetMid.y - botHeight * 0.75f) - eyeLevel), Vector2.Distance(eyeLevel, new Vector2(targetMid.x, targetMid.y - botHeight * 0.75f)), groundLayer);
                                Gizmos.DrawRay(eyeLevel, obstacle.point - eyeLevel);
                            }

                            if (awareness.seesBot)
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
                //else
                //{
                //    Debug.Log("Not Light");
                //}
            }



            //foreach (Collider2D interest in interestsInSight)
            //{
            //    if (interest.gameObject.tag == "Light")
            //    {
            //        Transform[] rayEnds = new Transform[interest.gameObject.transform.childCount];
            //        float[] distances = new float[interest.gameObject.transform.childCount];
            //        //List<float> distances = new List<float>();

            //        for (int i = 0; i < interest.gameObject.transform.childCount; i++)
            //        {
            //            rayEnds[i] = interest.gameObject.transform.GetChild(i).transform;
            //            distances[i] = Vector2.Distance(transform.position, rayEnds[i].position);
            //            //distances.Add(Vector2.Distance(transform.position, rayEnds[i].position));
            //        }

            //        for (int i = 0; i < rayEnds.Length; i++)
            //        {
            //            if (distances[i] == Mathf.Min(distances))
            //            {
            //                RaycastHit2D sight = Physics2D.Raycast(transform.position, rayEnds[i].position - transform.position, groundLayer);
            //                if (!sight)
            //                {
            //                    Gizmos.DrawRay(transform.position, rayEnds[i].position - transform.position);
            //                    //Debug.Log("can see closest ray");
            //                }
            //            }
            //        }

            //        //interest.gameObject.transform.
            //    }
            //    //else
            //    //{
            //    //    Debug.Log("Not Light");
            //    //}
            //}
            //}
        }
    }



}
