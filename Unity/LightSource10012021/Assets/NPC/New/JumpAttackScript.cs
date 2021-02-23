using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackScript : MonoBehaviour
{
    public AttackData data;

    //LINE RENDERER STUFF
    public LineRenderer lineRenderer;

    public int numPoints;
    private Vector3[] positions;

    
    private void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            lineRenderer = GetComponent<LineRenderer>();

            lineRenderer.positionCount = numPoints;
            positions = new Vector3[numPoints];
        }
    }

    public void DrawCurve(Vector3 startPoint, Vector3 startControl, Vector3 endControl, Vector3 endPoint)
    {
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateCurve(t, startPoint, startControl, endControl, endPoint);
        }
        lineRenderer.SetPositions(positions);
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

    public void ResetCurve()
    {
        lineRenderer.enabled = false;
    }
}
