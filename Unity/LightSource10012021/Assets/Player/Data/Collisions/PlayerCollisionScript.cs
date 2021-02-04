using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionScript : MonoBehaviour
{
    public PlayerStates playerStatesData;

    public Transform playerBodyPosition;
    public Vector2 playerBodySize;
    public LayerMask damagingLayers;

    public GameEvent eEndInvincibility;

    public int invincibilityCounter = 4;
    public GameObject player;

    bool invincibilityRunning = false;

    //public List<Collider2D> collisions;
    //public Collider2D[] collisions;
    //float a;

    public GameEvent eCollided;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        ////IF NOT HURT
        //if (!playerStatesData.isHurt)
        //{
        //    //IF BODY OVERLAPS WITH DAMAGING LAYER
        //    if (Physics2D.OverlapBox(playerBodyPosition.position, playerBodySize, 0f, damagingLayers))
        //    {
        //        RaiseCollision();
        //    }
        //}

        if (playerStatesData.isInvincible)
        {
            StartInvulnerability();
        }
    }

    public void RaiseCollision()
    {
        eCollided.Raise();
    }

    void OnDrawGizmosSelected()
    {
        if (playerBodySize == Vector2.zero)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerBodyPosition.position, playerBodySize);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerStatesData.isHurt)
        {
            if (collision.gameObject.layer == 6)
            {
                RaiseCollision();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("Collision" + collision.gameObject.layer.ToString());
        if (!playerStatesData.isHurt)
        {
            if (collision.gameObject.layer == 6)
            {
                RaiseCollision();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision" + collision.gameObject.layer.ToString());
    }

    public void StartInvulnerability()
    {
        if (!invincibilityRunning)
        {
            StartCoroutine(InvincibilityFrames());
        }
    }

    public IEnumerator InvincibilityFrames()
    {
        invincibilityRunning = true;
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
            //invincibilityCounter--;
            yield return new WaitForSeconds(0.25f);

            while (sprites[i].color.a < 0.7f)
            {
                Color spriteColor = sprites[i].color;
                spriteColor.a += Time.deltaTime;
                sprites[i].color = spriteColor;
                if (sprites[i].color.a >= 0.7f)
                {
                    break;
                }
            }

            yield return new WaitForSeconds(0.25f);

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

            yield return new WaitForSeconds(0.25f);

            while (sprites[i].color.a < 1f)
            {
                Color spriteColor = sprites[i].color;
                spriteColor.a += Time.deltaTime;
                sprites[i].color = spriteColor;
                if (sprites[i].color.a >= 1f)
                {
                    break;
                }
            }

            invincibilityRunning = false;
            eEndInvincibility.Raise();
        }
    }
    //GameObject player = gameObject.GetComponentInParent<GameObject>();
    

    //public void InvincibilityFrame2s()
    //{
    //    //GameObject player = GameObject.FindGameObjectWithTag("Player");
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
    //        eEndInvincibility.Raise();
    //    }
    //}
}
