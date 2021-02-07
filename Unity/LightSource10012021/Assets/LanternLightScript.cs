using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LanternLightScript : MonoBehaviour
{
    public PlayerStaminaData playerStaminaData;
    Light2D light;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        light.pointLightOuterRadius = playerStaminaData.currentStamina;
        light.pointLightInnerRadius = playerStaminaData.currentStamina / 10;
    }
}
