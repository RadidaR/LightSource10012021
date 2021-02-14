using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightColliderScript : MonoBehaviour
{
    [SerializeField] Light2D lightSource;
    [SerializeField] CircleCollider2D lightRange;
    [SerializeField] ParticleSystem particleSys;
    [SerializeField] ParticleSystem.EmissionModule emmisionMod;

    [SerializeField] Light2D[] particleLights;

    private void Start()
    {
        lightSource = GetComponentInParent<Light2D>();
        lightRange = GetComponentInParent<CircleCollider2D>();
        particleSys = lightRange.gameObject.GetComponentInChildren<ParticleSystem>();
        emmisionMod = particleSys.emission;
    }

    private void Update()
    {

        particleLights = particleSys.gameObject.GetComponentsInChildren<Light2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Debug.Log("collided");
            lightSource.enabled = false;
            lightRange.enabled = false;
            //particleSys.gameObject.SetActive(false);
            emmisionMod.rateOverTime = 0;


            //for (int i = 1; i < particleLights.Length; i++)
            //{
            //    Destroy(particleLights[i].gameObject, 2f);
            //}

        }
    }
}
