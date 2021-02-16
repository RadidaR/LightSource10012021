using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttackScript : MonoBehaviour
{
    public GameObject npc;

    public NPCData data;
    public NPCAbilities abilities;

    public float currentAttackRange;

    private void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            npc = GetComponentInParent<OfInterest>().gameObject;

            data = npc.GetComponent<NPCStatesScript>().data;
            abilities = npc.GetComponent<NPCStatesScript>().abilities;

            if (abilities.canAttack)
            {
                if (abilities.standardAttack)
                {
                    currentAttackRange = data.standardAttackRange;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (currentAttackRange == 0)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currentAttackRange);
    }


}
