using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TargetType
{
    PLAYER,
    TORCH
}

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float patrolSpeed = 2;
    public float chaseSpeed = 3;
    public float stunnedDuration = 2;
    public float lives;

    [Header("Sprites")]
    public Sprite lookLeft;
    public Sprite lookBottom;
    public Sprite lookTop;

    [Header("Target")]
    public float loseFocusDurationMin;
    public float loseFocusDurationMax;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public bool loopPatrol;
    public float patrolDelayMin;
    public float patrolDelayMax;

    [Header("Attack")]
    public float attackDelay = 1f;
    public float turnOffTheTorchDelay = 2f;

    [Header("Animations Speed")]
    public float animationSpeed = 3;
    public float animationToIdleSpeed = 3;
    public float idleAnimationSpeed = 0.2f;

    [Header("Animations Parameters")]
    public float maxRotation = 10;
    public float minIdleScale = 0.95f;
    public float maxIdleScale = 1.05f;

    [Space]
    public SpriteRenderer statusRenderer;

    private Transform target;
    private NavMeshAgent agent;

    private float currentLoseFocusDuration;

    private Vector3 patrolPosition;
    private int currentPatrolId = -1;
    private float currentPatrolDelay;

    private float currentAttackDelay;
    private float currentStunnedDuration;

    private float currentAnimRotation;
    private bool toggleAnimation;
    private float currentAnimScale;

    private Vector3 scaleSave;

    private bool isGoingBack;
    private bool focusTarget;
    private bool canLoseFocus;
    private bool isAttacking;
    private bool isStunned;
    private bool isOnFire;

    private TargetType targetType;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = patrolSpeed;

        scaleSave = transform.localScale;
    }

    private void Update()
    {
        Orientation();
        Animation();
        if (isStunned)
        {
            currentStunnedDuration -= Time.deltaTime;
            if (currentStunnedDuration <= 0)
            {
                isStunned = false;
            }
            return;
        }

        if (isAttacking)
        {
            currentAttackDelay -= Time.deltaTime;
            if (currentAttackDelay <= 0)
            {
                isAttacking = false;
                if (targetType == TargetType.PLAYER)
                {
                    Debug.Log("Attack!");
                }
                else
                {
                    Debug.Log("Turn off the torch!");
                }
            }
        }
        else
        {
            if (focusTarget)
            {
                if (!canLoseFocus)
                {
                    agent.SetDestination(target.position);
                }

                if (Vector3.Distance(target.position, transform.position) <= 1 && !isAttacking)
                {
                    // Start Attack
                    isAttacking = true;
                    if (targetType == TargetType.PLAYER)
                    {
                        currentAttackDelay = attackDelay;
                    }
                    else
                    {
                        currentAttackDelay = turnOffTheTorchDelay;
                    }
                }
            }
            else
            {
                // patrol
                if (currentPatrolDelay <= 0)
                {
                    currentPatrolDelay = Random.Range(patrolDelayMin, patrolDelayMax);
                    if (currentPatrolId == patrolPoints.Length - 1)
                    {
                        if (loopPatrol)
                        {
                            currentPatrolId = 0;
                        }
                        else
                        {
                            currentPatrolId--;
                            isGoingBack = true;
                        }
                    }
                    else if (currentPatrolId == 0 && isGoingBack)
                    {
                        currentPatrolId++;
                        isGoingBack = false;
                    }
                    else
                    {
                        if (!isGoingBack)
                        {
                            currentPatrolId++;
                        }
                        else
                        {
                            currentPatrolId--;
                        }
                    }
                    patrolPosition = patrolPoints[currentPatrolId].position;
                    agent.SetDestination(patrolPosition);
                }

                if (Vector3.Distance(patrolPosition, transform.position) <= 1)
                {
                    //Debug.Log("Test");
                    currentPatrolDelay -= Time.deltaTime;
                }
            }

            if (canLoseFocus)
            {
                currentLoseFocusDuration -= Time.deltaTime;
                if (currentLoseFocusDuration <= 0)
                {
                    focusTarget = false;
                    canLoseFocus = false;
                    agent.speed = patrolSpeed;
                    agent.SetDestination(patrolPosition);
                    statusRenderer.color = new Color(1, 1, 0);
                }
            }
        }
    }

    private void Animation()
    {
        
        if (agent.velocity.magnitude > 0)
        {
            if (toggleAnimation)
            {
                if (currentAnimRotation == 1)
                {
                    toggleAnimation = false;
                    currentAnimRotation = Mathf.Max(currentAnimRotation - Time.deltaTime * animationSpeed * agent.velocity.magnitude, -1);
                }
                else
                {
                    currentAnimRotation = Mathf.Min(currentAnimRotation + Time.deltaTime * animationSpeed * agent.velocity.magnitude, 1);
                }
            }
            else
            {
                if (currentAnimRotation == -1)
                {
                    toggleAnimation = true;
                    currentAnimRotation = Mathf.Min(currentAnimRotation + Time.deltaTime * animationSpeed * agent.velocity.magnitude, 1);
                }
                else
                {
                    currentAnimRotation = Mathf.Max(currentAnimRotation - Time.deltaTime * animationSpeed * agent.velocity.magnitude, -1);
                }
            }

            if (currentAnimScale > 1)
            {
                currentAnimScale = Mathf.Max(currentAnimScale - Time.deltaTime * idleAnimationSpeed, 1);
            }
            else if (currentAnimScale < 1)
            {
                currentAnimScale = Mathf.Min(currentAnimScale + Time.deltaTime * idleAnimationSpeed, 1);
            }
        }
        else
        {
            if (toggleAnimation)
            {
                if (currentAnimScale == maxIdleScale)
                {
                    toggleAnimation = false;
                    currentAnimScale = Mathf.Max(currentAnimScale - Time.deltaTime * idleAnimationSpeed, minIdleScale);
                }
                else
                {
                    currentAnimScale = Mathf.Min(currentAnimScale + Time.deltaTime * idleAnimationSpeed, maxIdleScale);
                }
            }
            else
            {
                if (currentAnimScale == minIdleScale)
                {
                    toggleAnimation = true;
                    currentAnimScale = Mathf.Min(currentAnimScale + Time.deltaTime * idleAnimationSpeed, maxIdleScale);
                }
                else
                {
                    currentAnimScale = Mathf.Max(currentAnimScale - Time.deltaTime * idleAnimationSpeed, minIdleScale);
                }
            }

            if (currentAnimRotation > 0)
            {
                currentAnimRotation = Mathf.Max(currentAnimRotation - Time.deltaTime * animationToIdleSpeed, 0);
            }
            else if (currentAnimRotation < 0)
            {
                currentAnimRotation = Mathf.Min(currentAnimRotation + Time.deltaTime * animationToIdleSpeed, 0);
            }
        }

        Quaternion newRotation = new Quaternion();
        newRotation.eulerAngles = new Vector3(transform.rotation.x, 0, currentAnimRotation * maxRotation);
        transform.rotation = newRotation;

        transform.localScale = scaleSave * currentAnimScale;
    }

    private void Orientation()
    {
        if (agent.velocity.x >= 0)
        {
            scaleSave = new Vector3(-1, 1, 1);
        }
        else
        {
            scaleSave = Vector3.one;
        }
    }

    public void Stun()
    {
        currentStunnedDuration = stunnedDuration;
        isStunned = true;
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (collision.transform.position + Vector3.up - transform.position).normalized);

        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.CompareTag("Player") && collision.CompareTag("Player") && (!focusTarget || targetType == TargetType.PLAYER))
            {
                targetType = TargetType.PLAYER;
                target = collision.transform;
                canLoseFocus = false;
                currentLoseFocusDuration = Random.Range(loseFocusDurationMin, loseFocusDurationMax);
                focusTarget = true;
                agent.speed = chaseSpeed;
                statusRenderer.color = new Color(1, 0, 0);
            }
            else if (hit.collider.CompareTag("Torch") && collision.CompareTag("Torch"))
            {
                targetType = TargetType.TORCH;
                target = collision.transform;
                canLoseFocus = false;
                currentLoseFocusDuration = Random.Range(loseFocusDurationMin, loseFocusDurationMax);
                focusTarget = true;
                agent.speed = chaseSpeed;
                statusRenderer.color = new Color(1, 0, 0);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((!focusTarget && collision.CompareTag("Player"))
            || (targetType == TargetType.PLAYER && collision.CompareTag("Torch")))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (collision.transform.position + Vector3.up - transform.position).normalized);

            if (hit.collider != null)
            {
                //Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.CompareTag("Player") && collision.CompareTag("Player") && (!focusTarget || targetType == TargetType.PLAYER))
                {
                    targetType = TargetType.PLAYER;
                    target = collision.transform;
                    canLoseFocus = false;
                    currentLoseFocusDuration = Random.Range(loseFocusDurationMin, loseFocusDurationMax);
                    focusTarget = true;
                    agent.speed = chaseSpeed;
                    statusRenderer.color = new Color(1, 0, 0);
                }
                else if (hit.collider.CompareTag("Torch") && collision.CompareTag("Torch"))
                {
                    targetType = TargetType.TORCH;
                    target = collision.transform;
                    canLoseFocus = false;
                    currentLoseFocusDuration = Random.Range(loseFocusDurationMin, loseFocusDurationMax);
                    focusTarget = true;
                    agent.speed = chaseSpeed;
                    statusRenderer.color = new Color(1, 0, 0);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player") && targetType == TargetType.PLAYER)
            || (collision.CompareTag("Torch") && targetType == TargetType.TORCH))
        {
            canLoseFocus = true;
            statusRenderer.color = new Color(1, 0.5f, 0);
        }
    }
}
