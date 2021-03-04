using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LanternLightScript : MonoBehaviour
{
    public PlayerStaminaData playerStaminaData;
    [SerializeField] Light2D lightSource;
    [SerializeField] CircleCollider2D lightRange;

    public LayerMask groundLayer;

    public Transform[] rayEnds;
    // Start is called before the first frame update
    void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            lightSource = GetComponent<Light2D>();
            lightRange = GetComponent<CircleCollider2D>();

            rayEnds = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.tag == "LightRayEnd")
                {
                    rayEnds[i] = transform.GetChild(i).transform;
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        lightRange.radius = lightSource.pointLightOuterRadius;
        lightSource.pointLightOuterRadius = playerStaminaData.currentStamina;
        lightSource.pointLightInnerRadius = playerStaminaData.currentStamina / 10;


        foreach (Transform rayEnd in rayEnds)
        {
            RaycastHit2D ray = Physics2D.Raycast(gameObject.transform.position, rayEnd.position - gameObject.transform.position, lightSource.pointLightOuterRadius, groundLayer);

            if (!ray)
            {
                rayEnd.position = gameObject.transform.position + (rayEnd.position - gameObject.transform.position).normalized * lightSource.pointLightOuterRadius;
            }
            else
            {
                if (ray.distance > 0)
                {
                    rayEnd.position = gameObject.transform.position + (rayEnd.position - gameObject.transform.position).normalized * ray.distance;
                }
                else
                {
                    rayEnd.position = gameObject.transform.position + (rayEnd.position - gameObject.transform.position).normalized;
                }
            }
            rayEnd.transform.GetChild(0).position = gameObject.transform.position + (rayEnd.position - gameObject.transform.position).normalized * Vector2.Distance(transform.position, rayEnd.position) * 0.33f;
            rayEnd.transform.GetChild(1).position = gameObject.transform.position + (rayEnd.position - gameObject.transform.position).normalized * Vector2.Distance(transform.position, rayEnd.position) * 0.67f;
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        foreach (Transform rayEnd in rayEnds)
        {
            Gizmos.DrawWireSphere(rayEnd.position, 1f);
            Gizmos.DrawRay(gameObject.transform.position, rayEnd.position - gameObject.transform.position);

            Gizmos.DrawWireSphere(rayEnd.transform.GetChild(0).position, 1f);
            Gizmos.DrawWireSphere(rayEnd.transform.GetChild(1).position, 1f);
        }
    }
}
