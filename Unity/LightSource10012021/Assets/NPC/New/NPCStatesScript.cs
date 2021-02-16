using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStatesScript : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public NPCData data;
    public NPCAbilities abilities;

    [Header("NPC Type")]
    [SerializeField] string name;
    [SerializeField] string type;

    [Header("Health")]
    [SerializeField] int currentHealth;
    [SerializeField] int healthLost;

    [Header("States")]
    [Range(-1, 1)][SerializeField] public int facingDirection;
    [SerializeField] public bool isIdle;
    [SerializeField] public bool isChasing;
    [SerializeField] public bool isTelegraphing;
    [SerializeField] public bool isAttacking;
    [SerializeField] public bool isHurt;
    [SerializeField] public bool isArmed;

    [Header("State Durations")]
    [SerializeField] float hurtDuration;

    [Header("Invincibility Frames")]
    [SerializeField] int flashCounter;
    [SerializeField] SpriteRenderer[] sprites;
    [SerializeField] [Range(0, 1)] float flashOpacity;


    private void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            sprites = GetComponentsInChildren<SpriteRenderer>();
            name = data.name;
            if (data.zombie)
            {
                type = "Zombie";
            }
            else if (data.human)
            {
                type = "Human";
            }
        }
    }

    void Start()
    {
        currentHealth = data.maxHealth;
    }

    private void Update()
    {
        if (gameObject.transform.localScale.x > 0)
        {
            facingDirection = 1;
        }
        else if (gameObject.transform.localScale.x < 0)
        {
            facingDirection = -1;
        }

        if (!isChasing && !isTelegraphing && !isAttacking && !isHurt)
        {
            isIdle = true;
        }
        else
        {
            isIdle = false;
        }
        
        if (hurtDuration > 0)
        {
            isHurt = true;
            hurtDuration -= Time.deltaTime;
        }
        if (hurtDuration <= 0)
        {
            hurtDuration = 0;
            isHurt = false;
        }
    }

    public void Damage()
    {
        if (!isHurt)
        {
            currentHealth -= healthLost;
            hurtDuration = data.hurtDuration;
            StartCoroutine(InvincibilityFrames());
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        Destroy(gameObject, 0.25f);
    }

    public IEnumerator InvincibilityFrames()
    {
        int temp = 0;
        while (temp < flashCounter)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                Color color;
                color = sprites[i].color;
                color.a = flashOpacity;
                sprites[i].color = color;
            }
            yield return new WaitForSeconds(data.hurtDuration / flashCounter / 2);
            for (int i = 0; i < sprites.Length; i++)
            {
                Color color;
                color = sprites[i].color;
                color.a = 1f;
                sprites[i].color = color;
            }
            yield return new WaitForSeconds(data.hurtDuration / flashCounter / 2);
            temp++;
        }
    }
}
