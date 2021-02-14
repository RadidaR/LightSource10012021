using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    public PlayerStatesData playerStatesData;

    public GameEvent eAttack;

    public void Attack()
    {
        if (!playerStatesData.isHurt)
        {
            if (playerStatesData.isArmed)
            {
                eAttack.Raise();
            }
        }


    }
}
