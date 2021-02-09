using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightColliderScript : MonoBehaviour
{
    [SerializeField] Light2D lightSource;
    [SerializeField] CircleCollider2D lightRange;

    private void Start()
    {
        lightSource = GetComponentInParent<Light2D>();
        lightRange = GetComponentInParent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            lightSource.enabled = false;
            lightRange.enabled = false;
        }
    }
}
