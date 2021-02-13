using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollisionScript : MonoBehaviour
{
    public WeaponScript weaponScript;
    public WeaponData weaponData;

    public NPCStatsData npcCollisionData;
    public int npcCollisionLayer;
    public int playerBodyLayer;

    public float unbreakableDuration;
    public bool unBreakable;
    // Start is called before the first frame update
    void Start()
    {
        weaponScript = GetComponentInParent<WeaponScript>();
        weaponData = weaponScript.weaponData;
    }

    private void Update()
    {
        if (unbreakableDuration > 0)
        {            
            unbreakableDuration -= Time.deltaTime;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            unbreakableDuration = 0;
            unBreakable = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == npcCollisionLayer || collision.gameObject.layer == playerBodyLayer)
        {
            if (!unBreakable)
            {
                unBreakable = true;
                if (collision.tag == "Body")
                {
                    GameObject hit = collision.GetComponentInParent<OfInterest>().gameObject;
                    if (hit.tag == "Player")
                    {
                        unbreakableDuration = collision.GetComponent<PlayerCollisionScript>().playerCollisionData.invincibilityDuration;
                    }
                    else if (hit.tag == "NPC")
                    {
                        unbreakableDuration = collision.GetComponent<NPCCollisionScript>().npcStatsData.hurtDuration;
                    }
                }
                weaponScript.durability -= 1;
            }


            if (weaponScript.durability <= 0)
            {
                weaponScript.Break();
            }


        }

        //if (collision.gameObject.layer == npcCollisionLayer)
        //{
        //    if (!invincible)
        //    {
        //        invincible = true;
        //        if (collision.GetComponentInParent<OfInterest>().gameObject.tag == "Player")
        //        {
        //            GameObject player = collision.GetComponentInParent<OfInterest>().gameObject;
        //            invincibilityDuration = player.GetComponentInChildren<PlayerCollisionScript>().playerCollisionData.invincibilityDuration;
        //            weaponScript.durability -= 1;
        //        }
        //        else if (collision.GetComponentInParent<OfInterest>().gameObject.tag == "NPC")
        //        {
        //            GameObject NPC = collision.GetComponentInParent<OfInterest>().gameObject;
        //            invincibilityDuration = NPC.GetComponentInChildren<NPCCollisionScript>().npcStatsData.hurtDuration;
        //            weaponScript.durability -= 1;
        //        }
        //        if (weaponScript.durability <= 0)
        //        {
        //            weaponScript.Break();
        //        }
        //    }
        //}
    }
}
