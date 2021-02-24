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
    public float cooldown;

    private void OnValidate()
    {
        if (gameObject.activeInHierarchy)
        {
            npc = GetComponentInParent<OfInterest>().gameObject;
            states = npc.GetComponent<NPCStatesScript>();
            movement = npc.GetComponentInChildren<NPCMovementScript>();
            seekTarget = npc.GetComponentInChildren<SeekTargetScript2>();
            rigidBody = npc.GetComponent<Rigidbody2D>();

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
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            cooldown = 0;
        }
    }

    public void LaunchAttack(GameObject target)
    {
        //CHECK IF ATTACK IS ON COOL DOWN
        if (cooldown == 0)
        {
            //START ATTACK AT GIVEN TARGET, PASSES ATTACK DATA
            StartCoroutine(Attack(target, nextAttack, nextAttackData.telegraph, nextAttackData.damage, nextAttackData.length));
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


    public IEnumerator Attack(GameObject target, int attack, float telegraph, int damage, float length)
    {        
        //MAKE SURE ATTACK ISN'T RUNNING ALREADY
        if (!attackRunning)
        {
            //RUN THE ATTACK
            attackRunning = true;
            //STOP IN PLACE
            movement.StopMoving();

            ////CHECK STATE
            if (!states.isTelegraphing && !states.isAttacking)
            {
                //IF NEITHER ATTACKING NOR 
                states.isTelegraphing = true;
                states.isAttacking = false;
            }

            //START TELEGRAPHING
            //states.isTelegraphing = true;

            //if (targetInSight(target))
            //{
            //    Debug.Log("Target in sight");
            //}
            
            //if (!targetInSight(target))
            //{
            //    Debug.Log("Target not in sight");
            //}

            //CHECK WHAT ATTACK TO MAKE
            //
            //JUMP ATTACK
            if (attack == jumpAttack)
            {
                //SET UP 4 POINTS TO CALCULATE JUMP CURVE
                Vector3 jumpStart = npc.transform.position;
                Vector3 jumpStartControl = npc.transform.position;
                Vector3 jumpEndControl = target.transform.position;
                Vector3 jumpEnd = target.transform.position;

                //GameObject top = target.transform.FindChild("Top").gameObject;
                //Debug.Log(top.name.ToString());

                //CHECK IF NOTHING OBSTRUCTS VIEW

                //if (targetInSight(target))
                //{
                //    Debug.Log("I can see you");
                //}
                //else
                //{
                //    Debug.Log("it's hidden");
                //}

                //RaycastHit2D obstaclesToTarget;

                //if (target.tag == "Player")
                //{
                //    obstaclesToTarget = Physics2D.Raycast(new Vector2(jumpStart.x, jumpStart.y + 3), new Vector3(jumpEnd.x, jumpEnd.y + 4) - jumpStart, Vector2.Distance(jumpStart, jumpEnd), groundLayer);
                //}
                //else
                //{
                //    obstaclesToTarget = Physics2D.Raycast(new Vector2(jumpStart.x, jumpStart.y + 3), jumpEnd - jumpStart, Vector2.Distance(jumpStart, jumpEnd), groundLayer);
                //}

                if (states.isTelegraphing)
                {
                    while (telegraph > 0)
                    {
                        movement.StopMoving();

                        Vector3 curveStart = npc.transform.position;
                        Vector3 curveEnd;

                        if (target.tag == "Player")
                        {
                            curveEnd = new Vector3(target.transform.position.x, target.transform.position.y + 4);
                        }
                        else
                        {
                            curveEnd = target.transform.position;
                        }

                        float distanceToTargetX = Mathf.Abs(curveStart.x - curveEnd.x);
                        float distanceToTargetY = Mathf.Abs(curveStart.y - curveEnd.y);

                        Vector3 curveStartControl;
                        Vector3 curveEndControl;

                        if (curveStart.y > curveEnd.y && Mathf.Abs(curveStart.y - curveEnd.y) > 10)
                        {
                            curveStartControl = new Vector3(curveStart.x + (distanceToTargetX * 0.25f * states.facingDirection), curveStart.y/* + (distanceToTargetX / 5) + (distanceToTargetY / 2)*/);
                            curveEndControl = new Vector3(curveEnd.x - (distanceToTargetX * 0.25f * states.facingDirection), curveEnd.y + (distanceToTargetX / 5) + (distanceToTargetY / 2));
                        }
                        else if (curveStart.y < curveEnd.y && Mathf.Abs(curveStart.y - curveEnd.y) > 5)
                        {
                            curveStartControl = new Vector3(curveStart.x + (distanceToTargetX * 0.25f * states.facingDirection), curveStart.y + (distanceToTargetX / 5) + (distanceToTargetY / 4));
                            curveEndControl = new Vector3(curveEnd.x - (distanceToTargetX * 0.25f * states.facingDirection), curveEnd.y);
                        }
                        else
                        {
                            curveStartControl = new Vector3(curveStart.x + (distanceToTargetX * 0.25f * states.facingDirection), curveStart.y + (distanceToTargetX / 5) + (distanceToTargetY / 2));
                            curveEndControl = new Vector3(curveEnd.x - (distanceToTargetX * 0.25f * states.facingDirection), curveEnd.y + (distanceToTargetX / 5) + (distanceToTargetY / 2));
                        }

                        //Vector3 curveStart = npc.transform.position;
                        //Vector3 curveStartControl;
                        //Vector3 curveEndControl;
                        //Vector3 curveEnd;


                        //Vector2 targetBot = GetNamedChild(target, "Bottom").transform.position;

                        //RaycastHit2D checkTargetAltitude = Physics2D.Raycast(target.transform.position, Vector2.down, jumpAttackData.data.range, groundLayer);

                        //if (checkTargetAltitude)
                        //{
                        //    //Debug.Log(checkTargetAltitude.distance.ToString());
                        //    if (checkTargetAltitude.distance < 5)
                        //    {

                        //    }
                        //}

                        //bool targetOnGround;

                        if (!targetInSight(target, target.transform.position))
                        {
                            //Debug.Log("Target not in sight");
                            //movement.Move(data.runSpeed, targetPosition);
                            states.isTelegraphing = false;
                            jumpAttackData.ResetCurve();
                            //attackRunning = false;
                            //StopCoroutine("Attack");
                            break;
                        }
                        else
                        {
                            movement.StopMoving();
                            jumpAttackData.lineRenderer.enabled = true;

                            if (Vector2.Distance(curveStart, curveEnd) < jumpAttackData.data.maxDistance)
                            {
                                jumpAttackData.DrawCurve(curveStart, curveStartControl, curveEndControl, curveEnd);
                            }
                            else
                            {
                                jumpAttackData.ResetCurve();
                            }
                        }

                        telegraph -= Time.fixedDeltaTime;
                        yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);

                        if (telegraph <= 0)
                        {
                            if (Vector2.Distance(curveStart, curveEnd) < jumpAttackData.data.maxDistance)
                            {
                                jumpStart = curveStart;
                                jumpStartControl = curveStartControl;
                                jumpEndControl = curveEndControl;
                                jumpEnd = curveEnd;
                                states.isAttacking = true;
                            }
                            states.isTelegraphing = false;
                            jumpAttackData.ResetCurve();
                            break;
                        }
                    }
                }

                RaycastHit2D obstaclesBetweenControlPoints = Physics2D.Raycast(jumpStartControl, jumpEndControl - jumpStartControl, Vector2.Distance(jumpStartControl, jumpEndControl), groundLayer);

                if (states.isAttacking)
                {
                    float tParam = 0;
                    int currentFacingDirection = states.facingDirection;

                    Vector2 jumpTarget;
                    Vector2 targetTop = GetNamedChild(target, "Top").transform.position;
                    Vector2 targetMid = GetNamedChild(target, "Mid").transform.position;
                    Vector2 targetBot = GetNamedChild(target, "Bottom").transform.position;
                    RaycastHit2D wall1 = Physics2D.Raycast(targetTop, new Vector2(currentFacingDirection, 0), 5.5f, groundLayer);
                    RaycastHit2D wall2 = Physics2D.Raycast(targetMid, new Vector2(currentFacingDirection, 0), 5.5f, groundLayer);
                    RaycastHit2D wall3 = Physics2D.Raycast(targetBot, new Vector2(currentFacingDirection, 0), 5.5f, groundLayer);

                    while (tParam < 1)
                    {

                        if (target.tag == "Player")
                        {

                            bool playerNextToWall;

                            if (!wall1 && !wall2 && !wall3)
                            {
                                playerNextToWall = false;
                            }
                            else
                            {
                                playerNextToWall = true;
                            }

                            if (playerNextToWall)
                            {
                                jumpTarget = jumpAttackData.CalculateCurve(tParam, jumpStart, jumpStartControl, jumpEndControl, new Vector2(jumpEnd.x, jumpEnd.y + 1));
                            }
                            else
                            {
                                jumpTarget = jumpAttackData.CalculateCurve(tParam, jumpStart, jumpStartControl, jumpEndControl, new Vector2(jumpEnd.x + 5 * currentFacingDirection, jumpEnd.y - 3));
                            }
                        }
                        else
                        {
                            jumpTarget = jumpAttackData.CalculateCurve(tParam, jumpStart, jumpStartControl, jumpEndControl, jumpEnd);
                        }                                           

                        if (targetInSight(null, jumpStartControl) && !obstaclesBetweenControlPoints)
                        {
                            tParam += Time.fixedDeltaTime * 1 / length;
                            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                            rigidBody.isKinematic = true;
                            rigidBody.MovePosition(jumpTarget);
                        }
                        else
                        {
                            states.isAttacking = false;
                            break;
                        }                        

                        yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
                    }

                    if (tParam >= 1)
                    {
                        cooldown = nextAttackData.cooldown;
                        attackRunning = false;
                        rigidBody.isKinematic = false;
                        movement.StopMoving();
                        StopCoroutine("Attack");
                    }
                }

                if ((!targetInSight(target, target.transform.position) || obstaclesBetweenControlPoints) && !states.isTelegraphing && !states.isAttacking)
                {
                    float findClearPath = 1f;
                    while (findClearPath > 0)
                    {
                        movement.Move(data.runSpeed, target.transform.position);
                        if (!states.isClimbing)
                        {
                            findClearPath -= Time.fixedDeltaTime;
                        }
                        yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
                        if (findClearPath <= 0)
                        {
                            break;
                        }
                        else if (targetInSight(target, target.transform.position) && !obstaclesBetweenControlPoints)
                        {
                            attackRunning = false;
                            rigidBody.isKinematic = false;
                            movement.StopMoving();
                            StopCoroutine("Attack");
                            break;
                        }
                    }
                }
            }
            states.isAttacking = false;
            rigidBody.isKinematic = false;
            //cooldown = nextAttackData.cooldown;
            UpdateAttackPattern();
            attackRunning = false;
        }
    }


    public bool targetInSight(GameObject target, Vector2 position)
    {
        Vector2 eyeLevel = GetNamedChild(npc, "EyeLevel").transform.position;

        if (target != null)
        {
            Vector2 targetTop = GetNamedChild(target, "Top").transform.position;
            Vector2 targetMid = GetNamedChild(target, "Mid").transform.position;
            Vector2 targetBot = GetNamedChild(target, "Bottom").transform.position;

            RaycastHit2D obstaclesToTop = Physics2D.Raycast(eyeLevel, targetTop - eyeLevel, Vector2.Distance(eyeLevel, targetTop), groundLayer);
            RaycastHit2D obstaclesToMid = Physics2D.Raycast(eyeLevel, targetMid - eyeLevel, Vector2.Distance(eyeLevel, targetMid), groundLayer);
            RaycastHit2D obstaclesToBot = Physics2D.Raycast(eyeLevel, targetBot - eyeLevel, Vector2.Distance(eyeLevel, targetBot), groundLayer);

            if (!obstaclesToTop || !obstaclesToMid || !obstaclesToBot)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            RaycastHit2D obstaclesToTarget = Physics2D.Raycast(eyeLevel, position - eyeLevel, Vector2.Distance(eyeLevel, position), groundLayer);

            if (!obstaclesToTarget)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public GameObject GetNamedChild(GameObject parentObject, string childName)
    {
        GameObject childObject = parentObject;

        if (parentObject.transform.childCount != 0)
        {
            for (int i = 0; i < parentObject.transform.childCount; i++)
            {
                if (parentObject.transform.GetChild(i).gameObject.name == childName)
                {
                    childObject = parentObject.transform.GetChild(i).gameObject;
                    return childObject;
                }
                else if (parentObject.transform.GetChild(i).childCount != 0)
                {
                    for (int j = 0; j < parentObject.transform.GetChild(i).childCount; j++)
                    {
                        if (parentObject.transform.GetChild(i).transform.GetChild(j).gameObject.name == childName)
                        {
                            childObject = parentObject.transform.GetChild(i).transform.GetChild(j).gameObject;
                            return childObject;
                        }
                        else if (parentObject.transform.GetChild(i).transform.GetChild(j).childCount != 0)
                        {
                            for (int k = 0; k < parentObject.transform.GetChild(i).transform.GetChild(j).childCount; k++)
                            {
                                if (parentObject.transform.GetChild(i).transform.GetChild(j).transform.GetChild(k).gameObject.name == childName)
                                {
                                    childObject = parentObject.transform.GetChild(i).transform.GetChild(j).transform.GetChild(k).gameObject;
                                    return childObject;
                                }
                            }
                        }
                    }
                }
            }
        }
        return childObject;
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

