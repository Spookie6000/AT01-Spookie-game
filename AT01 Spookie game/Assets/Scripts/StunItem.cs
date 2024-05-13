using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StunItem : MonoBehaviour, IInteraction
{   

    public void Activate()
    {
     


        Debug.Log("Where's the Ball(Pick up StunBat)");

        gameObject.SetActive(false);
    }

}
