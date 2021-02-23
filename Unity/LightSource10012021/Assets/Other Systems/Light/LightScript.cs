using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightScript : MonoBehaviour
{
    [SerializeField] CircleCollider2D lightRange;
    [SerializeField] Light2D lightSource;
    // Start is called before the first frame update
    void Start()
    {
        lightRange = GetComponent<CircleCollider2D>();
        lightSource = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lightRange.radius = lightSource.pointLightOuterRadius;

        if (!lightSource.enabled)
        {
            lightRange.enabled = false;
        }
        else
        {
            lightRange.enabled = true;
        }
    }


}
