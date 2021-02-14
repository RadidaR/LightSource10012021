using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour
{
    public PlayerStatesData playerStatesData;


    public GameObject player;
    public Animator animator;

    public string currentState;

    public string swordAttack1Animation;
    public GameEvent eAttackDone;
    public GameEvent eIsAttacking;

    public string idleAnimation;

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
        if (playerStatesData.isStill && currentState != swordAttack1Animation)
        {
            ChangeAnimationState(idleAnimation);
        }
    }

    public void PlaySwordAttack1()
    {
        //RaiseAttackDone();
        ChangeAnimationState(swordAttack1Animation);
    }

    public void PlayMoveAnimation()
    {
        RaiseAttackDone();
        ChangeAnimationState(moveLegs);
    }

    //public void PlayIdleAnimation()
    //{
    //        ChangeAnimationState(idleAnimation);
    //}

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }

        animator.Play(newState);

        currentState = newState;
    }

    public void RaiseIsAttacking()
    {
        eIsAttacking.Raise();
    }

    public void RaiseAttackDone()
    {
        eAttackDone.Raise();
        if (playerStatesData.isStill)
        {
            ChangeAnimationState(idleAnimation);
        }
    }



}
