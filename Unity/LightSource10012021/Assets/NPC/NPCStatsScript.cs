using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStatsScript : MonoBehaviour
{
    public NPCStatsData npcStatsData;

    public int currentHealth;
    public int healthLost;

    public float hurtDuration;

    public bool isHurt;
    [SerializeField] int flashCounter;
    [SerializeField] SpriteRenderer[] sprites;
    [SerializeField] [Range(0, 1)] float flashOpacity;

    private void OnValidate()
    {
        currentHealth = npcStatsData.maxHealth;
        sprites = GetComponentsInChildren<SpriteRenderer>();

        if (!npcStatsData.canFly)
        {
            GetComponent<NavMeshAgent2D>().enabled = false;
        }
        else
        {
            GetComponent<NavMeshAgent2D>().enabled = true;
        }
    }

    private void Update()
    {
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
            hurtDuration = npcStatsData.hurtDuration;
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
            yield return new WaitForSeconds(npcStatsData.hurtDuration / flashCounter / 2);
            for (int i = 0; i < sprites.Length; i++)
            {
                Color color;
                color = sprites[i].color;
                color.a = 1f;
                sprites[i].color = color;
            }
            yield return new WaitForSeconds(npcStatsData.hurtDuration / flashCounter / 2);
            temp++;
        }
    }

}
