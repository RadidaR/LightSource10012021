using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCurveScript : MonoBehaviour
{
    [SerializeField] public GameObject[] controlPoints;
    public Transform target;
    //private Vector2 gizmosPosition1;
    //private Vector2 gizmosPosition2;

    public LayerMask groundLayer;
    //ActionMap actionMap;
    //public PlayerInputData playerInputData;


    //LINE RENDERER STUFF
    public LineRenderer lineRenderer;

    public int numPoints;
    private Vector3[] positions/* = new Vector3[50]*/;

    Vector3 controlPoint1;
    Vector3 controlPoint2;

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
        //lineRenderer.positionCount = numPoints;
        positions = new Vector3[numPoints];
    }

    private void Update()
    {

        Vector3 startPoint;

        Vector3 startControlPoint;
        float sControlX;
        float sControlY;

        Vector3 endControlPoint;
        float eControlX;
        float eControlY;

        Vector3 endPoint;
        float endX;
        float endY;

        startPoint = gameObject.transform.position;

        Transform targetMid = GetNamedChild(target.gameObject, "Mid").transform;
        Transform targetBot = GetNamedChild(target.gameObject, "Bot").transform;

        float distanceToTarget = Vector2.Distance(startPoint, target.position);

        float xDistance = target.position.x - startPoint.x;
        float yDistance = targetMid.position.y - startPoint.y;

        //Debug.Log(yDistance.ToString());

        float curveDirection = xDistance / Mathf.Abs(xDistance);

        RaycastHit2D checkForWallsB = Physics2D.Raycast(targetBot.position, new Vector2(curveDirection, 0), Mathf.Abs(5f), groundLayer);
        RaycastHit2D checkForWallsM = Physics2D.Raycast(targetMid.position, new Vector2(curveDirection, 0), Mathf.Abs(5f), groundLayer);


        //if (!checkForWallsB && !checkForWallsM)
        //{
            endX = target.position.x + (xDistance * 0.75f);

            RaycastHit2D checkForGround = Physics2D.Raycast(new Vector2(endX, targetBot.position.y), Vector2.down, 250, groundLayer);

            //Debug.Log(checkForGround.distance.ToString());

            endY = target.position.y - checkForGround.distance;

            sControlX = target.position.x;

            //sControlY = targetMid.position.y + (10 / yDistance);
            if (yDistance >= 0)
            {
                if (yDistance >= 0 && yDistance <= 5)
                {
                    sControlY = targetMid.position.y + yDistance;
                    eControlX = target.position.x + ((endX - target.position.x) * 0.5f);
                    eControlY = targetMid.position.y + yDistance;
                }
                else if (yDistance > 5 && yDistance <= 10)
                {
                    sControlY = targetMid.position.y + yDistance;
                    eControlX = target.position.x + ((endX - target.position.x) * 0.5f);
                    eControlY = targetMid.position.y;
                }
                else if (yDistance > 10)
                {
                    sControlY = targetMid.position.y + (yDistance * 0.75f);
                    eControlX = target.position.x + ((endX - target.position.x) * 0.75f);
                    eControlY = targetMid.position.y - (yDistance * 0.25f);
                }
                else/* if (yDistance >= 40)*/
                {
                    sControlY = targetMid.position.y + (yDistance * 0.5f);
                    eControlX = target.position.x + ((endX - target.position.x));
                    eControlY = targetMid.position.y - (yDistance * 0.5f);
                }
                //else
                //{
                //    sControlY = target.position.y + yDistance;
                //    eControlX = target.position.x + ((endX - target.position.x));
                //    eControlY = targetMid.position.y;
                //}
            }
            else
            {
                //if (checkForGround.distance >= 10)
                //{
                //    endX += yDistance * curveDirection * 0.5f;

                //    if (Mathf.Abs(endX - startPoint.x) < Mathf.Abs(xDistance))
                //    {
                //        endX = target.position.x;
                //    }
                //}
                //else
                //{
                //    endX = target.position.x;
                //}

                if (yDistance >= -5)
                {

                    endX = target.position.x + (xDistance * 0.75f);
                    endX += yDistance * curveDirection * 0.5f;

                    sControlX = target.position.x;
                    sControlY = targetMid.position.y + Mathf.Abs(yDistance);

                    eControlX = target.position.x + ((endX - target.position.x) * 0.5f);
                    eControlY = targetBot.position.y + (-yDistance * 0.5f);
                }
                else if (yDistance < -5 && yDistance >= -12.5f)
                {
                    endX = target.position.x + (xDistance * 0.75f);
                    endX += yDistance * curveDirection * 0.5f;

                    sControlX = target.position.x;
                    sControlY = targetMid.position.y + (Mathf.Abs(yDistance) * 0.5f);

                    eControlX = target.position.x + ((endX - target.position.x) * 0.5f);
                    eControlY = endY + (-(endY - targetBot.position.y) /** 0.75f*/);
                }
                else /*(yDistance < -10)*/    /* && yDistance >= -25*/
                {
                    sControlX = target.position.x;
                    sControlY = targetMid.position.y + (Mathf.Abs(yDistance));

                    eControlX = target.position.x + ((endX - target.position.x) * 0.75f);
                    eControlY = endY + (-(endY - targetBot.position.y) * 0.5f);
                }
                //else
                //{
                //    sControlY = target.position.y + yDistance;
                //    eControlX = target.position.x + ((endX - target.position.x));
                //    eControlY = targetMid.position.y;
                //}

                if (Mathf.Abs(eControlX - startPoint.x) > Mathf.Abs(endX - startPoint.x))
                {
                    eControlX = endX;
                }
            }
        //}
        //else
        //{
        //    if (checkForWallsB)
        //    {
        //        endX = target.position.x + (checkForWallsB.distance * curveDirection);
        //        endY = targetMid.position.y - checkForWallsB.distance;
        //    }
        //    else
        //    {
        //        endX = target.position.x + (checkForWallsM.distance * curveDirection);
        //        endY = targetMid.position.y - checkForWallsM.distance;
        //    }


        //    RaycastHit2D checkForGround = Physics2D.Raycast(new Vector2(endX, targetBot.position.y), Vector2.down, 250, groundLayer);

        //    //Debug.Log(checkForGround.distance.ToString());
        //    //if (checkForGround.distance <= 5)
        //    //{
        //    //    endY = target.position.y - checkForGround.distance;
        //    //}
        //    //else
        //    //{
        //    //    endY = target.position.y - ;
        //    //}

        //    sControlX = target.position.x;

        //    if (yDistance >= 0)
        //    {
        //        if (yDistance >= 0 && yDistance <= 5)
        //        {
        //            sControlY = targetMid.position.y + yDistance;
        //            eControlX = target.position.x + ((endX - target.position.x) * 0.5f);
        //            eControlY = targetMid.position.y + yDistance;
        //        }
        //        else if (yDistance > 5 && yDistance <= 10)
        //        {
        //            sControlY = targetMid.position.y + yDistance;
        //            eControlX = target.position.x + ((endX - target.position.x) * 0.5f);
        //            eControlY = targetMid.position.y;
        //        }
        //        else if (yDistance > 10)
        //        {
        //            sControlY = targetMid.position.y + (yDistance * 0.75f);
        //            eControlX = target.position.x + ((endX - target.position.x) * 0.75f);
        //            eControlY = targetMid.position.y - (yDistance * 0.25f);
        //        }
        //        else if (yDistance >= 40)
        //        {
        //            sControlY = targetMid.position.y + (yDistance * 0.5f);
        //            eControlX = target.position.x + ((endX - target.position.x));
        //            eControlY = targetMid.position.y - (yDistance * 0.5f);
        //        }
        //        else
        //        {
        //            sControlY = target.position.y + yDistance;
        //            eControlX = target.position.x + ((endX - target.position.x));
        //            eControlY = targetMid.position.y;
        //        }
        //    }
        //    else
        //    {
        //        //if (checkForGround.distance >= 10)
        //        //{
        //        //    endX += yDistance * curveDirection * 0.5f;

        //        //    if (Mathf.Abs(endX - startPoint.x) < Mathf.Abs(xDistance))
        //        //    {
        //        //        endX = target.position.x;
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    endX = target.position.x;
        //        //}

        //        if (yDistance < 0 && yDistance >= -5)
        //        {

        //            //endX = target.position.x + (xDistance * 0.75f);
        //            //endX += yDistance * curveDirection * 0.5f;

        //            endX = target.position.x;

        //            sControlX = target.position.x;
        //            sControlY = targetMid.position.y + Mathf.Abs(yDistance);

        //            eControlX = target.position.x + ((endX - target.position.x) * 0.5f);
        //            eControlY = targetBot.position.y + (-yDistance * 0.5f);
        //        }
        //        else if (yDistance < -5 && yDistance >= -12.5f)
        //        {
        //            //endX = target.position.x + (xDistance * 0.75f);
        //            //endX += yDistance * curveDirection * 0.5f;

        //            endX = target.position.x;

        //            sControlX = target.position.x;
        //            sControlY = targetMid.position.y + (Mathf.Abs(yDistance) * 0.5f);

        //            eControlX = target.position.x + ((endX - target.position.x) * 0.5f);
        //            //eControlY = endY + (-(endY - targetBot.position.y) /** 0.75f*/);
        //            eControlY = targetBot.position.y + (-yDistance * 0.75f);
        //        }
        //        else /*(yDistance < -10)*/    /* && yDistance >= -25*/
        //        {
        //            endX = target.position.x;

        //            sControlX = target.position.x;
        //            sControlY = targetMid.position.y + (Mathf.Abs(yDistance));

        //            eControlX = target.position.x + ((endX - target.position.x) * 0.75f);
        //            eControlY = endY + (-(endY - targetBot.position.y) * 0.5f);
        //        }
        //        //else
        //        //{
        //        //    sControlY = target.position.y + yDistance;
        //        //    eControlX = target.position.x + ((endX - target.position.x));
        //        //    eControlY = targetMid.position.y;
        //        //}

        //        if (Mathf.Abs(eControlX - startPoint.x) > Mathf.Abs(endX - startPoint.x))
        //        {
        //            eControlX = endX;
        //        }
        //    }
        //}

        ////if (eControlY <= endY)
        ////{
        ////    eControlY = endY + 5;
        ////}

        ////if (eControlX - target.position.x > endX - eControlX)
        ////{
        ////    eControlX += (endX - target.position.x * curveDirection * 0.25f);
        ////}

        endPoint = new Vector3(endX, endY);
        startControlPoint = new Vector3(sControlX, sControlY);
        endControlPoint = new Vector3(eControlX, eControlY);

        DrawCurve(startPoint, startControlPoint, endControlPoint, endPoint);

        controlPoint1 = startControlPoint;
        controlPoint2 = endControlPoint;
        //DrawCurve(controlPoints[0].transform.position, controlPoints[1].transform.position, controlPoints[2].transform.position, controlPoints[3].transform.position);
    }

    private void OnDrawGizmos()
    {
        if (controlPoint1 != Vector3.zero)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(controlPoint1, 1f);
        }

        if (controlPoint2 != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(controlPoint2, 1f);
        }
    }


    public void DrawCurve(Vector3 startPoint, Vector3 startControl, Vector3 endControl, Vector3 endPoint)
    {
        //positions = new Vector3[numPoints];
        //lineRenderer.positionCount = numPoints;

        int drawOnGroundCount = 0;
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateCurve(t, startPoint, startControl, endControl, endPoint);
            if (Physics2D.OverlapCircle(positions[i - 1], 1, groundLayer))
            {
                //Debug.Log("Position " + (i - 1).ToString() + " overlaps with ground");
                lineRenderer.positionCount = i - 1;
                drawOnGroundCount++;
                if (drawOnGroundCount > 3)
                {
                    break;
                }
                //Debug.Log(positions[i - 1].ToString() + " Overlaps with ground");
                //positions = new Vector3[i - 1];
                //lineRenderer.positionCount = positions.Length;
                //break;
            }
        }
                
        lineRenderer.SetPositions(positions);

        //if (positions.Length > 0)
        //{
        //    for (int i = 0; !Physics2D.OverlapCircle(positions[i], 1, groundLayer) && i < positions.Length /*i < numPoints*/; i++)
        //    {
        //        positions = new Vector3[i];
        //        lineRenderer.positionCount = positions.Length;
        //        lineRenderer.SetPositions(positions);
        //    }
        //}
    }

    public void ResetCurve()
    {
        //positions = new Vector3[numPoints];
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


}
