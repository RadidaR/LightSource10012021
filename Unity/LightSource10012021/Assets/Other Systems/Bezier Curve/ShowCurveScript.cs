using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCurveScript : MonoBehaviour
{
    [SerializeField] public GameObject[] controlPoints;
    public Transform targetForCurve;
    //private Vector2 gizmosPosition1;
    //private Vector2 gizmosPosition2;

    public LayerMask groundLayer;
    //ActionMap actionMap;
    //public PlayerInputData playerInputData;


    //LINE RENDERER STUFF
    public LineRenderer lineRenderer;

    public int numPoints;
    private Vector3[] positions/* = new Vector3[50]*/;

    [SerializeField] private Vector3[] curvePositions;

    //Vector3 controlPoint1;
    //Vector3 controlPoint2;

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

    public void CalculatePath(Vector3 startingPosition, GameObject target, int resolution)
    //public Vector3[] CalculatePath(Vector3 startingPosition, GameObject target, int resolution)
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

        startPoint = startingPosition;

        Transform targetMid = GetNamedChild(target.gameObject, "Mid").transform;
        Transform targetBot = GetNamedChild(target.gameObject, "Bot").transform;

        float distanceToTarget = Vector2.Distance(startPoint, target.transform.position);

        float xDistance = target.transform.position.x - startPoint.x;
        float yDistance = targetMid.position.y - startPoint.y;


        float curveDirection = xDistance / Mathf.Abs(xDistance);

        RaycastHit2D checkForWallsB = Physics2D.Raycast(targetBot.position, new Vector2(curveDirection, 0), Mathf.Abs(5f), groundLayer);
        RaycastHit2D checkForWallsM = Physics2D.Raycast(targetMid.position, new Vector2(curveDirection, 0), Mathf.Abs(5f), groundLayer);


        endX = target.transform.position.x + (xDistance * 0.75f);

        RaycastHit2D checkForGround = Physics2D.Raycast(new Vector2(endX, targetBot.position.y), Vector2.down, 250, groundLayer);


        endY = target.transform.position.y - checkForGround.distance;

        sControlX = target.transform.position.x;

        if (yDistance >= 0)
        {
            if (yDistance >= 0 && yDistance <= 5)
            {
                sControlY = targetMid.position.y + yDistance;
                eControlX = target.transform.position.x + ((endX - target.transform.position.x) * 0.5f);
                eControlY = targetMid.position.y + yDistance;
            }
            else if (yDistance > 5 && yDistance <= 10)
            {
                sControlY = targetMid.position.y + yDistance;
                eControlX = target.transform.position.x + ((endX - target.transform.position.x) * 0.5f);
                eControlY = targetMid.position.y;
            }
            else if (yDistance > 10 && yDistance < 40)
            {
                sControlY = targetMid.transform.position.y + (yDistance * 0.75f);
                eControlX = target.transform.position.x + ((endX - target.transform.position.x) * 0.75f);
                eControlY = targetMid.position.y - (yDistance * 0.25f);
            }
            else
            {
                sControlX += xDistance * 0.5f;
                sControlY = targetMid.position.y + (yDistance * 1.25f);
                eControlX = target.transform.position.x + ((endX - target.transform.position.x));
                eControlY = targetMid.position.y - (yDistance);
            }
        }
        else
        {

            if (yDistance >= -5)
            {
                endX = target.transform.position.x + (xDistance * 0.75f);
                endX += yDistance * curveDirection * 0.5f;

                sControlX = target.transform.position.x;
                sControlY = targetMid.position.y + Mathf.Abs(yDistance);

                eControlX = target.transform.position.x + ((endX - target.transform.position.x) * 0.5f);
                eControlY = targetBot.position.y + (-yDistance * 0.5f);
            }
            else if (yDistance < -5 && yDistance >= -12.5f)
            {
                endX = target.transform.position.x + (xDistance * 0.75f);
                endX += yDistance * curveDirection * 0.5f;

                sControlX = target.transform.position.x;
                sControlY = targetMid.position.y + (Mathf.Abs(yDistance) * 0.5f);

                eControlX = target.transform.position.x + ((endX - target.transform.position.x) * 0.5f);
                eControlY = endY + (-(endY - targetBot.position.y));
            }
            else
            {
                endX = target.transform.position.x;

                sControlX = target.transform.position.x;
                sControlY = targetMid.position.y + (Mathf.Abs(yDistance));

                eControlX = target.transform.position.x + ((endX - target.transform.position.x) * 0.75f);
                eControlY = endY + (-(endY - targetBot.position.y) * 0.5f);

            }

            if (Mathf.Abs(eControlX - startPoint.x) > Mathf.Abs(endX - startPoint.x))
            {
                eControlX = endX;
            }
        }


        endPoint = new Vector3(endX, endY);
        startControlPoint = new Vector3(sControlX, sControlY);
        endControlPoint = new Vector3(eControlX, eControlY);


        /*Vector3[] */
        //curvePositions = new Vector3[resolution];
        ////int drawOnGroundCount = 0;

        //for (int i = 1; i < resolution + 1; i++)
        //{
        //    //curvePositions = new Vector3[i - 1];
        //    Debug.Log(i.ToString());
        //    float t = i / resolution;
        //    curvePositions[i - 1] = CalculateCurve(t, startPoint, new Vector3(sControlX, sControlY), new Vector3(eControlX, eControlY), new Vector3(endX, endY));
        //    //if (Physics2D.OverlapCircle(positions[i - 1], 1, groundLayer))
        //    //{
        //    //    //Debug.Log("Position " + (i - 1).ToString() + " overlaps with ground");
        //    //    lineRenderer.positionCount = i - 1;
        //    //    drawOnGroundCount++;
        //    //    if (drawOnGroundCount > 3)
        //    //    {
        //    //        //curvePositions = new Vector3[i - 1];
        //    //        break;
        //    //    }
        //    //    //Debug.Log(positions[i - 1].ToString() + " Overlaps with ground");
        //    //    //positions = new Vector3[i - 1];
        //    //    //lineRenderer.positionCount = positions.Length;
        //    //    //break
        //    //}
        //}

        //lineRenderer.SetPositions(curvePositions);
        //return curvePositions;



        DrawCurve(startPoint, startControlPoint, endControlPoint, endPoint);

        curvePositions = new Vector3[lineRenderer.positionCount];
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            curvePositions[i] = lineRenderer.GetPosition(i);
        }

        //controlPoint1 = startControlPoint;
        //controlPoint2 = endControlPoint;
    }

    private void Update()
    {

        //DrawCurve(controlPoints[0].transform.position, controlPoints[1].transform.position, controlPoints[2].transform.position, controlPoints[3].transform.position);

        CalculatePath(gameObject.transform.position, targetForCurve.gameObject, 150);
    }

    


    public void DrawCurve(Vector3 startPoint, Vector3 startControl, Vector3 endControl, Vector3 endPoint)
    {
        int drawOnGroundCount = 0;
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateCurve(t, startPoint, startControl, endControl, endPoint);
            if (Physics2D.OverlapCircle(positions[i - 1], 1, groundLayer))
            {
                drawOnGroundCount++;
                lineRenderer.positionCount = i - 1;
                if (drawOnGroundCount > 3)
                {
                    break;
                }
            }
        }                
        lineRenderer.SetPositions(positions);
    }

    //public Vector3[] GetCurvePositions(int numberOfPoints, Vector3 startPoint, Vector3 startControl, Vector3 endControl, Vector3 endPoint)
    //{
    //    Vector3[] resolution = new Vector3[numberOfPoints];
    //    //positions = new Vector3[numPoints];
    //    //lineRenderer.positionCount = numPoints;

    //    int drawOnGroundCount = 0;
    //    for (int i = 1; i < numberOfPoints + 1; i++)
    //    {
    //        float t = i / numberOfPoints;
    //        resolution[i - 1] = CalculateCurve(t, startPoint, startControl, endControl, endPoint);
    //        if (Physics2D.OverlapCircle(resolution[i - 1], 1, groundLayer))
    //        {
    //            //Debug.Log("Position " + (i - 1).ToString() + " overlaps with ground");
    //            drawOnGroundCount++;
    //            //lineRenderer.positionCount = i - 1;
    //            resolution = new Vector3[i - 1];
    //            if (drawOnGroundCount > 3)
    //            {
    //                break;
    //            }
    //        }
    //    }
    //    //lineRenderer.SetPositions(positions);

    //    return resolution;
    //}

    public void ResetCurve()
    {
        //positions = new Vector3[numPoints];
        lineRenderer.enabled = false;        
    }    

    public Vector3 CalculateCurve(float t, Vector2 startPoint, Vector2 startControl, Vector2 endControl, Vector2 endPoint)
    {
        float A = 1 - t;
        float sqT = t * t;
        float sqA = A * A;
        float cubA = sqA * A;
        float cubT = sqT * t;

        Vector2 position = cubA * startPoint;
        position += 3 * sqA * t * startControl;
        position += 3 * A * sqT * endControl;
        position += cubT * endPoint;
        return position;
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

    //private void OnDrawGizmos()
    //{
    //    if (controlPoint1 != Vector3.zero)
    //    {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawWireSphere(controlPoint1, 1f);
    //    }

    //    if (controlPoint2 != Vector3.zero)
    //    {
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawWireSphere(controlPoint2, 1f);
    //    }
    //}
}

//Vector3 startPoint;

//Vector3 startControlPoint;
//float sControlX;
//float sControlY;

//Vector3 endControlPoint;
//float eControlX;
//float eControlY;

//Vector3 endPoint;
//float endX;
//float endY;

//startPoint = gameObject.transform.position;

//Transform targetMid = GetNamedChild(target.gameObject, "Mid").transform;
//Transform targetBot = GetNamedChild(target.gameObject, "Bot").transform;

//float distanceToTarget = Vector2.Distance(startPoint, target.position);

//float xDistance = target.position.x - startPoint.x;
//float yDistance = targetMid.position.y - startPoint.y;

////Debug.Log(yDistance.ToString());

//float curveDirection = xDistance / Mathf.Abs(xDistance);

//RaycastHit2D checkForWallsB = Physics2D.Raycast(targetBot.position, new Vector2(curveDirection, 0), Mathf.Abs(5f), groundLayer);
//RaycastHit2D checkForWallsM = Physics2D.Raycast(targetMid.position, new Vector2(curveDirection, 0), Mathf.Abs(5f), groundLayer);


////if (!checkForWallsB && !checkForWallsM)
////{
//    endX = target.position.x + (xDistance * 0.75f);

//    RaycastHit2D checkForGround = Physics2D.Raycast(new Vector2(endX, targetBot.position.y), Vector2.down, 250, groundLayer);

//    //Debug.Log(checkForGround.distance.ToString());

//    endY = target.position.y - checkForGround.distance;

//    sControlX = target.position.x;

////sControlY = targetMid.position.y + (10 / yDistance);
//if (yDistance >= 0)
//{
//    if (yDistance >= 0 && yDistance <= 5)
//    {
//        Debug.Log("Scenario 1 Between 0 & 5");
//        sControlY = targetMid.position.y + yDistance;
//        eControlX = target.position.x + ((endX - target.position.x) * 0.5f);
//        eControlY = targetMid.position.y + yDistance;
//    }
//    else if (yDistance > 5 && yDistance <= 10)
//    {
//        Debug.Log("Scenario 2 between 5 & 10");
//        sControlY = targetMid.position.y + yDistance;
//        eControlX = target.position.x + ((endX - target.position.x) * 0.5f);
//        eControlY = targetMid.position.y;
//    }
//    else if (yDistance > 10 && yDistance < 40)
//    {
//        Debug.Log("Scenario 3 between 10 and 40");
//        sControlY = targetMid.position.y + (yDistance * 0.75f);
//        eControlX = target.position.x + ((endX - target.position.x) * 0.75f);
//        eControlY = targetMid.position.y - (yDistance * 0.25f);
//    }
//    else/* if (yDistance >= 40)*/
//    {
//        Debug.Log("Scenario 4 above 40");
//        sControlX += xDistance * 0.5f;
//        sControlY = targetMid.position.y + (yDistance/* * 0.5f*/ * 1.25f);
//        eControlX = target.position.x + ((endX - target.position.x));
//        eControlY = targetMid.position.y - (yDistance/* * 0.5f*/);
//    }
//    //else
//    //{
//    //    sControlY = target.position.y + yDistance;
//    //    eControlX = target.position.x + ((endX - target.position.x));
//    //    eControlY = targetMid.position.y;
//    //}
//}
//else
//{
//    //if (checkForGround.distance >= 10)
//    //{
//    //    endX += yDistance * curveDirection * 0.5f;

//    //    if (Mathf.Abs(endX - startPoint.x) < Mathf.Abs(xDistance))
//    //    {
//    //        endX = target.position.x;
//    //    }
//    //}
//    //else
//    //{
//    //    endX = target.position.x;
//    //}

//    if (yDistance >= -5)
//    {
//        Debug.Log("Scenario 5 Below -5");

//        endX = target.position.x + (xDistance * 0.75f);
//        endX += yDistance * curveDirection * 0.5f;

//        sControlX = target.position.x;
//        sControlY = targetMid.position.y + Mathf.Abs(yDistance);

//        eControlX = target.position.x + ((endX - target.position.x) * 0.5f);
//        eControlY = targetBot.position.y + (-yDistance * 0.5f);
//    }
//    else if (yDistance < -5 && yDistance >= -12.5f)
//    {
//        Debug.Log("Scenario 6 Between -5 and -12.5");
//        endX = target.position.x + (xDistance * 0.75f);
//        endX += yDistance * curveDirection * 0.5f;

//        sControlX = target.position.x;
//        sControlY = targetMid.position.y + (Mathf.Abs(yDistance) * 0.5f);

//        eControlX = target.position.x + ((endX - target.position.x) * 0.5f);
//        eControlY = endY + (-(endY - targetBot.position.y) /** 0.75f*/);
//    }
//    else /*(yDistance < -10)*/    /* && yDistance >= -25*/
//    {
//        Debug.Log("Scenario 7 Below -12.5");
//        endX = target.position.x;

//        sControlX = target.position.x;
//        sControlY = targetMid.position.y + (Mathf.Abs(yDistance));

//        eControlX = target.position.x + ((endX - target.position.x) * 0.75f);
//        eControlY = endY + (-(endY - targetBot.position.y) * 0.5f);

//        //eControlX = target.position.x + ((endX - target.position.x));
//        //eControlY = targetMid.position.y;
//    }
//    //else
//    //{
//    //    sControlY = target.position.y + yDistance;
//    //    eControlX = target.position.x + ((endX - target.position.x));
//    //    eControlY = targetMid.position.y;
//    //}

//    if (Mathf.Abs(eControlX - startPoint.x) > Mathf.Abs(endX - startPoint.x))
//    {
//        eControlX = endX;
//    }
//}
////}
////else
////{
////    if (checkForWallsB)
////    {
////        endX = target.position.x + (checkForWallsB.distance * curveDirection);
////        endY = targetMid.position.y - checkForWallsB.distance;
////    }
////    else
////    {
////        endX = target.position.x + (checkForWallsM.distance * curveDirection);
////        endY = targetMid.position.y - checkForWallsM.distance;
////    }


////    RaycastHit2D checkForGround = Physics2D.Raycast(new Vector2(endX, targetBot.position.y), Vector2.down, 250, groundLayer);

////    //Debug.Log(checkForGround.distance.ToString());
////    //if (checkForGround.distance <= 5)
////    //{
////    //    endY = target.position.y - checkForGround.distance;
////    //}
////    //else
////    //{
////    //    endY = target.position.y - ;
////    //}

////    sControlX = target.position.x;

////    if (yDistance >= 0)
////    {
////        if (yDistance >= 0 && yDistance <= 5)
////        {
////            sControlY = targetMid.position.y + yDistance;
////            eControlX = target.position.x + ((endX - target.position.x) * 0.5f);
////            eControlY = targetMid.position.y + yDistance;
////        }
////        else if (yDistance > 5 && yDistance <= 10)
////        {
////            sControlY = targetMid.position.y + yDistance;
////            eControlX = target.position.x + ((endX - target.position.x) * 0.5f);
////            eControlY = targetMid.position.y;
////        }
////        else if (yDistance > 10)
////        {
////            sControlY = targetMid.position.y + (yDistance * 0.75f);
////            eControlX = target.position.x + ((endX - target.position.x) * 0.75f);
////            eControlY = targetMid.position.y - (yDistance * 0.25f);
////        }
////        else if (yDistance >= 40)
////        {
////            sControlY = targetMid.position.y + (yDistance * 0.5f);
////            eControlX = target.position.x + ((endX - target.position.x));
////            eControlY = targetMid.position.y - (yDistance * 0.5f);
////        }
////        else
////        {
////            sControlY = target.position.y + yDistance;
////            eControlX = target.position.x + ((endX - target.position.x));
////            eControlY = targetMid.position.y;
////        }
////    }
////    else
////    {
////        //if (checkForGround.distance >= 10)
////        //{
////        //    endX += yDistance * curveDirection * 0.5f;

////        //    if (Mathf.Abs(endX - startPoint.x) < Mathf.Abs(xDistance))
////        //    {
////        //        endX = target.position.x;
////        //    }
////        //}
////        //else
////        //{
////        //    endX = target.position.x;
////        //}

////        if (yDistance < 0 && yDistance >= -5)
////        {

////            //endX = target.position.x + (xDistance * 0.75f);
////            //endX += yDistance * curveDirection * 0.5f;

////            endX = target.position.x;

////            sControlX = target.position.x;
////            sControlY = targetMid.position.y + Mathf.Abs(yDistance);

////            eControlX = target.position.x + ((endX - target.position.x) * 0.5f);
////            eControlY = targetBot.position.y + (-yDistance * 0.5f);
////        }
////        else if (yDistance < -5 && yDistance >= -12.5f)
////        {
////            //endX = target.position.x + (xDistance * 0.75f);
////            //endX += yDistance * curveDirection * 0.5f;

////            endX = target.position.x;

////            sControlX = target.position.x;
////            sControlY = targetMid.position.y + (Mathf.Abs(yDistance) * 0.5f);

////            eControlX = target.position.x + ((endX - target.position.x) * 0.5f);
////            //eControlY = endY + (-(endY - targetBot.position.y) /** 0.75f*/);
////            eControlY = targetBot.position.y + (-yDistance * 0.75f);
////        }
////        else /*(yDistance < -10)*/    /* && yDistance >= -25*/
////        {
////            endX = target.position.x;

////            sControlX = target.position.x;
////            sControlY = targetMid.position.y + (Mathf.Abs(yDistance));

////            eControlX = target.position.x + ((endX - target.position.x) * 0.75f);
////            eControlY = endY + (-(endY - targetBot.position.y) * 0.5f);
////        }
////        //else
////        //{
////        //    sControlY = target.position.y + yDistance;
////        //    eControlX = target.position.x + ((endX - target.position.x));
////        //    eControlY = targetMid.position.y;
////        //}

////        if (Mathf.Abs(eControlX - startPoint.x) > Mathf.Abs(endX - startPoint.x))
////        {
////            eControlX = endX;
////        }
////    }
////}

//////if (eControlY <= endY)
//////{
//////    eControlY = endY + 5;
//////}

//////if (eControlX - target.position.x > endX - eControlX)
//////{
//////    eControlX += (endX - target.position.x * curveDirection * 0.25f);
//////}

//endPoint = new Vector3(endX, endY);
//startControlPoint = new Vector3(sControlX, sControlY);
//endControlPoint = new Vector3(eControlX, eControlY);

//DrawCurve(startPoint, startControlPoint, endControlPoint, endPoint);

//controlPoint1 = startControlPoint;
//controlPoint2 = endControlPoint;
