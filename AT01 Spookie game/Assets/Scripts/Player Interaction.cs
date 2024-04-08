using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    private bool gamePaused;



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

    }
    private void Update()
    {
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
