using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlayer : MonoBehaviour
{
    InputManager inputManager;
    PlayerManager playerManager;
    Vector3 moveDirection;
    Transform cameraGameObject;
    Rigidbody rb;

    [Header("Falling and Landing")]
    public Transform groundCheck;
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float raycastHeightOffset = 0.5f;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isMoving;
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Values")]
    public float movementSpeed = 2f;
    public float rotationSpeed = 13f;

    public float sprintingSpeed = 7f;

    [Header("Jump Var")]
    public float jumpHeight = 4f;
    public float gravityIntensity = -15f;

    private void Awake() 
    {
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody>();
        cameraGameObject = Camera.main.transform;
    }

    public void HandleAllMovements()
    {
        HandleMovement();
        HandleRotation();
        HandleFallingAndLanding();
    }

    void HandleMovement()
    {
        moveDirection = new Vector3(cameraGameObject.forward.x, 0f, cameraGameObject.forward.z) * inputManager.verticalInput;
        moveDirection = moveDirection + cameraGameObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();

        moveDirection.y = 0;

        if(isSprinting)
        {
            moveDirection = moveDirection * sprintingSpeed;
        }
        else
        {
            if(inputManager.movementAmount >= 0.5f)
            {
                moveDirection = moveDirection * movementSpeed;
                isMoving = true;
            }

            if(inputManager.movementAmount >= 0f)
            {
                isMoving = false;
            }
        }

        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;
    }

    void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraGameObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraGameObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = groundCheck.position;
        Vector3 targetPosition;
        raycastOrigin.y += raycastHeightOffset;
        targetPosition = groundCheck.position;
        
        if(!isGrounded && !isJumping)
        {
            inAirTimer += Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(-transform.up * fallingVelocity * inAirTimer);
        }

        // Adjust the SphereCast to a longer distance and fine-tune the parameters
        float raycastDistance = 0f; // Adjust this distance as needed
        if(Physics.SphereCast(raycastOrigin, 0.2f, -groundCheck.up, out hit, raycastDistance, groundLayer))
        {
            Vector3 raycastHitPoint = hit.point;
            targetPosition.y = raycastHitPoint.y;
            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if(isGrounded && !isJumping)
        {
            if(inputManager.movementAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }

    public void HandleJumping()
    {
        if(isGrounded)
        {
            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            rb.velocity = playerVelocity;

            isJumping = false;
        }
    }

    public void SetIsJumping(bool isJumping)
    {
        this.isJumping = isJumping;
    }
}
