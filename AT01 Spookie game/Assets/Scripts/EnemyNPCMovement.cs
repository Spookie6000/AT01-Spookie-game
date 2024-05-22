using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;
using UnityEngine.AI;

public class EnemyNPCMovement : MonoBehaviour
{
    public StateMachine StateMachine {  get; private set; }

    [SerializeField] PlayerMovment player;

    [SerializeField] PlayerInteraction playerInteraction;

    [SerializeField] NavMeshAgent agent;

    [SerializeField] List<GameObject> wayPoints = new List<GameObject>();

    [SerializeField] int wanderCount;

    [SerializeField] private Animator controller;

    [SerializeField] private float viewDistance;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip chaseClip;

    private void Awake()
    {
        StateMachine = new StateMachine();
    }
    // Start is called before the first frame update
    void Start()
    {
        //set the state machine to the idle state to start
        StateMachine.SetState(new IdleState(this));
    }

    // Update is called once per frame
    void Update()
    {
        //call 'onupdate' for whatever the currentstate is
        StateMachine.OnUpdate();
    }

    //abstact class called 'NPCState' which inhericts fron IState
    public abstract class NPCState : IState
    {
        protected EnemyNPCMovement instance;

        public NPCState(EnemyNPCMovement _instance)
        {
            instance = _instance;
        }

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }

    }

    //class for the idle state
    public class IdleState : NPCState
    {
        public IdleState(EnemyNPCMovement _instance) : base(_instance)
        {

        }
        float timer = 3f;

        public override void OnEnter()
        {
            Debug.Log("Entering idle");
        }
        public override void OnUpdate()
        {
            Debug.Log("Still in idle");

            if(timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                instance.controller.SetTrigger("walk");
                instance.StateMachine.SetState(new PatrolState(instance));
            }

            //create an overlapshere to see if the player is in spotting distance
            Collider[] hitColliders = Physics.OverlapSphere(instance.transform.position, instance.viewDistance);
            foreach(Collider collider in hitColliders)
            {
                if(collider.gameObject.TryGetComponent<PlayerMovment>(out PlayerMovment pMove))
                {
                    //check if there isn't anything between AI and player
                    RaycastHit hit;
                    Vector3 direction = instance.player.transform.position - instance.transform.position;
                    if (Physics.Raycast(instance.transform.position, direction, out hit, instance.viewDistance = 5f))
                    {
                        if(hit.collider.gameObject.TryGetComponent<PlayerMovment>(out PlayerMovment pMove2))
                        {
                            float angle = Vector3.Angle(direction, instance.transform.forward);
                            if (angle < 45f)
                            {
                                if (angle > -45f)
                                {
                                    instance.controller.SetTrigger("run");
                                    instance.StateMachine.SetState(new ChaseState(instance));
                                }
                            }
                            else
                            {
                                Debug.Log("Not in view");
                            }
                        }
                        else
                        {
                            Debug.Log("Wall in the way");
                        }
                    }
                }
            }
        }
        
        public override void OnExit()
        {
            Debug.Log("Leavng idle");
        }
    }

    //class for patrol state
    public class PatrolState : NPCState
    {
        public PatrolState(EnemyNPCMovement _instance) : base(_instance)
        {

        }

        public override void OnEnter()
        {
            Debug.Log("Entering Patrol");
            instance.wanderCount = Random.Range(1, 5);

            int ran = Random.Range(0, instance.wayPoints.Count - 1);
            GameObject randowWaypoint = instance.wayPoints[ran];

            instance.agent.SetDestination(randowWaypoint.transform.position);
        }


        public override void OnUpdate()
        {
            if(Vector3.Distance(instance.transform.position, instance.agent.destination) < instance.agent.stoppingDistance)
            {
                if(instance.wanderCount > 0)
                {
                    instance.wanderCount--;

                    int ran = Random.Range(0, instance.wayPoints.Count - 1);
                    GameObject randomWaypoint = instance.wayPoints[ran];

                    instance.agent.SetDestination(randomWaypoint.transform.position);
                }
                else
                {
                    instance.controller.SetTrigger("idle");
                    instance.StateMachine.SetState(new IdleState(instance));
                }
            }
            //Setting view cone
            Collider[] hitColliders = Physics.OverlapSphere(instance.transform.position, instance.viewDistance);
            foreach (Collider collider in hitColliders)
            {
                if (collider.gameObject.TryGetComponent<PlayerMovment>(out PlayerMovment pMove))
                {
                    RaycastHit hit;
                    Vector3 direction = instance.player.transform.position - instance.transform.position;
                    if (Physics.Raycast(instance.transform.position, direction, out hit, instance.viewDistance + 5f))
                    {
                        if (hit.collider.gameObject.TryGetComponent<PlayerMovment>(out PlayerMovment pMove2))
                        {
                            float angle = Vector3.Angle(direction, instance.transform.forward);
                            if(angle < 45f)
                            {
                                if(angle > -45f)
                                {
                                    instance.controller.SetTrigger("run");
                                    instance.StateMachine.SetState(new ChaseState(instance));
                                }
                            }
                            else
                            {
                                Debug.Log("Not in view");
                            }

                        }
                        else
                        {
                            Debug.Log("Wall in the way");
                        }
                    }
                }
            }
        }

        public override void OnExit()
        {
            Debug.Log("Leavng patrol");
        }
    }

    //class for chasing state
    public class ChaseState : NPCState
    {
        public ChaseState(EnemyNPCMovement _instance) : base(_instance)
        {

        }
        public override void OnEnter()
        {
            Debug.Log("Entering chase state");
            //Set Ai to follow player di
            instance.agent.SetDestination(instance.player.transform.position);

            instance.audioSource.PlayOneShot(instance.chaseClip);
;        }
        public override void OnUpdate()
        {
            Debug.Log("Still chasing");
            // If the player is in view go back to Idle
            instance.agent.SetDestination(instance.player.transform.position);

            if (Vector3.Distance(instance.transform.position, instance.player.transform.position) > instance.viewDistance * 1.5f)
            {
                instance.controller.SetTrigger("idle");
                instance.StateMachine.SetState(new IdleState(instance));
            }
        }

        public override void OnExit()
        {
            Debug.Log("Stopped chasing");
        }
    }

    public class StunState : NPCState
    {
        public StunState(EnemyNPCMovement _instance) : base(_instance)
        {

        }

        float stunTimer = 3.5f;
        


        public override void OnEnter()
        {
            Debug.Log("Entering StunState");
            // Stun Timer 
           stunTimer = Time.time;

            




        }
        public override void OnUpdate()
        {
            if (stunTimer <= 0) 
            {
                instance.controller.SetTrigger("idle");
                instance.StateMachine.SetState(new IdleState(instance));


            }
            else 
            {
                //stun Enemy

                instance.controller.SetTrigger("stun");
                stunTimer -= Time.deltaTime;
            
            
            }
           



            
        }

        
        public override void OnExit()
        {
            Debug.Log("No longer Stunned ");
        }
    }

    public void ActivateStunState()
    {
        StateMachine.SetState(new StunState(this));




    }


}










