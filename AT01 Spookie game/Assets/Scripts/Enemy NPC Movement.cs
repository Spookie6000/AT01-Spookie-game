using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;
using UnityEngine.AI;
using System.Linq;

//public enum PatrolTime(patrol);

public class EnemyNPCMovement : MonoBehaviour
{
    // StateMachine for handling the AI enemy
    public StateMachine StateMachine { get; private set; }
    // Calling the PLayer script 
    [SerializeField] PlayerMovment player;
    // Animaor controller 
    [SerializeField] private Animator controller;
    // AI viewing Distance 
    [SerializeField] private float viewDistance;
    // Stun timer 
    public float StunTime;
    // List of patrolPoints 
   // bool PatrolPoint;
   // list of PatrolPoints 
    List<GameObject> patrolPoints = new List<GameObject>();
    // List of privacte patrolpoints
    //public int currentPatrolIndex;
    // NaveMeshAgent 
    private NavMeshAgent navMeshAgent;
   
   

    private void Awake()
    {
        //Calls StateMachine 
        StateMachine = new FiniteStateMachine.StateMachine();
        // Calls the NavMeshAagent 
        navMeshAgent = GetComponent < NavMeshAgent> ();

    }


    // Start is called before the first frame update
    void Start()
    {
        //set the state machine to the idel state to start
        StateMachine.SetState(new IdelState(this));
    }

    // Update is called once per frame
    void Update()
    {
        // call "onupdate' for whatever the currentstate is 
        StateMachine.OnUpdate();
    }
    //Abstract cllas called "NPCState" which inherits form IState
    public abstract class NPCState : IState
    {
        protected EnemyNPCMovement instance;

        public NPCState(EnemyNPCMovement _intance)
        {
            instance = _intance;
        }

        public virtual void OnUpdate() { }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

    }
    // clas for the idle state
    public class IdelState : NPCState
    {
        public IdelState(EnemyNPCMovement _intance) : base(_intance)
        {

        }

        float timer = 3f;

        public override void OnEnter()
        {
            Debug.Log("Entering idle");
            // Trigger Idel Animation
            instance.controller.SetTrigger("idel");

        }
        public override void OnUpdate()
        {
            Debug.Log("Still in  idle");
            // timer for state change 
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                // Changes to partol state
                instance.controller.SetTrigger("walk");
                instance.StateMachine.SetState(new PatrolState(instance));



            }

            Collider[] hitCollider = Physics.OverlapSphere(instance.transform.position, instance.viewDistance);
            foreach (Collider collider in hitCollider)
            {
                if (collider.gameObject.TryGetComponent<PlayerMovment>(out PlayerMovment pMove))
                {//checkinng if there anything be dectied 
                    RaycastHit hit;
                    Vector3 derection = instance.player.transform.position - instance.transform.position;
                    if (Physics.Raycast(instance.transform.position, derection, out hit, 15f))
                    {
                        if (hit.collider.gameObject.TryGetComponent<PlayerMovment>(out PlayerMovment pMove2))
                        {
                            float angle = Vector3.Angle(derection, instance.transform.forward);
                            if (angle < 45f)
                            {
                                if (angle > -45f)
                                {
                                    instance.controller.SetTrigger("walk");
                                    instance.StateMachine.SetState(new ChaseState(instance));

                                }
                            }
                        }
                    }
                }
            }



        }

        public override void OnExit()
        {
            Debug.Log("Leaving idle");
        }

    }


    // class for the patrol state 

    public class PatrolState : NPCState
    {
        public PatrolState(EnemyNPCMovement _intance) : base(_intance)
        { }
        //  int of currentpatrolindex
        int currentPatrolIndex;
        public override void OnEnter()
        {
       
          

            Debug.Log("Entering patrol");
            // Set the NavMeshAgent first partol point
           instance. navMeshAgent.SetDestination(instance.patrolPoints[0].transform.position);
            //set 'currentPatrolIndex' to 0
            currentPatrolIndex = 0;



          
            






           /* if (instance.patrolPoints.) 
            {
                // Move to the next Patrol Point
                //instance.currentPatrolIndex = (instance.currentPatrolIndex + 1) % instance.patrolPoints.Count;
                instance.navMeshAgent.SetDestination(instance.patrolPoints[instance.currentPatrolIndex].position);


            } */     
   
        }
        public override void OnUpdate()
        {

            //check if the Agent is within stopping distance of current destination
            if (instance.navMeshAgent.remainingDistance < instance.navMeshAgent.stoppingDistance) 
            {
                //if yes: increase currentPatrolIndex by 1
                 currentPatrolIndex += 1;
                if currentPatrolIndex 
                {
                    currentPatrolIndex > instance.patrolPoints



                }
                //check if currentPatrolIndex+1 is over the limit

                //if yes: go to idle
                instance.StateMachine.SetState(new IdelState(instance));

                
               else

                    //if no:
                    //set navmeshagent destination to 'listOfWaypoint[currentPatrolIndex]'
                    instance.navMeshAgent.destination = instance.patrolPoints[currentPatrolIndex].transform.position;



                
              



            }



            Debug.Log("Still in  patrol");
            // Has the Enemy reach the patrol point
           /* if(!instance.navMeshAgent.pathPending && instance.navMeshAgent.remainingDistance > 0);*/

            // looking for player movement

            Collider[] hitCollider = Physics.OverlapSphere(instance.transform.position, instance.viewDistance);
            foreach (Collider collider in hitCollider)
            {
                if (collider.gameObject.TryGetComponent<PlayerMovment>(out PlayerMovment pMove))
                {
                    RaycastHit hit;
                    Vector3 derection = instance.player.transform.position - instance.transform.position;
                    if (Physics.Raycast(instance.transform.position, derection, out hit, instance.viewDistance))
                    {
                        if (hit.collider.gameObject.TryGetComponent<PlayerMovment>(out PlayerMovment pMove2))
                        {

                            float angle = Vector3.Angle(derection, instance.transform.forward);
                            if (angle < 45f)
                            {
                                if (angle > -45f)
                                {
                                    instance.controller.SetTrigger("Pa");
                                    instance.StateMachine.SetState(new ChaseState(instance));

                                }
                            }



                        }
                        else
                        {
                            Debug.Log("wall in the way");
                        }
                    }



                }
            }
        }

        public override void OnExit()
        {
            Debug.Log("Leaving patrol");
        }

         


    }
       // class for chasing state 
    public class ChaseState : NPCState
    {
        public ChaseState(EnemyNPCMovement _intance) : base(_intance)
        { }

        public override void OnEnter()
        {
            Debug.Log("Entering CHASE");



        }
        public override void OnUpdate()
        {
            Debug.Log("Still in  CHASE");

            // The NPC loss the player and goes back to IdelState
            if (Vector3.Distance(instance.transform.position, instance.player.transform.position) > instance.viewDistance * 1.5f)
            {
                instance.controller.SetTrigger("run");
                instance.StateMachine.SetState(new IdelState(instance));
            }


        }
        public override void OnExit()
        {
            Debug.Log("Leaving CHASE");
        }
    }
    public class StunState : NPCState
    {

        float stunTimer;
        public StunState(EnemyNPCMovement _instance, float stunTime) : base(_instance) 
        {
             stunTimer = stunTime;
        }
        
        public override void OnEnter() 
        {
            // Tigger Stun animation 
            instance.controller.SetTrigger("stun");
        
        }
        public override void OnUpdate()
        {
            // Setting the stun timer 
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                //Changes to Idel when stun is over 
                instance.StateMachine.SetState(new IdelState(instance));
            }
        }


    }
    // Setting stun methed call
    public void Stun(float stunDuraction)
    {
        StateMachine.SetState(new StunState(this, stunDuraction));
    }

}





