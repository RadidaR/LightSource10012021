using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementModule : MonoBehaviour
{
    public NPCScript npc;
    public NPCType type;
    public Rigidbody2D body;


    private void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            npc = GetComponentInParent<NPCScript>();
            type = npc.type;
            body = npc.gameObject.GetComponentInChildren<Rigidbody2D>();
        }
    }
}
