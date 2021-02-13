using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    public PlayerStatesData playerStatesData;

    public void Attack()
    {
        if (!playerStatesData.isHurt)
        {
            if (playerStatesData.isArmed)
            {
                Debug.Log("Attacking");
            }
        }


    }
}
