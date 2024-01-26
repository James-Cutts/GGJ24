using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody playerRigidbody;

    InputManager inputManager;

    Vector3 moveDirection;
    Transform cameraObject;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isJumping;
    public bool isGrounded;
    public bool isIdle;

    [Header("Ground Check")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundCheckDistance = 0.4f;

    [Header("Movement Speeds")]
    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningSpeed = 5;
    [SerializeField] float sprintSpeed = 7;
    [SerializeField] float rotationSpeed = 15f;

    [Header("Jump")]
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float gravity = -9.81f;


    public void Awake()
    {
        // Get the required components and references
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;

    }

    public void HandleAllMovement()
    {
        // Handle all movement
        HandleMovement();
        HandleRotation();
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
                    moveDirection = moveDirection * walkingSpeed;
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
         // Calculate the target direction based on camera orientation and player input
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0f;

        if(targetDirection == Vector3.zero)
        {
            // If there is no input, maintain the current forward direction
            targetDirection = transform.forward;
        }
        // Calculate the target rotation based on the target direction
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        // Smoothly interpolate between the current rotation and the target rotation
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Apply the final rotation to the player's transform
        transform.rotation = playerRotation;
    }
}
