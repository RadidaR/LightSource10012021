using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerHealthScript : MonoBehaviour
{
    [Header("Data Types")]
    public PlayerHealthData playerHealthData;
    public PlayerStatesData playerStatesData;

    [Header("Events")]
    public GameEvent eGotHurt;
    public GameEvent eEndInvincibility;

    void Start()
    {
        playerHealthData.currentHealth = playerHealthData.maxHealth;
    }

    public void Damage()
    {
        if (!playerStatesData.isInvincible)
        {
            playerHealthData.currentHealth -= playerHealthData.healthLost;
            eGotHurt.Raise();
        }
    }
}
