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
    public float lives;

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

    [Space]
    private Transform target;
    public SpriteRenderer statusRenderer;
    private NavMeshAgent agent;

    private float currentLoseFocusDuration;

    private Vector3 patrolPosition;
    private int currentPatrolId = -1;
    private float currentPatrolDelay;

    private float currentAttackDelay;

    private bool isGoingBack;
    private bool focusTarget;
    private bool canLoseFocus;
    private bool isAttacking;

    private TargetType targetType;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = patrolSpeed;
    }

    private void Update()
    {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && (!focusTarget || targetType == TargetType.PLAYER))
        {
            targetType = TargetType.PLAYER;
            target = collision.transform;
            canLoseFocus = false;
            currentLoseFocusDuration = Random.Range(loseFocusDurationMin, loseFocusDurationMax);
            focusTarget = true;
            agent.speed = chaseSpeed;
            statusRenderer.color = new Color(1, 0, 0);
        }
        else if (collision.CompareTag("Torch"))
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
