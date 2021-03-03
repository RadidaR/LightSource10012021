using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectLightScript : MonoBehaviour
{
    public NPCData data;

    //public bool isInterested;
    public Collider2D[] interestsInSight;
    public LayerMask interestLayers;
    public LayerMask groundLayer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        interestsInSight = Physics2D.OverlapCircleAll(transform.position, data.visionRange, interestLayers);

        foreach (Collider2D interest in interestsInSight)
        {
            if (interest.gameObject.tag == "Light")
            {
                List<Transform> rayEnds = new List<Transform>();
                //float[] distances = new float[interest.gameObject.transform.childCount];
                List<float> distances = new List<float>();

                List<Transform> closestRays = new List<Transform>();

                //Transform[] closestRayEnd = new Transform[interest.gameObject.transform.childCount / 2 + 1];
                //int posToDel;

                for (int i = 0; i < interest.gameObject.transform.childCount; i++)
                {
                    rayEnds.Add(interest.gameObject.transform.GetChild(i).transform);
                    distances.Add(Vector2.Distance(transform.position, rayEnds[i].position));


                    //rayEnds[i] = interest.gameObject.transform.GetChild(i).transform;
                    //distances[i] = Vector2.Distance(transform.position, rayEnds[i].position);
                    //closestRayEnd[i] = rayEnds[i];

                    int o = 0;
                    if (i == interest.gameObject.transform.childCount - 1)
                    {

                    }
                    //distances.Add(Vector2.Distance(transform.position, rayEnds[i].position));
                }


                for (int i = 0; i < interest.gameObject.transform.childCount / 2 + 1; i++)
                {
                    if (distances[i] == Mathf.Min(distances.ToArray()) && rayEnds.Count > interest.gameObject.transform.childCount / 2 + 1)
                    {
                        closestRays.Add(rayEnds[i]);

                        rayEnds.Remove(rayEnds[i]);
                        distances.Remove(distances[i]);

                        i = -1;
                    }
                }


                //float closest = Mathf.Min(distances);

                //float[] distances2 = new float[distances.Length - 1];
                //for (int i = 0; i < rayEnds.Length; i++)
                //{
                //    if (distances[i] != closest)
                //    {
                //        distances2[i] = distances[i];                        
                //    }
                //}

                //float closest2 = Mathf.Min(distances2);

                //float[] distances3 = new float[distances2.Length - 1];
                //for (int i = 0; i < rayEnds.Length; i++)
                //{
                //    if (distances2[i] != closest2)
                //    {
                //        distances3[i] = distances2[i];
                //    }
                //}

                //float closest3 = Mathf.Min(distances3);

                //interest.gameObject.transform.
            }
            //else
            //{
            //    Debug.Log("Not Light");
            //}
        }
    }

    private void OnDrawGizmos()
    {
        if (data != null)
        {
            Gizmos.DrawWireSphere(transform.position, data.visionRange);

            foreach (Collider2D interest in interestsInSight)
            {
                if (interest.gameObject.tag == "Light")
                {
                    List<Transform> rayEnds = new List<Transform>();
                    //float[] distances = new float[interest.gameObject.transform.childCount];
                    List<float> distances = new List<float>();

                    List<Transform> closestRays = new List<Transform>();

                    //Transform[] closestRayEnd = new Transform[interest.gameObject.transform.childCount / 2 + 1];
                    //int posToDel;

                    for (int i = 0; i < interest.gameObject.transform.childCount; i++)
                    {
                        rayEnds.Add(interest.gameObject.transform.GetChild(i).transform);
                        distances.Add(Vector2.Distance(transform.position, rayEnds[i].position));


                        //rayEnds[i] = interest.gameObject.transform.GetChild(i).transform;
                        //distances[i] = Vector2.Distance(transform.position, rayEnds[i].position);
                        //closestRayEnd[i] = rayEnds[i];

                        int o = 0;
                        if (i == interest.gameObject.transform.childCount - 1)
                        {

                        }
                        //distances.Add(Vector2.Distance(transform.position, rayEnds[i].position));
                    }


                    for (int i = 0; i < interest.gameObject.transform.childCount / 2 + 1; i++)
                    {
                        if (distances[i] == Mathf.Min(distances.ToArray()) && rayEnds.Count > interest.gameObject.transform.childCount / 2 + 1)
                        {
                            closestRays.Add(rayEnds[i]);

                            rayEnds.Remove(rayEnds[i]);
                            distances.Remove(distances[i]);

                            i = -1;
                        }
                    }

                    Debug.Log(closestRays.Count.ToString());

                    for (int i = 0; i < closestRays.Count; i++)
                    {
                        Gizmos.DrawRay(transform.position, closestRays[i].position - transform.position);
                    }

                    //foreach (Transform lightRay in closestRays)
                    //{
                    //    Gizmos.DrawRay(transform.position, lightRay.position - transform.position);
                    //}


                    //float closest = Mathf.Min(distances);

                    //float[] distances2 = new float[distances.Length - 1];
                    //for (int i = 0; i < rayEnds.Length; i++)
                    //{
                    //    if (distances[i] != closest)
                    //    {
                    //        distances2[i] = distances[i];                        
                    //    }
                    //}

                    //float closest2 = Mathf.Min(distances2);

                    //float[] distances3 = new float[distances2.Length - 1];
                    //for (int i = 0; i < rayEnds.Length; i++)
                    //{
                    //    if (distances2[i] != closest2)
                    //    {
                    //        distances3[i] = distances2[i];
                    //    }
                    //}

                    //float closest3 = Mathf.Min(distances3);

                    //interest.gameObject.transform.
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
        }
    }
}
