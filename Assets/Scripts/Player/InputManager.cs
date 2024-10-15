using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerController playerController;
    MovingPlayer playerMovement;
    public Vector2 movementInput;
    public Vector2 cameraMovementInput;

    public float verticalInput;
    public float horizontalInput;
    public float cameraInputX;
    public float cameraInputY;
    public float movementAmount;

    [Header("Input Button")]
    public bool bInput;
    public bool jumpInput;

    private void Awake() 
    {
        playerMovement = GetComponent<MovingPlayer>();
    }

    private void OnEnable()
    {
        if(playerController == null)
        {
            playerController = new PlayerController();

            playerController.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerController.PlayerMovement.CameraMovement.performed += i => cameraMovementInput = i.ReadValue<Vector2>();
            playerController.PlayerAction.B.performed += i => bInput = true;
            playerController.PlayerAction.B.canceled += i => bInput = false;
            playerController.PlayerAction.Jump.performed += i => jumpInput = true;
        }

        playerController.Enable();
    }

    private void OnDisable() 
    {
        playerController.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraMovementInput.x;
        cameraInputY = cameraMovementInput.y;

        movementAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
    }

    private void HandleSprintingInput()
    {
        if(bInput && movementAmount > 0.5)
        {
            playerMovement.isSprinting = true;
        }
        else
        {
            playerMovement.isSprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if(jumpInput)
        {
            jumpInput = false;
            playerMovement.HandleJumping();
        }
    }
}
