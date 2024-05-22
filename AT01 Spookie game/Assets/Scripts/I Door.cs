using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class IDoor : MonoBehaviour, IInteraction
{
    public bool locked;

    public enum DoorID { RedDoor, BlueDoor, Doorsix }
    public DoorID doorID = DoorID.RedDoor;

    bool doorOpen = false;

    [SerializeField] Animator animator;
    private void Start()
    {
        EventManger.unlockDoorEvent += UnlockDoor;
    }

    private void OnDestroy()
    {
        EventManger.unlockDoorEvent -= UnlockDoor;
    }

    private void UnlockDoor(int id)
    {
        if (id == (int)doorID)
        {
            Debug.Log("DOOR IS LOCKED");
            locked = false;
        }
    }
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {

    }


    public void Activate()
    {
        if (locked)
        {
            Debug.Log(" you need the " + doorID + "Key");

            animator.Play("Doorclosed");

        }
        else
        {
            if (!doorOpen)
            {
                animator.Play("DoorOpen");
                doorOpen = true;
                SceneManager.LoadScene(2);
                Debug.Log("The door");
            }
            else
            {
                animator.Play("DoorClose");
                doorOpen = false;
                Debug.Log("The door opens");
            }


        }
     

    }


}
