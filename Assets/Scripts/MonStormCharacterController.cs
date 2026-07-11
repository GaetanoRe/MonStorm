using System;
using UnityEngine;
using UnityEngine.InputSystem; // Requires Input System Package

[RequireComponent(typeof(CharacterController))]
public class MonStormCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isMoving = false;

    private CharacterAI m_playerAI;
    private InputSystem_Actions m_inputActions;

    void Start()
    {
        isMoving = false;
        controller = GetComponent<CharacterController>();
        m_playerAI = GetComponent<CharacterAI>();
        m_inputActions = new InputSystem_Actions();
        m_inputActions.Player.Enable();
    }

    void OnDestroy()
    {
        m_inputActions.Player.Disable();
    }

    void Update()
    {
        HandleMovement();
        HandleButtons();
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        // 1. Left Stick: Movement (X and Y axes)
        Vector2 moveInput = m_inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        
        if (move.magnitude >= 0.1f)
        {
            isMoving = true;
            controller.Move(move * moveSpeed * Time.deltaTime);

            m_playerAI.HandlePreRunState (); //run

            // Rotate character to face movement direction
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        } else if ((move.magnitude < 0.1f) && (isMoving)) {
            m_playerAI.HandlePreIdleState ();
            isMoving = false;
        }

        // 2. Right Stick: Camera or Look (Custom logic can be added here)
        Vector2 lookInput = m_inputActions.Player.Look.ReadValue<Vector2>();

        // Apply Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleButtons()
    {
        // Face Button Bindings
        if (m_inputActions.Player.Interact.WasPressedThisFrame())
        {
            Debug.Log("Pressed Interact");
        }
        if (m_inputActions.Player.Dodge.WasPressedThisFrame())
        {
            Debug.Log("Pressed Dodge.");
        }

        // Shoulder Buttons & Triggers
        if (m_inputActions.Player.SpecialAction.WasPressedThisFrame())
        {
            Debug.Log("Pressed Special Action");
        }
        if (m_inputActions.Player.CenterCamera.WasPressedThisFrame())
        {
            Debug.Log("Pressed Center Camera");
        }
        if (m_inputActions.Player.Sprint.WasPressedThisFrame())
        {
            Debug.Log("Pressed Dash");
        }
    }
}