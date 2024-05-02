using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunItem : MonoBehaviour
{
    public AINPC aiNPC;

    public float stunD = 3.5f;
   
    public void ActivateStun()
    {
        if (aiNPC == null)
        {
            aiNPC.Stun(stunD);
        }
        else
        {
            Debug.Log("AINPC script is not set in StunTriggerItem script");
        }

        gameObject.SetActive(false);


    }
    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player"))
        {
            ActivateStun();
        }
    }
}
