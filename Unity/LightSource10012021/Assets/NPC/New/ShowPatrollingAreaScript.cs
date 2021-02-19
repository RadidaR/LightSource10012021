using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPatrollingAreaScript : MonoBehaviour
{
    NPCData data;

    private void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            data = GetComponentInChildren<NPCStatesScript>().data;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if ((data.idleBoundaryNegX != 0 || data.idleBoundaryPosX != 0) && (data.idleBoundaryNegY != 0 || data.idleBoundaryPosY != 0))
        {
            Vector2 centerpoint = gameObject.transform.position;
            Gizmos.color = Color.blue;

            if (Mathf.Abs(data.idleBoundaryPosX) > Mathf.Abs(data.idleBoundaryNegX))
            {
                centerpoint.x += (Mathf.Abs(data.idleBoundaryPosX) - Mathf.Abs(data.idleBoundaryNegX)) / 2;
            }
            else if (Mathf.Abs(data.idleBoundaryPosX) < Mathf.Abs(data.idleBoundaryNegX))
            {
                centerpoint.x -= (Mathf.Abs(data.idleBoundaryNegX) - Mathf.Abs(data.idleBoundaryPosX)) / 2;
            }

            if (Mathf.Abs(data.idleBoundaryPosY) > Mathf.Abs(data.idleBoundaryNegY))
            {
                centerpoint.y += (Mathf.Abs(data.idleBoundaryPosY) - Mathf.Abs(data.idleBoundaryNegY)) / 2;
            }
            else if (Mathf.Abs(data.idleBoundaryPosY) < Mathf.Abs(data.idleBoundaryNegY))
            {
                centerpoint.y -= (Mathf.Abs(data.idleBoundaryNegY) - Mathf.Abs(data.idleBoundaryPosY)) / 2;
            }

            Gizmos.DrawWireCube(centerpoint, new Vector3(data.idleBoundaryPosX + Mathf.Abs(data.idleBoundaryNegX), data.idleBoundaryPosY + Mathf.Abs(data.idleBoundaryNegY), 0));
            //Gizmos.(gameObject.transform.position, new Vector2(data.idleBoundaryPosX + Mathf.Abs(data.idleBoundaryNegX), data.idleBoundaryPosY + Mathf.Abs(data.idleBoundaryNegY)));
        }
    }
}
