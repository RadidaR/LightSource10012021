using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LanternLightScript : MonoBehaviour
{
    public PlayerStaminaData playerStaminaData;
    [SerializeField] Light2D lightSource;
    [SerializeField] CircleCollider2D lightRange;
    // Start is called before the first frame update
    void Start()
    {
        lightSource = GetComponent<Light2D>();
        lightRange = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lightRange.radius = lightSource.pointLightOuterRadius;
        lightSource.pointLightOuterRadius = playerStaminaData.currentStamina;
        lightSource.pointLightInnerRadius = playerStaminaData.currentStamina / 10;
    }
}
