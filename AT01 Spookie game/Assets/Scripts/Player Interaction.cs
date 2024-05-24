using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;


/*
 * receive input for the player's mouse (left click)
 * check of there's anything on front of the camera within a certain distance that can be intrtacted with
 * if there is something in front of the camera ......
 * then call it's Activate method
 * 
 * 
 */
public class PlayerInteraction : MonoBehaviour
{
    //variables 

    // the maximim interaction distance 
    [SerializeField] private float iDistance;
    [SerializeField] GameObject MyCamera;
    [SerializeField] bool stunItem;
    [SerializeField] GameObject Enemy;


    private EnemyNPCMovement enemyNPC;
    private bool gamePaused;
    float stunTimer;



    private void OnEnable()
    {
        EventManger.pauseGameEvent += TogglePause;
    }
    private void OnDestroy()
    {
        EventManger.pauseGameEvent -= TogglePause;
    }


    private void Start()
    {
        enemyNPC = Enemy.GetComponent<EnemyNPCMovement>();   


    }
    private void Update()
    {
      if (stunItem)    
       {
            if (Input.GetButtonDown("StunButton"))
            {
                RaycastHit hit;

                if (Physics.Raycast(MyCamera.transform.position, MyCamera.transform.forward, out hit, iDistance))
                {
                    if (hit.collider.gameObject.CompareTag("Enemy"))
                    {
                        // Activate Stun State
                        enemyNPC.ActivateStunState();
                    }
                }

            }




        }

        if (!gamePaused)
        {
            // check for player left clicking 
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                if (Physics.Raycast(MyCamera.transform.position, MyCamera.transform.forward, out hit, iDistance))
                {
                    IInteraction interaction;
                    if (hit.collider.gameObject.TryGetComponent<IInteraction>(out interaction))
                    {
                        interaction.Activate();
                    }
                }
            }

        }
        // check for intercatable in front of camera 
        // if yes -  call Activate
        // if no - do notthing 
    }

    private void TogglePause(bool toggle)
    {

        gamePaused = toggle;
    }

   
}
