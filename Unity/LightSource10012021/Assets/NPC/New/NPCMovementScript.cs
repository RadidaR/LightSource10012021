using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementScript : MonoBehaviour
{
    public GameObject npc;

    public NPCData data;
    public NPCAbilities abilities;

    public NPCStatesScript states;


    public void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            npc = GetComponentInParent<OfInterest>().gameObject;

            states = npc.GetComponent<NPCStatesScript>();

            data = states.data;
            abilities = states.abilities;
        }
    }
}
