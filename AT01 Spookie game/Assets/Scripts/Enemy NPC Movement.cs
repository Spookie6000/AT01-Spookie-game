using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;
using UnityEngine.AI;

public class EnemyNPCMovement : MonoBehaviour
{

    public StateMachine StateMachine { get; private set; }
    [SerializeField] PlayerMovment player;

    [SerializeField] private Animator controller;

    [SerializeField] private float viewDistance;


    private void Awake()
    {
        StateMachine = new FiniteStateMachine.StateMachine();


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

            instance.controller.SetTrigger("idel");

        }
        public override void OnUpdate()
        {
            Debug.Log("Still in  idle");

            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {

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
        public override void OnEnter()
        {
            Debug.Log("Entering patrol");



        }
        public override void OnUpdate()
        {
            Debug.Log("Still in  patrol");
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
                                    instance.controller.SetTrigger("walk");
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

        // class for chasing state  


    }
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

}





