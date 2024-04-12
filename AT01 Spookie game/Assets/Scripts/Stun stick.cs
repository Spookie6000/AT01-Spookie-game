using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stunstick : MonoBehaviour
{
    // set stun time 
    public float stunDuration = 3.5f;
    private EnemyNPCMovement enemyNPCMovement;


    // Start is called before the first frame update
    private void Start()
    {
        // Find enemy movement script
        enemyNPCMovement = GetComponent<EnemyNPCMovement>();
        if (enemyNPCMovement == null) 
        {
            Debug.LogError("EnemyNPCMovemrnt not found");
        }

    }
    // Activate StunState
    public void ActivateStun()
    {
        if (enemyNPCMovement != null) 
        {
            // calls the StunState form enemy script
            enemyNPCMovement.Stun(stunDuration);
            Debug.Log("Stunned");
        }
        else
        {

            Debug.LogError("Stunned failled");


        }







    }
   
}
