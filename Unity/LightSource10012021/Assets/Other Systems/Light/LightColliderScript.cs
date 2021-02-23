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

    [SerializeField] int npcCollisionLayer;
    [SerializeField] int weaponDamageLayer;
    [SerializeField] int playerWeaponLayer;

    //[SerializeField] Light2D[] particleLights;

    private void Start()
    {
        lightSource = GetComponentInParent<Light2D>();
        lightRange = GetComponentInParent<CircleCollider2D>();
        particleSys = lightRange.gameObject.GetComponentInChildren<ParticleSystem>();
        emmisionMod = particleSys.emission;
    }

    private void Update()
    {

        //particleLights = particleSys.gameObject.GetComponentsInChildren<Light2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == weaponDamageLayer || collision.gameObject.layer == playerWeaponLayer)
        {
            LightOff();
            //lightSource.enabled = false;
            //lightRange.enabled = false;
            ////particleSys.gameObject.SetActive(false);
            //emmisionMod.rateOverTime = 0;


        }
        else if (collision.gameObject.layer == npcCollisionLayer)
        {
            LightOff();
            //lightSource.enabled = false;
            //lightRange.enabled = false;
            ////particleSys.gameObject.SetActive(false);
            //emmisionMod.rateOverTime = 0;


        }
    }

    public void LightOff()
    {
        lightSource.enabled = false;
        lightRange.enabled = false;
        //particleSys.gameObject.SetActive(false);
        emmisionMod.rateOverTime = 0;
    }
}
