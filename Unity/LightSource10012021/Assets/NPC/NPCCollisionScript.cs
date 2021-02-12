using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCollisionScript : MonoBehaviour
{
    public NPCStatsScript npcStatsScript;
    public NPCStatsData npcStatsData;

    public NPCStatsData npcCollisionData;
    public int npcCollisionLayer;

    public WeaponData weaponCollisionData;
    public int weaponCollisionLayer;

    // Start is called before the first frame update
    void Start()
    {
        npcStatsScript = GetComponentInParent<NPCStatsScript>();
        npcStatsData = npcStatsScript.npcStatsData;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.layer == npcCollisionLayer)
        //{
        //    npcCollisionData = collision.gameObject.GetComponentInParent<NPCStatsScript>().npcStatsData;
        //    //playerCollisionData.collisionStatsData = npcCollisionData;
        //    npcStatsScript.healthLost = npcCollisionData.collisionDamage;
        //    npcStatsScript.Damage();
        //}

        if (collision.gameObject.layer == weaponCollisionLayer)
        {
            weaponCollisionData = collision.gameObject.GetComponentInParent<WeaponScript>().weaponData;
            npcStatsScript.healthLost = weaponCollisionData.damage;
            npcStatsScript.Damage();
        }
    }
}
