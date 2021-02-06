using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionScript : MonoBehaviour
{
    public PlayerStates playerStatesData;
    public PlayerHealth playerHealthData;
    public PlayerCollision playerCollisionData;

    public Transform playerBodyPosition;
    public Vector2 playerBodySize;
    public LayerMask damagingLayers;

    [Header("Hurt Stuff")]
    float hurtDuration;
    float invincibilityDuration;

    public NPCStats colliderStats;



    public int flashCounter = 4;
    public GameObject player;
    public SpriteRenderer[] sprites;

    bool invincibilityRunning = false;

    //public List<Collider2D> collisions;
    //public Collider2D[] collisions;
    //float a;

    public GameEvent eCollided;
    public GameEvent eEndHurt;
    public GameEvent eEndInvincibility;


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
            eEndHurt.Raise();
            //playerStatesData.isHurt = false;
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
            eEndInvincibility.Raise();
            //playerStatesData.isHurt = false;
        }




        ////IF NOT HURT
        //if (!playerStatesData.isHurt)
        //{
        //    //IF BODY OVERLAPS WITH DAMAGING LAYER
        //    if (Physics2D.OverlapBox(playerBodyPosition.position, playerBodySize, 0f, damagingLayers))
        //    {
        //        RaiseCollision();
        //    }
        //}

        //if (playerStatesData.isInvincible)
        //{
        //    StartInvulnerability();
        //}
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
            colliderStats = collision.gameObject.GetComponentInParent<NPCStatsScript>().npcStats;
            playerHealthData.healthLost = colliderStats.attackDamage;
            eCollided.Raise();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.layer == 6)
        {
            colliderStats = collision.gameObject.GetComponentInParent<NPCStatsScript>().npcStats;
            playerHealthData.healthLost = colliderStats.attackDamage;
            eCollided.Raise();
        }
    }

    public IEnumerator InvincibilityFrames()
    {
        invincibilityRunning = true;
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
        invincibilityRunning = false;
    }

    //public IEnumerator InvincibilityFrames()
    //{
    //    invincibilityRunning = true;
    //    SpriteRenderer[] sprites = player.GetComponentsInChildren<SpriteRenderer>();

    //    for (int i = 0; i < sprites.Length; i++)
    //    {
    //        while (sprites[i].color.a > 0.3f)
    //        {
    //            Color spriteColor = sprites[i].color;
    //            spriteColor.a -= Time.deltaTime;
    //            sprites[i].color = spriteColor;
    //            if (sprites[i].color.a <= 0.3f)
    //            {
    //                break;
    //            }
    //        }
    //        //invincibilityCounter--;
    //        yield return new WaitForSeconds(0.25f);

    //        while (sprites[i].color.a < 0.7f)
    //        {
    //            Color spriteColor = sprites[i].color;
    //            spriteColor.a += Time.deltaTime;
    //            sprites[i].color = spriteColor;
    //            if (sprites[i].color.a >= 0.7f)
    //            {
    //                break;
    //            }
    //        }

    //        yield return new WaitForSeconds(0.25f);

    //        while (sprites[i].color.a > 0.3f)
    //        {
    //            Color spriteColor = sprites[i].color;
    //            spriteColor.a -= Time.deltaTime;
    //            sprites[i].color = spriteColor;
    //            if (sprites[i].color.a <= 0.3f)
    //            {
    //                break;
    //            }
    //        }

    //        yield return new WaitForSeconds(0.25f);

    //        while (sprites[i].color.a < 1f)
    //        {
    //            Color spriteColor = sprites[i].color;
    //            spriteColor.a += Time.deltaTime;
    //            sprites[i].color = spriteColor;
    //            if (sprites[i].color.a >= 1f)
    //            {
    //                break;
    //            }
    //        }

    //        invincibilityRunning = false;
    //        eEndInvincibility.Raise();
    //    }
    //}
}
