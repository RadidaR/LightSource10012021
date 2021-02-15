using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour
{
    public PlayerStatesData playerStatesData;


    public GameObject player;
    public Animator animator;

    public string PLAYER_IDLE;
    public string PLAYER_BORED;

    public float boredomCounter;


    public string currentState;

    public string swordAttack1Animation;
    public GameEvent eAttackDone;
    public GameEvent eIsAttacking;


    public string moveLegs;
    public string moveArms;
    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponentInParent<OfInterest>().gameObject;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerStatesData.isStill && currentState != swordAttack1Animation && currentState != PLAYER_IDLE && currentState != PLAYER_BORED)
        {
            ChangeAnimationState(PLAYER_IDLE);
            boredomCounter = Random.Range(3, 10);
        }

        if (boredomCounter < 0)
        {
            boredomCounter = 0;
        }

        if (currentState == PLAYER_IDLE && boredomCounter == 0)
        {
            PLAYER_BORED = PLAYER_IDLE + " " + Random.Range(1, 4).ToString();
            ChangeAnimationState(PLAYER_BORED);
        }

        if (boredomCounter > 0)
        {
            boredomCounter -= Time.deltaTime;
        }


    }

    public void PlaySwordAttack1()
    {
        //RaiseAttackDone();
        ChangeAnimationState(swordAttack1Animation);
    }

    public void PlayMoveAnimation()
    {
        boredomCounter = 0;
        //RaiseAttackDone();
        ChangeAnimationState(moveLegs);
    }

    public void PlayIdleAnimation()
    {
        boredomCounter = Random.Range(3, 10);
        ChangeAnimationState(PLAYER_IDLE);
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }
        currentState = newState;

        animator.Play(newState);
    }

    public void RaiseIsAttacking()
    {
        eIsAttacking.Raise();
    }

    public void RaiseAttackDone()
    {
        Debug.Log("Attack Done");
        eAttackDone.Raise();
        //if (playerStatesData.isStill)
        //{
        //    ChangeAnimationState(PLAYER_IDLE);
        //}
    }



}
