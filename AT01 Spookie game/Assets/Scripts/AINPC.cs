using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// This Script was create by following the Immergo Media:  
/// Unity - Basic Ai States 
/// </summary>

// The BehaviourState
public enum BState { None, Idle, Patrol, Chase, Stun, ObjectiveChase }

public class AINPC : MonoBehaviour
{
    // Calling the Player script 
    [SerializeField] PlayerMovment player;
    // Animator controller 
    [SerializeField] private Animator controller;
    // AI viewing Distance 
    [SerializeField] private float baseViewDistance = 10f; // Base view distance
    [SerializeField] private float sprintViewConeMultiplier = 1.5f; // Multiplier for sprinting
    [SerializeField] private float crouchViewConeMultiplier = 0.5f; // Multiplier for crouch walking
    private float currentViewDistance; // Current view distance
    // Initial state the enemy starts in
    public BState initialState;
    // Idle Timer 
    float idleTimer = 3f;
    // The Patrol Points
    public Transform[] patrolPoints;
    // Random Sequence 
    public bool randomSequence = false;
    // Calling the NavMesh
    private NavMeshAgent agent;
    // Current state
    private BState currentState;
    // Timer for stun duration
    private float stunTimer;
    // Layer mask for line of sight check
    public LayerMask sightBlockingLayers;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = initialState;
        currentViewDistance = baseViewDistance;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PerformState());
    }

    // Update is called once per frame
    void Update()
    {
        // Update view distance based on player's movement
        UpdateViewDistance();

        // Check if player is in view distance and within line of sight
        if (currentState != BState.Stun && currentState != BState.ObjectiveChase &&
            CanSeePlayer() && Vector3.Distance(transform.position, player.transform.position) <= currentViewDistance)
        {
            // Transition to chase state
            TransitionToState(BState.Chase);
        }
    }

    // Update view distance based on player's movement
    void UpdateViewDistance()
    {
        if (player.IsSprinting)
        {
            currentViewDistance = baseViewDistance * sprintViewConeMultiplier;
        }
        else if (player.IsCrouching)
        {
            currentViewDistance = baseViewDistance * crouchViewConeMultiplier;
        }
        else
        {
            currentViewDistance = baseViewDistance;
        }
    }

    // Check if the NPC can see the player
    bool CanSeePlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, currentViewDistance, sightBlockingLayers))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("I SEEE YOOOU");
                return true;
               
            }
        }
        return false;
    }

    IEnumerator PerformState()
    {
        while (true)
        {
            switch (currentState)
            {
                case BState.Idle:
                    // Trigger idle animation
                    controller.SetTrigger("IdleTrigger");
                    // Perform actions for idle state
                    yield return new WaitForSeconds(idleTimer);
                    // Transition to next state (e.g., patrol)
                    TransitionToState(BState.Patrol);
                    break;

                case BState.Patrol:
                    // Trigger patrol animation
                    controller.SetTrigger("PatrolTrigger");
                    // Perform actions for patrol state





                    if (Vector3.Distance(transform.position,agent.destination) < agent.stoppingDistance)
                    {



                        if (randomSequence)
                        {
                            // Choose a random patrol point
                            int randomIndex = Random.Range(0, patrolPoints.Length);
                            agent.SetDestination(patrolPoints[randomIndex].position);

                        }
                        else
                        {
                            // Sequentially visit each patrol point
                            for (int i = 0; i < patrolPoints.Length; i++)
                            {
                                agent.SetDestination(patrolPoints[i].position);
                                // Stop at patrol point
                                while(Vector3.Distance(transform.position, patrolPoints[i].position) > agent.stoppingDistance)
                                {
                                    yield return null;
                                }
                                yield return new WaitForSeconds(2f); // Adjust delay between each point
                            }
                        }
                    }   
                    yield return null;
                    break;

                case BState.Chase:
                    // Trigger chase animation
                    controller.SetTrigger("ChaseTrigger");
                    // Perform actions for chase state
                    agent.SetDestination(player.transform.position);
                    yield return null;
                    break;

                case BState.Stun:
                    // Trigger stun animation
                    controller.SetTrigger("StunTrigger");
                    // Perform actions for stun state
                    yield return new WaitForSeconds(stunTimer);
                    // Transition back to the previous state or a default state
                    TransitionToState(BState.Idle); // Example transition to idle state
                    break;

                case BState.ObjectiveChase:
                    // Trigger objective chase animation
                    controller.SetTrigger("ObjectiveChaseTrigger");
                    // Perform actions for objective chase state
                    agent.SetDestination(player.transform.position);
                    yield return null;
                    break;

                default:
                    yield return null;
                    break;
            }
            yield return null;
        }
    }

    // Transition to a new state
    void TransitionToState(BState newState)
    {
        currentState = newState;
        StopAllCoroutines(); // Stop current state coroutine
        StartCoroutine(PerformState()); // Start coroutine for the new state
    }

    // Call this method to stun the NPC
    public void Stun(float duration)
    {
        if (currentState != BState.Stun)
        {
            stunTimer = duration;
            TransitionToState(BState.Stun);
        }
    }

    // Call this method when the player picks up the objective
    public void PlayerPickedUpObjective()
    {
        // Transition to objective chase state
        TransitionToState(BState.ObjectiveChase);
    }
}