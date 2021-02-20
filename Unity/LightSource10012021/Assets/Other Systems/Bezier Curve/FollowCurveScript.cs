using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCurveScript : MonoBehaviour
{
    [SerializeField] private Transform[] routes;

    private int routeToGo;

    private float tParam;

    private Vector2 objectPosition;

    private float speedModifier;

    private bool coroutineRunning;

    public ShowCurveScript showCurve;

    // Start is called before the first frame update
    void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        speedModifier = 1f;
        coroutineRunning = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!coroutineRunning)
        {
            StartCoroutine(FollowCurve(routeToGo));
        }
        
    }

    private IEnumerator FollowCurve(int routeNumber)
    {
        coroutineRunning = true;

        Vector2 startPoint = routes[routeNumber].GetChild(0).position;
        Vector2 startControl = routes[routeNumber].GetChild(1).position;
        Vector2 endControl = routes[routeNumber].GetChild(2).position;
        Vector2 endPoint = routes[routeNumber].GetChild(3).position;

        float time1 = Time.time;
        //Debug.Log("Started " + Time.time.ToString());
        while (tParam < 1)
        {
            tParam += Time.fixedDeltaTime * speedModifier;

            //objectPosition = Mathf.Pow(1 - tParam, 3) * startPoint +
            //    3 * Mathf.Pow(1 - tParam, 2) * tParam * startControl +
            //    3 * (1 - tParam) * Mathf.Pow(tParam, 2) * endControl +
            //    Mathf.Pow(tParam, 3) * endPoint;

            objectPosition = showCurve.CalculateCurve(tParam, startPoint, startControl, endControl, endPoint);

            //transform.position = objectPosition;


            gameObject.GetComponent<Rigidbody2D>().MovePosition(objectPosition);
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }

        tParam = 0f;

        routeToGo++;

        if (routeToGo > routes.Length - 1)
        {
            routeToGo = 0;
        }

        coroutineRunning = false;
        float time2 = Time.time;

        float jumpDuration = time2 - time1;
        Debug.Log(jumpDuration.ToString());
        //Debug.Log("Ended " + Time.time.ToString());
    }
}
