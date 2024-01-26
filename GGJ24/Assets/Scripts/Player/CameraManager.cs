using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;

    public Transform targetTransform; // The target the camera follows
    public Transform cameraPivot; // The pivot point for camera rotation
    public Transform cameraTransform; // The actual camera transform

    private Vector3 cameraFollowVelocity = Vector3.zero; 
    private Vector3 cameraVectorPosition;

    private float defaultPosition;

    [Header("Camera Collision")]
    [SerializeField] float cameraCollisionOffSet = 0.2f; // How far the camera moves away from objects it hits
    [SerializeField] float minCollisionOffSet = 0.2f; // Minimum offset for camera collision
    [SerializeField] float cameraCollisionRadius = 0.2f; // Radius of the collision sphere
    public LayerMask collisionLayers; // Layers to consider for camera collision

    [Header("Camera Speeds")]
    [SerializeField] float cameraFollowSpeed = 0.2f; // Speed of camera follow
    [SerializeField] float cameraLookSpeed = 2; // Speed of camera horizontal rotation
    [SerializeField] float cameraPivotSpeed = 2; // Speed of camera vertical rotation

    [Header("Camera Angles")]
    [SerializeField] float lookAngle; // Up and down looking angle
    [SerializeField] float pivotAngle; // Left and right looking angle
    [SerializeField] float minPivot = -45; // Minimum vertical pivot angle
    [SerializeField] float maxPivot = 45; // Maximum vertical pivot angle

    private void Awake()
    {
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        inputManager = FindObjectOfType<InputManager>();
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }
    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }
    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition; // Move the camera to follow the target with smooth dampening
    }
    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle = lookAngle + (inputManager.cameraInputX * cameraLookSpeed); // Adjust the horizontal rotation based on player input
        pivotAngle = pivotAngle - (inputManager.cameraInputY * cameraPivotSpeed); // Adjust the vertical rotation based on player input
        pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot); // Clamp the pivot angle to the specified range

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation; // Set the rotation of the camera based on the look angle

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation; // Set the rotation of the camera pivot based on the pivot angle
    }

    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition; // Start with the default local z position of the camera
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast
            (cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            // If a collision occurs within the camera's movement range, adjust the target position
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition =- (distance - cameraCollisionOffSet);
        }

        if(Mathf.Abs(targetPosition) < minCollisionOffSet)
        {
            // If the adjusted target position is less than the minimum collision offset, move it further back
            targetPosition = targetPosition - minCollisionOffSet;
        }
        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition; // Update the camera's local position using a smooth lerp
    }
}
