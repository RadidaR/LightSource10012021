using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCurveScript : MonoBehaviour
{
    [SerializeField] public GameObject[] controlPoints;
    public Transform target;
    //private Vector2 gizmosPosition1;
    //private Vector2 gizmosPosition2;


    //ActionMap actionMap;
    //public PlayerInputData playerInputData;


    //LINE RENDERER STUFF
    public LineRenderer lineRenderer;

    public int numPoints;
    private Vector3[] positions/* = new Vector3[50]*/;

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
        positions = new Vector3[numPoints];
    }

    private void Update()
    {
        Vector3 startPoint;
        Vector3 startControlPoint;
        Vector3 endControlPoint;
        Vector3 endPoint;

        startPoint = gameObject.transform.position;

        //float distanceToTarget = Vector2.Distance(startPoint, target.position);
        float distanceToTarget = Vector2.Distance(startPoint, target.position);

        float xDistance = target.position.x - startPoint.x;
        float yDistance = target.position.y - startPoint.y;

        if (Mathf.Abs(yDistance) < 10)
        {
           // endPoint = new Vector3(target.position.x + (xDistance / 3), endPoint.y);

            if (Physics2D.OverlapCircle(new Vector2(target.position.x + (xDistance / 3), target.position.y), 0.5f, 3))
            {
                endPoint = new Vector3(target.position.x + (xDistance / 3), target.position.y + 2);
            }
            else
            {
                RaycastHit2D checkForGround = Physics2D.Raycast(new Vector2(target.position.x + (xDistance / 3), target.position.y), Vector2.down, 50f, 3);

                if (checkForGround)
                {
                    endPoint = new Vector3(target.position.x + (xDistance / 3), target.position.y - checkForGround.distance);
                }
                else
                {
                    return;
                }
            }

            startControlPoint = new Vector3(startPoint.x + xDistance * 0.75f, startPoint.y + 3);
            endControlPoint = new Vector3(target.position.x + (xDistance / 3) * 0.25f, endPoint.y + 3);

            DrawCurve(startPoint, startControlPoint, endControlPoint, endPoint);
        }
        //else
        //{
        //    if (Physics2D.OverlapCircle(new Vector2(target.position.x + xDistance, target.position.y), 0.5f, 3))
        //    {
        //        endPoint = new Vector3(target.position.x + xDistance, target.position.y +)
        //    }
        //}





        //else
        //{
        //    if (Physics2D.OverlapCircle(new Vector2(target.position.x + xDistance, target.position.y), 0.5f, 3))
        //    {
        //        endPoint = new Vector3(target.position.x + xDistance, target.position.y + Time.deltaTime);
        //    }
        //    else
        //    {
        //        RaycastHit2D checkForGround = Physics2D.Raycast(new Vector2(target.position.x + xDistance), target.position.y), Vector2.down, 50f, 3);

        //        if (checkForGround)
        //        {
        //            endPoint = new Vector3(target.position.x + xDistance, target.position.y - checkForGround.distance);
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }

        //    startControlPoint = new Vector3(startPoint.x + xDistance * 0.75f, startPoint.y + 3);
        //    endControlPoint = new Vector3(target.position.x + (xDistance / 3) * 0.75f, endPoint.y + 3);
        //    DrawCurve(startPoint, startControlPoint, endControlPoint, endPoint);

        //}


        //endPoint = 
        //DrawCurve(controlPoints[0].transform.position, controlPoints[1].transform.position, controlPoints[2].transform.position, controlPoints[3].transform.position);
    }

    public void PositionPoints(Vector3 startPoint, Vector3 startControl, Vector3 endControl, Vector3 endPoint)
    {
        //controlPoints[0].transform.position = startPoint;
        //controlPoints[1].transform.position = startControl;
        //controlPoints[2].transform.position = endControl;
        //controlPoints[3].transform.position = endPoint;
    }

    public void DrawCurve(Vector3 startPoint, Vector3 startControl, Vector3 endControl, Vector3 endPoint)
    {
        //Debug.Log("Drawing Curve");


        //for (int i = 1; i < numPoints + 1; i++)
        //{
        //    float t = i / (float)numPoints;
        //    positions[i - 1] = CalculateCurve(t, controlPoints[0].transform.position, controlPoints[1].transform.position, controlPoints[2].transform.position, controlPoints[3].transform.position);
        //}
        //lineRenderer.SetPositions(positions);

        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateCurve(t, startPoint, startControl, endControl, endPoint);
        }
        lineRenderer.SetPositions(positions);
    }

    public void ResetCurve()
    {
        //lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;        
    }    

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
