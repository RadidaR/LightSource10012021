using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerHealthScript : MonoBehaviour
{
    public PlayerHealth playerHealthData;
    public PlayerStates playerStatesData;
    //public float maxHealth;
    //public float currentHealth;

    public float hurtDuration;

    public GameEvent eGotHurt;
    public GameEvent eEndHurt;
    public GameEvent eEndInvincibility;

    bool decreasingOpacity;
    bool increasingOpacity;

    //public GameEvent playerDeath;
    //public GameEvent takeDamage;

    void Start()
    {
        playerHealthData.currentHealth = playerHealthData.maxHealth;
    }


    public void Damage()
    {
        if (!playerStatesData.isHurt)
        {
            eGotHurt.Raise();
            hurtDuration = playerHealthData.hurtDuration;
            playerHealthData.currentHealth -= 10;
        }
    }

    public void InvincibilityFrames()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        SpriteRenderer[] sprites = player.GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < sprites.Length; i++)
        {

            while (sprites[i].color.a > 0.3f)
            {
                Color spriteColor = sprites[i].color;
                spriteColor.a -= Time.deltaTime;
                sprites[i].color = spriteColor;
                if (sprites[i].color.a <= 0.3f)
                {
                    break;
                }
            }
            eEndInvincibility.Raise();
        }
    }


    void Update()
    {
        if (hurtDuration > 0)
        {
            hurtDuration -= Time.deltaTime;
        }
        else if (hurtDuration < 0)
        {
            hurtDuration = 0;
        }
        else if (hurtDuration == 0)
        {
            playerStatesData.isHurt = false; 
        }

        if (playerStatesData.isInvincible)
        {
            InvincibilityFrames();
        }
    }
}
