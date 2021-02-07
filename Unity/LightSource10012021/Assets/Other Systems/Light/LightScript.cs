using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightScript : MonoBehaviour
{
    CircleCollider2D collider;
    Light2D light;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CircleCollider2D>();
        light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        collider.radius = light.pointLightOuterRadius;
    }
}
