using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKey : MonoBehaviour, IInteraction
{
    public enum KeyCode { RedKey, BlueKey, KeySix }
    public KeyCode keyCode = KeyCode.RedKey;


    public void Activate()
    {
        // call the 'picjed up keyt event' and pass it out keycode value
        EventManger.unlockDoorEvent((int)keyCode);



        Debug.Log(" picked up key");

        gameObject.SetActive(false);
    }
}
