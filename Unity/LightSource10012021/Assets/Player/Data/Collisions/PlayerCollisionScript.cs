using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionScript : MonoBehaviour
{
    [Header("Data Types")]
    public PlayerStatesData playerStatesData;
    public PlayerHealthData playerHealthData;
    public PlayerCollisionData playerCollisionData;

    [Header("Events")]
    public GameEvent eCollided;
    public GameEvent eEndHurt;
    public GameEvent eEndInvincibility;

    [Header("Local Variables")]
    [SerializeField] int flashCounter;
    [SerializeField] float hurtDuration;
    [SerializeField] float invincibilityDuration;
    [SerializeField] NPCStats collisionStats;
    [SerializeField] GameObject player;
    [SerializeField] SpriteRenderer[] sprites;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sprites = player.GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
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
            if (playerStatesData.isHurt)
            {
                eEndHurt.Raise();
            }
        }

        if (invincibilityDuration > 0)
        {
            invincibilityDuration -= Time.deltaTime;
        }
        else if (invincibilityDuration < 0)
        {
            invincibilityDuration = 0;
        }
        else if (invincibilityDuration == 0)
        {
            if (playerStatesData.isInvincible)
            {
                eEndInvincibility.Raise();
            }
        }
    }

    public void GotHurtCountdowns()
    {
        hurtDuration = playerCollisionData.hurtDuration;
        invincibilityDuration = playerCollisionData.invincibilityDuration;
        StartCoroutine(InvincibilityFrames());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            collisionStats = collision.gameObject.GetComponentInParent<NPCStatsScript>().npcStats;
            playerCollisionData.collisionStats = collisionStats;
            playerHealthData.healthLost = collisionStats.attackDamage;
            eCollided.Raise();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            collisionStats = collision.gameObject.GetComponentInParent<NPCStatsScript>().npcStats;
            playerCollisionData.collisionStats = collisionStats;
            playerHealthData.healthLost = collisionStats.attackDamage;
            eCollided.Raise();
        }
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
                color.a = 0.3f;
                sprites[i].color = color;
            }
            yield return new WaitForSeconds(playerCollisionData.invincibilityDuration / flashCounter / 2);
            for (int i = 0; i < sprites.Length; i++)
            {
                Color color;
                color = sprites[i].color;
                color.a = 1f;
                sprites[i].color = color;
            }
            yield return new WaitForSeconds(playerCollisionData.invincibilityDuration / flashCounter / 2);
            temp++;
        }
    }
}
