using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackScript : MonoBehaviour
{
    public AttackData data;

    public GameObject npc;
    //public NPCStatesScript states;
    public NPCAwarenessScript awareness;

    //LINE RENDERER STUFF
    public LineRenderer lineRenderer;

    public LayerMask groundLayer;

    public int numPoints;
    public Vector3[] positions;
    //private Vector3[] jumpArc;

    
    private void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            npc = GetComponentInParent<OfInterest>().gameObject;
            awareness = npc.GetComponentInChildren<NPCAwarenessScript>();

            lineRenderer = GetComponent<LineRenderer>();

            lineRenderer.positionCount = numPoints;
            positions = new Vector3[numPoints];
        }
    }

    public void CalculatePath(Vector3 startPoint, GameObject target, int resolution)
    //public Vector3[] CalculatePath(Vector3 startingPosition, GameObject target, int resolution)
    {

        Vector3 startControlPoint;
        float sControlX;
        float sControlY;

        Vector3 endControlPoint;
        float eControlX;
        float eControlY;

        Vector3 endPoint;
        float endX;
        float endY;

        Transform targetMid = awareness.GetNamedChild(target.gameObject, "Mid").transform;
        Transform targetBot = awareness.GetNamedChild(target.gameObject, "Bot").transform;

        float xDistance = target.transform.position.x - startPoint.x;
        float yDistance = targetMid.position.y - startPoint.y;

        float curveDirection = xDistance / Mathf.Abs(xDistance);

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

        DrawCurve(startPoint, startControlPoint, endControlPoint, endPoint);

        //curvePositions = new Vector3[lineRenderer.positionCount];
        //for (int i = 0; i < lineRenderer.positionCount; i++)
        //{
        //    curvePositions[i] = lineRenderer.GetPosition(i);
        //}

    }

    public void DrawCurve(Vector3 startPoint, Vector3 startControl, Vector3 endControl, Vector3 endPoint)
    {
        lineRenderer.enabled = true;
        int drawOnGroundCount = 0;
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateCurve(t, startPoint, startControl, endControl, endPoint);
            if (Physics2D.OverlapCircle(positions[i - 1], 0.1f, groundLayer))
            {
                drawOnGroundCount++;
                lineRenderer.positionCount = i - 1;
                if (drawOnGroundCount > 0)
                {
                    break;
                }
            }
        }
        lineRenderer.SetPositions(positions);
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

    //public void DrawCurve(Vector3 startPoint, Vector3 startControl, Vector3 endControl, Vector3 endPoint)
    //{
    //    for (int i = 1; i < numPoints + 1; i++)
    //    {
    //        float t = i / (float)numPoints;
    //        positions[i - 1] = CalculateCurve(t, startPoint, startControl, endControl, endPoint);
    //    }
    //    lineRenderer.SetPositions(positions);
    //}

    //public Vector3 CalculateCurve(float t, Vector2 startPoint, Vector2 startControl, Vector2 endControl, Vector2 endPoint)
    //{
    //    float u = 1 - t;
    //    float tt = t * t;
    //    float uu = u * u;
    //    float uuu = uu * u;
    //    float ttt = tt * t;

    //    Vector2 p = uuu * startPoint;
    //    p += 3 * uu * t * startControl;
    //    p += 3 * u * tt * endControl;
    //    p += ttt * endPoint;
    //    return p;
    //}

    public void ResetCurve()
    {
        lineRenderer.enabled = false;
    }
}
