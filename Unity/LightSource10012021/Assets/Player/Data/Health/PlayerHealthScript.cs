using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerHealthScript : MonoBehaviour
{
    //Take data from Scriptable Object
    public PlayerHealth playerHealthData;
    public float maxHealth;
    public float currentHealth;

    //Register events
    public GameEvent playerDeath;
    public GameEvent takeDamage;
    // Start is called before the first frame update

    void Start()
    {
        //Set starting values
        maxHealth = playerHealthData.maxHealth;
        currentHealth = maxHealth;
        playerHealthData.currentHealth = currentHealth;
    }

    //Called when takeDamage event is Raised
    public void Damage()
    {
        currentHealth -= 10;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Update Scriptable Object
        playerHealthData.currentHealth = currentHealth;
    }

    //Raises playerDeath event
    public void Die()
    {
        playerDeath.Raise();
    }
}
