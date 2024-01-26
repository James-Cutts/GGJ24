using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerMovement playerMovement;
   // AnimManager animManager;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float moveAmount;

    [Header("Camera Inputs")]
    public float cameraInputX;
    public float cameraInputY;

    [Header("Movement Inputs")]
    public float verticalInput;
    public float horizontalInput;

    [Header("Other Inputs")]
    public bool sprintInput;
    public bool jumpInput;
    public bool interactInput;


    private void Awake()
    {
       // animManager = GetComponent<AnimManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.Gameplay.Move.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Gameplay.Move.canceled += i => movementInput = Vector2.zero;
            Cursor.lockState = CursorLockMode.Locked;

            playerControls.Gameplay.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.Gameplay.Jump.performed += i => jumpInput = true;
            playerControls.Gameplay.Jump.canceled += i => jumpInput = false;

            playerControls.Gameplay.Sprint.performed += i => sprintInput = true;
            playerControls.Gameplay.Sprint.canceled += i => sprintInput = false;

            playerControls.Gameplay.Interact.performed += i => interactInput = true;
            playerControls.Gameplay.Interact.canceled += i => interactInput = false;

        }

        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpInput();
    }
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        //animManager.UpdateAnimatorValues(0, moveAmount, playerMovement.isSprinting);

        if (moveAmount <= 0)
        {
            playerMovement.isIdle = true;
        }
        else
        {
            playerMovement.isIdle = false;
        }
    }
    private void HandleSprintingInput()
    {
        if(sprintInput && moveAmount > 0.5f)
        {
            playerMovement.isSprinting = true;
        }
        else
        {
            playerMovement.isSprinting = false;
        }
    }

    private void HandleJumpInput()
    {
        if(jumpInput && playerMovement.isGrounded && !sprintInput)
        {
            playerMovement.isJumping = true;
        }
        else if (playerMovement.isGrounded)
        {
            playerMovement.isJumping = false;
        }
    }
}
