using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollisionScript : MonoBehaviour
{
    public WeaponScript weaponScript;
    public WeaponData weaponData;

    public NPCStatsData npcCollisionData;
    public int npcCollisionLayer;

    public float invincibilityDuration;
    public bool invincible;
    // Start is called before the first frame update
    void Start()
    {
        weaponScript = GetComponentInParent<WeaponScript>();
        weaponData = weaponScript.weaponData;
    }

    private void Update()
    {
        if (invincibilityDuration > 0)
        {            
            invincibilityDuration -= Time.deltaTime;
        }
        else
        {
            invincibilityDuration = 0;
            invincible = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == npcCollisionLayer)
        {
            if (!invincible)
            {
                invincible = true;
                if (collision.GetComponentInParent<OfInterest>().gameObject.tag == "Player")
                {
                    GameObject player = collision.GetComponentInParent<OfInterest>().gameObject;
                    invincibilityDuration = player.GetComponentInChildren<PlayerCollisionScript>().playerCollisionData.invincibilityDuration;
                    weaponScript.durability -= 1;
                }
                else if (collision.GetComponentInParent<OfInterest>().gameObject.tag == "NPC")
                {
                    GameObject NPC = collision.GetComponentInParent<OfInterest>().gameObject;
                    invincibilityDuration = NPC.GetComponentInChildren<NPCCollisionScript>().npcStatsData.hurtDuration;
                    weaponScript.durability -= 1;
                }
                if (weaponScript.durability <= 0)
                {
                    weaponScript.Break();
                }
            }
        }
    }
}
