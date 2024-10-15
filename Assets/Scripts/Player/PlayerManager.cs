using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    MovingPlayer playerMovement;
    CameraManager cameraManager;

    private void Awake() 
    {
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<MovingPlayer>();
        cameraManager = FindObjectOfType<CameraManager>();
    }
    
    private void Update() 
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate() 
    {
        playerMovement.HandleAllMovements();
    }

    private void LateUpdate() 
    {
        cameraManager.HandleAllCameraMovement();
    }
}
