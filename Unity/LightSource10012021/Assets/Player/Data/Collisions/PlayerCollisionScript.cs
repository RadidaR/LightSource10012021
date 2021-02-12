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
    [SerializeField] GameObject player;

    [Header("Invincibility Frames Variables")]
    [SerializeField] int flashCounter;
    [SerializeField] float invincibilityDuration;
    [SerializeField] SpriteRenderer[] sprites;

    [Header("Collisions Variables")]
    [SerializeField] float hurtDuration;
    [SerializeField] NPCStatsData npcCollisionData;
    [SerializeField] int npcCollisionLayer;
    [SerializeField] WeaponData weaponCollisionData;
    [SerializeField] int weaponCollisionLayer;


    private void Start()
    {
        player = GetComponentInParent<OfInterest>().gameObject;
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
        if (collision.gameObject.layer == npcCollisionLayer)
        {
            npcCollisionData = collision.gameObject.GetComponentInParent<NPCStatsScript>().npcStatsData;
            //playerCollisionData.collisionStatsData = npcCollisionData;
            playerHealthData.healthLost = npcCollisionData.collisionDamage;
            eCollided.Raise();
        }

        if (collision.gameObject.layer == weaponCollisionLayer)
        {
            weaponCollisionData = collision.gameObject.GetComponentInParent<WeaponScript>().weaponData;
            playerHealthData.healthLost = weaponCollisionData.damage;
            eCollided.Raise();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            npcCollisionData = collision.gameObject.GetComponentInParent<NPCStatsScript>().npcStatsData;
            //playerCollisionData.collisionStatsData = npcCollisionData;
            playerHealthData.healthLost = npcCollisionData.collisionDamage;
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
