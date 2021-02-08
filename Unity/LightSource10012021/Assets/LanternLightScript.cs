using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LanternLightScript : MonoBehaviour
{
    public PlayerStaminaData playerStaminaData;
    Light2D lightSource;
    // Start is called before the first frame update
    void Start()
    {
        lightSource = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lightSource.pointLightOuterRadius = playerStaminaData.currentStamina;
        lightSource.pointLightInnerRadius = playerStaminaData.currentStamina / 10;
    }
}
