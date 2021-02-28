using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttackScript : MonoBehaviour
{
    public GameObject npc;
    public NPCStatesScript states;
    public NPCMovementScript movement;
    public SeekTargetScript2 seekTarget;
    public Rigidbody2D rigidBody;

    public NPCAwarenessScript awareness;

    public NPCData data;
    public NPCAbilities abilities;

    public JumpAttackScript jumpAttackData;

    //public ShowCurveScript curve;

    public LayerMask groundLayer;

    //public int attackOrder;

    public int standardAttack = 0;
    public int weaponAttack = 0;
    public int rangedAttack = 0;
    public int chargeAttack = 0;
    public int jumpAttack = 0;
    public int burrowAttack = 0;


    public List<int> attackPattern;

    public bool attackRunning;

    public int nextAttack;
    public AttackData nextAttackData;

    //public float attackRange;
    //public int attackDamage;
    //public float telegraphDuration;
    //public float attackLength;
    //public float attackCooldown;

    public Vector2 targetPosition;
    //public string targetType;
    public float currentCooldown;

    private void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            npc = GetComponentInParent<OfInterest>().gameObject;
            states = npc.GetComponent<NPCStatesScript>();
            movement = npc.GetComponentInChildren<NPCMovementScript>();
            seekTarget = npc.GetComponentInChildren<SeekTargetScript2>();
            rigidBody = npc.GetComponent<Rigidbody2D>();
            awareness = npc.GetComponentInChildren<NPCAwarenessScript>();

            data = npc.GetComponent<NPCStatesScript>().data;
            abilities = npc.GetComponent<NPCStatesScript>().abilities;

            //curve = GetComponentInChildren<ShowCurveScript>();

            if (!this.enabled)
            {
                standardAttack = 0;
                weaponAttack = 0;
                rangedAttack = 0;
                chargeAttack = 0;
                jumpAttack = 0;
                burrowAttack = 0;
            }

            if (abilities.canAttack && this.enabled)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (abilities.standardAttack && standardAttack == 0)
                    {
                        standardAttack = i;
                    }
                    else if (abilities.weaponAttack && weaponAttack == 0)
                    {
                        weaponAttack = i;
                    }
                    else if (abilities.rangedAttack && rangedAttack == 0)
                    {
                        rangedAttack = i;
                    }
                    else if (abilities.chargeAttack && chargeAttack == 0)
                    {
                        chargeAttack = i;
                    }
                    else if (abilities.jumpAttack && jumpAttack == 0)
                    {
                        jumpAttack = i;
                    }
                    else if (abilities.burrowAttack && burrowAttack == 0)
                    {
                        burrowAttack = i;
                    }
                }
            }

            if (abilities.jumpAttack)
            {
                jumpAttackData = GetComponentInChildren<JumpAttackScript>();
            }
        }
    }

    private void Start()
    {
        nextAttack = attackPattern[0];

        if (nextAttack == standardAttack)
        {
            nextAttack = standardAttack;
        }
        else if (nextAttack == weaponAttack)
        {
            nextAttack = weaponAttack;
        }
        else if (nextAttack == rangedAttack)
        {
            nextAttack = rangedAttack;
        }
        else if (nextAttack == chargeAttack)
        {
            nextAttack = chargeAttack;
        }
        else if (nextAttack == jumpAttack)
        {
            nextAttack = jumpAttack;
            nextAttackData = jumpAttackData.data;
        }
        else if (nextAttack == burrowAttack)
        {
            nextAttack = burrowAttack;
        }
    }

    private void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        else
        {
            currentCooldown = 0;
        }
    }

    public void LaunchAttack(GameObject target)
    {
        //CHECK IF ATTACK IS ON COOL DOWN
        if (currentCooldown == 0)
        {
            //START ATTACK AT GIVEN TARGET, PASSES ATTACK DATA
            StartCoroutine(Attack(target, nextAttack, nextAttackData.telegraph, nextAttackData.damage, nextAttackData.length, nextAttackData.cooldown));
        }
    }

    public void UpdateAttackPattern()
    {
        //PUT LAST ATTACK MADE AT THE BOTTOM OF THE QUEUE
        attackPattern.Remove(attackPattern[0]);
        attackPattern.Add(nextAttack);

        //PREPARE NEXT ATTACK'S DATA
        if (attackPattern[0] == standardAttack)
        {
            nextAttack = standardAttack;
        }
        else if (attackPattern[0] == weaponAttack)
        {
            nextAttack = weaponAttack;
        }
        else if (attackPattern[0] == rangedAttack)
        {
            nextAttack = rangedAttack;
        }
        else if (attackPattern[0] == chargeAttack)
        {
            nextAttack = chargeAttack;
        }
        else if (attackPattern[0] == jumpAttack)
        {
            nextAttack = jumpAttack;
            nextAttackData = jumpAttackData.data;
        }
        else if (attackPattern[0] == burrowAttack)
        {
            nextAttack = burrowAttack;
        }
    }


    public IEnumerator Attack(GameObject target, int attack, float telegraph, int damage, float length, float cooldown)
    {        
        //MAKE SURE ATTACK ISN'T RUNNING ALREADY
        if (!attackRunning)
        {
            //TURN COROUTINE BOOLEAN ON
            attackRunning = true;
            //STOP IN PLACE
            movement.StopMoving();

            //START TELEGRAPHING
            states.isTelegraphing = true;

            //CHECK WHAT ATTACK TO MAKE
            //
            //JUMP ATTACK
            if (attack == jumpAttack)
            {
                //ARRAY OF POINTS THAT WILL DETERMINE MOVEMENT LATER
                Vector3[] jumpPath;

                //WHILE TELEGRAPHING
                if (states.isTelegraphing)
                {
                    while (telegraph > 0)
                    {
                        //STAY STILL
                        movement.StopMoving();                        

                        //IF TARGET IS NOT VISIBLE
                        if (!awareness.targetInSight(target, target.transform.position))
                        {
                            //STOP TELEGRAPHING
                            states.isTelegraphing = false;
                            //RESET CURVE
                            jumpAttackData.ResetCurve();
                            //AND BREAK
                            break;
                        }
                        //IF VISIBLE
                        else
                        {
                            //STAY STILL
                            movement.StopMoving();

                            //IF DISTANCE TO TARGET IS LESS THAN JUMP'S MAX DISTANCE
                            if (Vector3.Distance(npc.transform.position, target.transform.position) < jumpAttackData.data.maxDistance)
                            {
                                //CALCULATE AND DRAW PATH
                                jumpAttackData.CalculatePath(npc.transform.position, target, 150);
                            }
                            //IF BEYOND MAX JUMP DISTANCE
                            else
                            {
                                //RESET CURVE
                                jumpAttackData.ResetCurve();
                            }
                        }

                        //TICK AWAY FROM TELEGRAPHING TIME
                        telegraph -= Time.fixedDeltaTime;
                        //WAIT THE TICKET AWAY TIME
                        yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);

                        //IF TELEGRAPH HAS COUNTED DOWN
                        if (telegraph <= 0)
                        {
                            //CHECK IF TARGET STILL IN JUMPING RANGE
                            if (Vector2.Distance(npc.transform.position, target.transform.position) < jumpAttackData.data.maxDistance)
                            {
                                //START ATTACKING
                                states.isAttacking = true;
                            }
                            //STOP TELEGRAPHING
                            states.isTelegraphing = false;
                            //RESET CURVE
                            jumpAttackData.ResetCurve();
                            //AND BREAK
                            break;
                        }
                    }
                }

                //WHILE ATTACKING
                if (states.isAttacking)
                {
                    //MAKE JUMP PATH ARRAY THE SAME SIZE AS JUMP ATTACK'S LINE RENDERER POSITIONS
                    jumpPath = new Vector3[jumpAttackData.lineRenderer.positionCount];

                    //ASSIGN EACH POSITION TO JUMP PATH
                    for (int i = 0; i < jumpAttackData.lineRenderer.positionCount - 1; i++)
                    {
                        jumpPath[i] = jumpAttackData.lineRenderer.GetPosition(i);
                    }

                    //CHECK EACH POSITION
                    for (int i = 0; i < jumpPath.Length; i++)
                    {
                        //ASSIGN CONSTRAINT SO NPC CAN JUMP OFF WALLS
                        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                        //MAKE KINEMATIC
                        rigidBody.isKinematic = true;
                        //MAKE SURE CHECKED POINT ISN'T 0,0,0
                        if (jumpPath[i] != Vector3.zero)
                        {
                            //MOVE TO POINT
                            rigidBody.MovePosition(jumpPath[i]);
                        }
                        //WAIT TIME BASED ON JUMP ATTACK'S LENGTH
                        yield return new WaitForSecondsRealtime(Time.fixedDeltaTime * length);
                    }
                    //WHEN DONE
                    //END ATTACK
                    states.isAttacking = false;
                    //SET COOLDOWN
                    currentCooldown = cooldown;
                    //STOP COROUTINE BOOLEAN
                    attackRunning = false;
                    //MAKE RIGIDBODY DYNAMIC AGAIN
                    rigidBody.isKinematic = false;
                    //STOP MOVING
                    movement.StopMoving();
                    //AND END COROUTINE
                    StopCoroutine("Attack");
                }

                //IF NEITHER TELEGRAPHING, NOR ATTACKING & TARGET IS OUT OF SIGHT
                if (!awareness.targetInSight(target, target.transform.position) && !states.isTelegraphing && !states.isAttacking)
                {
                    //LOOK FOR CLEAR PATH FOR 1 SECOND
                    float findClearPath = 1f;

                    //WHILE FIND PATH TIMER HAS NOT RUN OUT
                    while (findClearPath > 0)
                    {
                        //MOVE IN FACING DIRECTION AT RUN SPEED
                        movement.Move(data.runSpeed, target.transform.position);

                        //TICK AWAY AT FIND PATH TIMER IF NOT CLIMBING (PREVENTS IT FROM FALLING OFF WALLS)
                        if (!states.isClimbing)
                        {
                            findClearPath -= Time.fixedDeltaTime;
                        }
                        //WAIT TICKED TIME 
                        yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
                        //IF TIMER HAS RUN OUT
                        if (findClearPath <= 0)
                        {
                            //BREAK
                            break;
                        }
                        //ELSE IF TARGET IS IN SIGHT
                        else if (awareness.targetInSight(target, target.transform.position))
                        {
                            //STOP COROUTINE BOOLEAN
                            attackRunning = false;
                            //MAKE RIGIDBODY DYNAMIC
                            rigidBody.isKinematic = false;
                            //STOP MOVING
                            movement.StopMoving();
                            //END COROUTINE 
                            StopCoroutine("Attack");
                            //BREAK
                            break;
                        }
                    }
                }
            }

            //END ATTACK
            states.isAttacking = false;
            //MAKE RIGIDBODY DYNAMIC AGAIN
            rigidBody.isKinematic = false;
            //UPDATE ATTACK PATTERN
            UpdateAttackPattern();
            //AND TURN COROUTINE BOOLEAN OFF
            attackRunning = false;
        }

    }

    void OnDrawGizmosSelected()
    {
        if (nextAttackData != null)
        {
            if (nextAttackData.range == 0)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, nextAttackData.range);
        }
    }
}

