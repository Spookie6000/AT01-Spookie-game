using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
/// <summary>
/// Chat GPT Has been used to modify this script
/// </summary>

public class PlayerMovment : MonoBehaviour
{
    private CharacterController cTroller;
    // Sets the speed for the movement
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float spritSpeed = 10f;
    [SerializeField] private float crouchSpeed = 2.5f;
    // Srinting cooldown 
    [SerializeField] private float sprintCooldown = 3f;
    // Jump Hieght 
    [SerializeField] private float jumpHeight = 1.0f;
    // Stamina Variables 
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float sprinStaminaDrainRate = 20f;
    [SerializeField] private float sprintStaminaRecoveryRate = 10f;
    private float currentStamina;


    private bool isSpriting = false;
    private bool isCrouchingWalking = false;
    private float gravityValue = -9.81f;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    // Sets the properties to get the sprinting and crouch 
    public bool IsSprinting => isSprinting;
    public bool IsCrouching => isCrouchingWalking;


    private Vector3 velocity;

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

        /*if (Instance == null)
        {
            Instance = this;
        }*/

        gamePaused = false;
        cTroller = GetComponent<CharacterController>();
        currentStamina = maxStamina;


        /* if (!TryGetComponent<CharacterController>(out cTroller))
         {
             Debug.Log("you need to attach a CharaterController to the player object you human");

             gameObject.SetActive(false);

         }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (!gamePaused)
        {
            PlayerInput();
        }
        // Stamina Over Tinme
        if (!isSpriting && currentStamina < maxStamina)
        {
            currentStamina += sprintStaminaRecoveryRate * Time.deletaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        }

    }
    private void TogglePaused(bool toggled)
    {
        Debug.Log("PLayerMove paused or Unpause");
        gamePaused = toggled;
    }



    private void PlayerInput()
    {   // Set the isGrounded bool 
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

        }
        // Set the Movement for Basic movement 
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        float speed = IsSprinting ? spritSpeed : (isCrouchingWalking ? crouchSpeed : moveSpeed);

        cTroller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }
        // add garvity 
        velocity.y += gravityValue * Time.deltaTime;
        cTroller.Move(velocity * Time.deltaTime);
        // Starts the spriting and checks the stamina 
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isSprinting && currentStamina > 0)
        {
            isSprinting = true;
            Debug.Log("Out of Stamina");

            //Stop sprintting
            if (Input.GetKeyUp(KeyCode.LeftShift) || currentStamina <= 0)
            {
                isSprinting = false;
            }
            // Takes stamina while sprinting 
            if (isSprinting)
            {
                currentStamina -= sprintStaminaConsumptionRate * Time.deltaTime;
                // sets current stamina 
                currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            }



            // Crouch walking input
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                isCrouchWalking = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                isCrouchWalking = false;
            }


        }

    }
    private void OnDestroy()
    {
        EventManger.pauseGameEvent -= TogglePaused;
    }
}