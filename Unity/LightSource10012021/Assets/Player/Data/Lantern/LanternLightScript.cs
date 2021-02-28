using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LanternLightScript : MonoBehaviour
{
    public PlayerStaminaData playerStaminaData;
    [SerializeField] Light2D lightSource;
    [SerializeField] CircleCollider2D lightRange;

    public LayerMask groundLayer;

    public Transform pos1;
    public Transform pos2;
    public Transform pos3;
    public Transform pos4;
    public Transform pos5;
    public Transform pos6;
    public Transform pos7;
    public Transform pos8;
    public Transform pos9;
    public Transform pos10;
    public Transform pos11;
    public Transform pos12;
    // Start is called before the first frame update
    void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            lightSource = GetComponent<Light2D>();
            lightRange = GetComponent<CircleCollider2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        lightRange.radius = lightSource.pointLightOuterRadius;
        lightSource.pointLightOuterRadius = playerStaminaData.currentStamina;
        lightSource.pointLightInnerRadius = playerStaminaData.currentStamina / 10;
        
        
        RaycastHit2D ray1 = Physics2D.Raycast(gameObject.transform.position, pos1.position - gameObject.transform.position, lightSource.pointLightOuterRadius, groundLayer);
        RaycastHit2D ray2 = Physics2D.Raycast(gameObject.transform.position, pos2.position - gameObject.transform.position, lightSource.pointLightOuterRadius, groundLayer);
        RaycastHit2D ray3 = Physics2D.Raycast(gameObject.transform.position, pos3.position - gameObject.transform.position, lightSource.pointLightOuterRadius, groundLayer);
        RaycastHit2D ray4 = Physics2D.Raycast(gameObject.transform.position, pos4.position - gameObject.transform.position, lightSource.pointLightOuterRadius, groundLayer);
        RaycastHit2D ray5 = Physics2D.Raycast(gameObject.transform.position, pos5.position - gameObject.transform.position, lightSource.pointLightOuterRadius, groundLayer);
        RaycastHit2D ray6 = Physics2D.Raycast(gameObject.transform.position, pos6.position - gameObject.transform.position, lightSource.pointLightOuterRadius, groundLayer);
        RaycastHit2D ray7 = Physics2D.Raycast(gameObject.transform.position, pos7.position - gameObject.transform.position, lightSource.pointLightOuterRadius, groundLayer);
        RaycastHit2D ray8 = Physics2D.Raycast(gameObject.transform.position, pos8.position - gameObject.transform.position, lightSource.pointLightOuterRadius, groundLayer);
        RaycastHit2D ray9 = Physics2D.Raycast(gameObject.transform.position, pos9.position - gameObject.transform.position, lightSource.pointLightOuterRadius, groundLayer);
        RaycastHit2D ray10 = Physics2D.Raycast(gameObject.transform.position, pos10.position - gameObject.transform.position, lightSource.pointLightOuterRadius, groundLayer);
        RaycastHit2D ray11 = Physics2D.Raycast(gameObject.transform.position, pos11.position - gameObject.transform.position, lightSource.pointLightOuterRadius, groundLayer);
        RaycastHit2D ray12 = Physics2D.Raycast(gameObject.transform.position, pos12.position - gameObject.transform.position, lightSource.pointLightOuterRadius, groundLayer);

        if (!ray1)
        {
            pos1.position = gameObject.transform.position + (pos1.position - gameObject.transform.position).normalized * lightSource.pointLightOuterRadius;
        }
        else
        {
            if (ray1.distance > 0)
            {
                pos1.position = gameObject.transform.position + (pos1.position - gameObject.transform.position).normalized * ray1.distance;
            }
            else
            {
                pos1.position = gameObject.transform.position + (pos1.position - gameObject.transform.position).normalized;
            }    
        }

        if (!ray2)
        {
            pos2.position = gameObject.transform.position + (pos2.position - gameObject.transform.position).normalized * lightSource.pointLightOuterRadius;
        }
        else
        {
            if (ray2.distance > 0)
            {
                pos2.position = gameObject.transform.position + (pos2.position - gameObject.transform.position).normalized * ray2.distance;
            }
            else
            {
                pos2.position = gameObject.transform.position + (pos2.position - gameObject.transform.position).normalized;
            }
        }

        if (!ray3)
        {
            pos3.position = gameObject.transform.position + (pos3.position - gameObject.transform.position).normalized * lightSource.pointLightOuterRadius;
        }
        else
        {
            if (ray3.distance > 0)
            {
                pos3.position = gameObject.transform.position + (pos3.position - gameObject.transform.position).normalized * ray3.distance;
            }
            else
            {
                pos3.position = gameObject.transform.position + (pos3.position - gameObject.transform.position).normalized;
            }
        }

        if (!ray4)
        {
            pos4.position = gameObject.transform.position + (pos4.position - gameObject.transform.position).normalized * lightSource.pointLightOuterRadius;
        }
        else
        {
            if (ray4.distance > 0)
            {
                pos4.position = gameObject.transform.position + (pos4.position - gameObject.transform.position).normalized * ray4.distance;
            }
            else
            {
                pos4.position = gameObject.transform.position + (pos4.position - gameObject.transform.position).normalized;
            }
        }

        if (!ray5)
        {
            pos5.position = gameObject.transform.position + (pos5.position - gameObject.transform.position).normalized * lightSource.pointLightOuterRadius;
        }
        else
        {
            if (ray5.distance > 0)
            {
                pos5.position = gameObject.transform.position + (pos5.position - gameObject.transform.position).normalized * ray5.distance;
            }
            else
            {
                pos5.position = gameObject.transform.position + (pos5.position - gameObject.transform.position).normalized;
            }
        }

        if (!ray6)
        {
            pos6.position = gameObject.transform.position + (pos6.position - gameObject.transform.position).normalized * lightSource.pointLightOuterRadius;
        }
        else
        {
            if (ray6.distance > 0)
            {
                pos6.position = gameObject.transform.position + (pos6.position - gameObject.transform.position).normalized * ray6.distance;
            }
            else
            {
                pos6.position = gameObject.transform.position + (pos6.position - gameObject.transform.position).normalized;
            }
        }

        if (!ray7)
        {
            pos7.position = gameObject.transform.position + (pos7.position - gameObject.transform.position).normalized * lightSource.pointLightOuterRadius;
        }
        else
        {
            if (ray7.distance > 0)
            {
                pos7.position = gameObject.transform.position + (pos7.position - gameObject.transform.position).normalized * ray7.distance;
            }
            else
            {
                pos7.position = gameObject.transform.position + (pos7.position - gameObject.transform.position).normalized;
            }
        }

        if (!ray8)
        {
            pos8.position = gameObject.transform.position + (pos8.position - gameObject.transform.position).normalized * lightSource.pointLightOuterRadius;
        }
        else
        {
            if (ray8.distance > 0)
            {
                pos8.position = gameObject.transform.position + (pos8.position - gameObject.transform.position).normalized * ray8.distance;
            }
            else
            {
                pos8.position = gameObject.transform.position + (pos8.position - gameObject.transform.position).normalized;
            }
        }

        if (!ray9)
        {
            pos9.position = gameObject.transform.position + (pos9.position - gameObject.transform.position).normalized * lightSource.pointLightOuterRadius;
        }
        else
        {
            if (ray9.distance > 0)
            {
                pos9.position = gameObject.transform.position + (pos9.position - gameObject.transform.position).normalized * ray9.distance;
            }
            else
            {
                pos9.position = gameObject.transform.position + (pos9.position - gameObject.transform.position).normalized;
            }
        }

        if (!ray10)
        {
            pos10.position = gameObject.transform.position + (pos10.position - gameObject.transform.position).normalized * lightSource.pointLightOuterRadius;
        }
        else
        {
            if (ray10.distance > 0)
            {
                pos10.position = gameObject.transform.position + (pos10.position - gameObject.transform.position).normalized * ray10.distance;
            }
            else
            {
                pos10.position = gameObject.transform.position + (pos10.position - gameObject.transform.position).normalized;
            }
        }

        if (!ray11)
        {
            pos11.position = gameObject.transform.position + (pos11.position - gameObject.transform.position).normalized * lightSource.pointLightOuterRadius;
        }
        else
        {
            if (ray11.distance > 0)
            {
                pos11.position = gameObject.transform.position + (pos11.position - gameObject.transform.position).normalized * ray11.distance;
            }
            else
            {
                pos11.position = gameObject.transform.position + (pos11.position - gameObject.transform.position).normalized;
            }
        }

        if (!ray12)
        {
            pos12.position = gameObject.transform.position + (pos12.position - gameObject.transform.position).normalized * lightSource.pointLightOuterRadius;
        }
        else
        {
            if (ray12.distance > 0)
            {
                pos12.position = gameObject.transform.position + (pos12.position - gameObject.transform.position).normalized * ray12.distance;
            }
            else
            {
                pos12.position = gameObject.transform.position + (pos12.position - gameObject.transform.position).normalized;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(gameObject.transform.position, pos1.position - gameObject.transform.position);
        Gizmos.DrawRay(gameObject.transform.position, pos2.position - gameObject.transform.position);
        Gizmos.DrawRay(gameObject.transform.position, pos3.position - gameObject.transform.position);
        Gizmos.DrawRay(gameObject.transform.position, pos4.position - gameObject.transform.position);
        Gizmos.DrawRay(gameObject.transform.position, pos5.position - gameObject.transform.position);
        Gizmos.DrawRay(gameObject.transform.position, pos6.position - gameObject.transform.position);
        Gizmos.DrawRay(gameObject.transform.position, pos7.position - gameObject.transform.position);
        Gizmos.DrawRay(gameObject.transform.position, pos8.position - gameObject.transform.position);
        Gizmos.DrawRay(gameObject.transform.position, pos9.position - gameObject.transform.position);
        Gizmos.DrawRay(gameObject.transform.position, pos10.position - gameObject.transform.position);
        Gizmos.DrawRay(gameObject.transform.position, pos11.position - gameObject.transform.position);
        Gizmos.DrawRay(gameObject.transform.position, pos12.position - gameObject.transform.position);
    }
}
