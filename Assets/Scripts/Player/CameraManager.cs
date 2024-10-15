using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;
    public Transform playerTransform;
    public Transform cameraTransform;

    [Header("Camera Movement")]
    public Transform cameraPivot;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    public float cameraFollowSpeed = 0.3f;
    public float camLookSpeed = 2f;
    public float camPivotSpeed = 2f;
    public float lookAngle;
    public float pivotAngle;
    public float minPivotAngle = -30f;
    public float maxPivotAngle = 30f;

    [Header("Camera Collision")]
    public LayerMask collisionLayer;
    private float defaultPosition;
    public float cameraCollisionOffset = 0.2f;
    public float minColiisionOffset = 0.2f;
    public float cameraCollisionRadius = 2f;

    private Vector3 cameraVectorPosition;

    private void Awake() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputManager = FindObjectOfType<InputManager>();
        playerTransform = FindObjectOfType<PlayerManager>().transform;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        CameraCollision();
    }

    void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, playerTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);

        transform.position = targetPosition;
    }

    void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        // Horizontal rotation (left-right)
        lookAngle = lookAngle + (inputManager.cameraInputX * camLookSpeed);

        // Invert vertical rotation (up-down) by multiplying with -1
        pivotAngle = pivotAngle + (inputManager.cameraInputY * -1 * camPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        // Apply horizontal rotation
        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        // Apply vertical rotation (pivot)
        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    void CameraCollision()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if(Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayer))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition =- (distance - cameraCollisionOffset);
        }

        if(Mathf.Abs(targetPosition) < minColiisionOffset)
        {
            targetPosition = targetPosition - minColiisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
