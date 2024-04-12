using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManger : MonoBehaviour
{
    public static EventManger Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Instance = this;

        }
        else
        {
            Debug.Log("You can only have one event manager in the scene"); 
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    public delegate void UnlockDoor(int id);
    public static UnlockDoor unlockDoorEvent;

    public delegate void PauseGame(bool toggle);
    public static PauseGame pauseGameEvent;


}
