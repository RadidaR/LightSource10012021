using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCurveScript : MonoBehaviour
{
    [SerializeField] Transform[] controlPoints;
    private Vector2 gizmosPosition1;
    private Vector2 gizmosPosition2;


    //ActionMap actionMap;
    //public PlayerInputData playerInputData;


    //LINE RENDERER STUFF
    public LineRenderer lineRenderer;

    private int numPoints = 50;
    private Vector3[] positions = new Vector3[50];

    //private void OnEnable()
    //{
    //    actionMap.Enable();
    //}

    //private void OnDisable()
    //{
    //    actionMap.Disable();
    //}

    private void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
    }

    private void Awake()
    {
        //actionMap = new ActionMap();

        //actionMap.Gameplay.Jump.canceled += ctx => ResetCurve();
    }

    private void Start()
    {
        //lineRenderer.SetVertexCount(numPoints);
        lineRenderer.positionCount = numPoints;
    }

    private void Update()
    {
            DrawCurve();
    }

    private void DrawCurve()
    {
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateCurve(t, controlPoints[0].position, controlPoints[1].position, controlPoints[2].position, controlPoints[3].position);
        }
        lineRenderer.SetPositions(positions);
    }

    private void ResetCurve()
    {
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
        
    }

    //private void OnDrawGizmos()
    //{
        
    //        for (float t = 0; t <= 1; t += 0.02f)
    //        {
    //            gizmosPosition1 = Mathf.Pow(1 - t, 3) * controlPoints[0].position +
    //                3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
    //                3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
    //                Mathf.Pow(t, 3) * controlPoints[3].position;

    //        //gizmosPosition2 = Mathf.Pow(1 - t + 0.04f, 3) * controlPoints[0].position +
    //        //    3 * Mathf.Pow(1 - t + 0.04f, 2) * (t + 0.04f) * controlPoints[1].position +
    //        //    3 * (1 - t + 0.04f) * Mathf.Pow(t + 0.04f, 2) * controlPoints[2].position +
    //        //    Mathf.Pow(t + 0.04f, 3) * controlPoints[3].position;

    //        Gizmos.DrawWireSphere(gizmosPosition1, 0.1f);
    //        //Gizmos.DrawLine(gizmosPosition1, gizmosPosition2);
    //        }

    //        //for (float t = 0.01f; t <= 1; t += 0.02f)
    //        //{
    //        //    gizmosPosition2 = Mathf.Pow(1 - t, 3) * controlPoints[0].position +
    //        //        3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
    //        //        3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
    //        //        Mathf.Pow(t, 3) * controlPoints[3].position;

    //        //}
       


    //    Gizmos.DrawLine(controlPoints[0].position, controlPoints[1].position);
    //    Gizmos.DrawLine(controlPoints[2].position, controlPoints[3].position);
    //}

    public Vector3 CalculateCurve(float t, Vector2 startPoint, Vector2 startControl, Vector2 endControl, Vector2 endPoint)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector2 p = uuu * startPoint;
        p += 3 * uu * t * startControl;
        p += 3 * u * tt * endControl;
        p += ttt * endPoint;
        return p;
    }
}
