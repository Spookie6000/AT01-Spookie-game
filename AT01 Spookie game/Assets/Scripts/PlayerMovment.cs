using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UIElements;
/// <summary>
/// Chat GPT Has been used to modify this script
/// </summary>

public class PlayerMovment : MonoBehaviour
{
    private CharacterController cTroller;
    public Slider instance;

    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float jumpHeight = 1.0f;

 
    private float gravityValue = -9.81f;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    bool isGrounded;

    bool gamePaused;

    public static PlayerMovment Instance;
    private void OnEnable()
    {
        EventManger.pauseGameEvent += TogglePaused;

    }


    // Start is called before the first frame update
    void Start()
    {

        if (Instance == null)
        {
            Instance = this;
        }





        gamePaused = false;
        //cTroller = GetComponent<CharacterController>();


        if (!TryGetComponent<CharacterController>(out cTroller))
        {
            Debug.Log("you need to attach a CharaterController to the player object you human");

            gameObject.SetActive(false);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gamePaused)
        {
            PlayerInput();
        }

    }
    private void TogglePaused(bool toggled)
    {
        Debug.Log("PLayerMove paused or Unpause");
        gamePaused = toggled;
    }



    private void PlayerInput()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        cTroller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        velocity.y += gravityValue * Time.deltaTime;
        cTroller.Move(velocity * Time.deltaTime);


    }
    private void OnDestroy()
    {
        EventManger.pauseGameEvent -= TogglePaused;
    }

}