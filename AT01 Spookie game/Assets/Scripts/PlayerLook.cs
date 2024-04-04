using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] float sensitivity = 2.5f;

    [SerializeField] float drag = 1.5f;

    [SerializeField] bool lookEnabled = true;



    private Transform character;
    private Vector2 mouseDir;
    private Vector2 smootthing;
    private Vector2 result;

    public bool gamepaused;

    public bool CursorToggle
    {
        set
        {
            if (value == true)
            {
                Cursor.visible = true; Cursor.lockState = CursorLockMode.None;
                gamepaused = true;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                gamepaused = false;
            }
        }
    }

    private void Awake()
    {
        character = transform.parent;
        CursorToggle = false;
    }

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

        
        if (!gamepaused)
        {
            MouseInput();

        }

        if (Input.GetButton("Pause"))
        {
            if (gamepaused == true)
            {
                EventManger.pauseGameEvent(false);
            }
            else
            {
                EventManger.pauseGameEvent(true);
            }
        }

    }
    private void OnEnable()
    {
        EventManger.pauseGameEvent += TogglePause;

    }

    private void OnDestroy()
    {
        EventManger.pauseGameEvent -= TogglePause;
    }

    private void TogglePause(bool toggle)
    {
        CursorToggle = toggle;
    }

    private void MouseInput()
    {
        if (lookEnabled == true)
        {
            mouseDir = new Vector2(Input.GetAxisRaw("Mouse X") * sensitivity, Input.GetAxisRaw("Mouse Y") * sensitivity);

            smootthing = Vector2.Lerp(smootthing, mouseDir, 1 / drag);

            result += smootthing;
            result.y = Mathf.Clamp(result.y, -90f, 90f);

            transform.localRotation = Quaternion.AngleAxis(-result.y, Vector3.right);
            character.rotation = Quaternion.AngleAxis(result.x, character.transform.up);


        }
    }

}
