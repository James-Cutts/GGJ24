using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public GameObject[] npcs;
    public float detectionRange = 10f;
    public bool inRange;

    Rigidbody playerRigidbody;

    InputManager inputManager;

    Vector3 moveDirection;
    Transform cameraObject;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isJumping;
    public bool isGrounded;
    public bool isIdle;
    public bool isTickling;

    [Header("Ground Check")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundCheckDistance = 0.4f;

    [Header("Movement Speeds")]
    [SerializeField] float walkingSpeed;
    [SerializeField] float runningSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float rotationSpeed;

    [Header("Jump")]
    [SerializeField] float jumpHeight;
    [SerializeField] float gravity = -9.81f;





    //Sound
    public string tickle = "event:/Player/Tickle";
    public string footstep = "event:/Player/Footsteps";

    FMOD.Studio.EventInstance TickleEv;
    FMOD.Studio.EventInstance FootstepEv;

    public void Awake()
    {
        // Get the required components and references
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;

        TickleEv = FMODUnity.RuntimeManager.CreateInstance(tickle);
        FootstepEv = FMODUnity.RuntimeManager.CreateInstance(footstep);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("ChaseState", 0); 
        //idle

    }

    private void Update()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        GameObject[] npcsFemale = GameObject.FindGameObjectsWithTag("NPCFemale");

        foreach (var npc in npcs)
        {
            if (Vector3.Distance(transform.position, npc.transform.position) < +detectionRange)
            {
                if (isTickling)
                {
                    Destroy(npc);
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("ChaseState", 1); //chase
                   
                    
                }

            }
        }
        foreach (var npcFemale in npcsFemale)
        {
            if (Vector3.Distance(transform.position, npcFemale.transform.position) < +detectionRange)
            {
                if (isTickling)
                {
                    Destroy(npcFemale);
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("ChaseState", 1); //chasee
                    
                    
                }
            }
        }
    }

    public void HandleAllMovement()
    {
        // Handle all movement
        HandleMovement();
        HandleRotation();
        HandleTickleInput();
    }

    private void HandleMovement()
    {

        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded)
        {
            // Calculate the movement direction based on camera orientation and player input
            moveDirection = cameraObject.forward * inputManager.verticalInput;
            moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
            moveDirection.Normalize();
            moveDirection.y = 0f;

            // Apply the appropriate movement speed based on sprinting and input amount
            if (isSprinting)
            {
                moveDirection = moveDirection * sprintSpeed;
            }
            else
            {

                if (inputManager.moveAmount >= 0.5f)
                {
                    moveDirection = moveDirection * runningSpeed;

                }
                else
                {
                    FootstepEv.start();
                    moveDirection = moveDirection * walkingSpeed;
                    //FootstepEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                }
            }

            // Check if the player is jumping
            if (isJumping)
            {
                // Calculate the jump velocity
                float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
                // Apply the jump velocity
                moveDirection.y = jumpVelocity;

            }
            // Calculate the final movement velocity and apply it to the player's rigidbody
            Vector3 movementVelocity = moveDirection + Vector3.up * playerRigidbody.velocity.y;
            playerRigidbody.velocity = movementVelocity;
        }
        else
        {
            // Apply gravity force when the player is not grounded
            Vector3 gravityForce = new Vector3(0f, gravity, 0f);
            playerRigidbody.AddForce(gravityForce, ForceMode.Acceleration);
        }
    }

    private void HandleRotation()
    {
        float rotationAngle = inputManager.cameraInputX * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotationAngle);
    }

    private void HandleTickleInput()
    {

        if (isTickling)
        {
            //FMODUnity.RuntimeManager.PlayOneShot("event:/Player/Tickle", this.transform.position);
            TickleEv.start();
            Debug.Log("Tickle");
            
        }
        else
        {
            //TickleEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
