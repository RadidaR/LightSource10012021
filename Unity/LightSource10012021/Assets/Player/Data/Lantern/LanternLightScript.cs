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
        RaycastHit2D threeRay = Physics2D.Raycast(gameObject.transform.position, pos3.position - gameObject.transform.position, lightSource.pointLightOuterRadius, groundLayer);
        if (!threeRay)
        {
            Debug.Log("three ray clear");
            pos3.position = gameObject.transform.position + (pos3.position - gameObject.transform.position).normalized * lightSource.pointLightOuterRadius;
        }
        else
        {
            Debug.Log("hit");
            pos3.position = gameObject.transform.position + (pos3.position - gameObject.transform.position).normalized * threeRay.distance;
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
