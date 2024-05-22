using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StunItem : MonoBehaviour, IInteraction
{
    public bool stunItem;

    public void Activate()
    {

        gameObject.SetActive(false);

        Debug.Log("Where's the Ball(Pick up StunBat)");
        
        stunItem = true;

    }

}
