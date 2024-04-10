using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stunstick : MonoBehaviour
{
    // set stun time 
    public flaot stunDuration = 3.5f
    private EnemyNPCMovement enemyNPCMovement;


    // Start is called before the first frame update
    void Start()
    {
        // Find enemy movement script
        enemyNPCMovement = GetComponent<enemyNPCMovement>();

    }
    // Activate StunState 
    public void ActivatStun()
    {
        if (enemyNPCMovement != null) 
        {
            // calls the StunState form enemy script
            enemyNPCMovement.Stun(stunDuration);
        }







    }
   
}
