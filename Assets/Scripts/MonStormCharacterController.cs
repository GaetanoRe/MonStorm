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
        // Face Buttons
        if (m_inputActions.Player.Jump.WasPressedThisFrame()) { 
            m_playerAI.HandlePreJumpState (); //jump
            Debug.Log("Xbox A Pressed (Jump)");
        }
        /**if (m_inputActions.Player.Jump.WasPressedThisFrame()) {
            m_playerAI.HandlePreFleeState (); //evade
            Debug.Log("Xbox B Pressed");
        }**/
        if (m_inputActions.Player.Attack.WasPressedThisFrame()) {
            m_playerAI.HandlePreAttackState (); //attack
            Debug.Log("Xbox X Pressed");
        }
        if (m_inputActions.Player.Interact.WasPressedThisFrame()) {
            Debug.Log("Xbox Y Pressed");
        }

        // Bumpers and Triggers
        /**if (gamepad.leftShoulder.wasPressedThisFrame) {
            Debug.Log("LB Pressed");
        }
        if (gamepad.rightShoulder.wasPressedThisFrame) {
            Debug.Log("RB Pressed");
        }
        
        float leftTrigger = gamepad.leftTrigger.ReadValue(); // Analog 0.0 to 1.0
        float rightTrigger = gamepad.rightTrigger.ReadValue();

        // D-Pad
        if (gamepad.dpad.up.wasPressedThisFrame) {
            Debug.Log("D-Pad Up");
        }
        
        // Stick Clicks
        if (gamepad.leftStickButton.wasPressedThisFrame) {
            Debug.Log("L3 Clicked");
        }
        if (gamepad.rightStickButton.wasPressedThisFrame) {
            Debug.Log("R3 Clicked");
        }

        // Menu Buttons
        if (gamepad.startButton.wasPressedThisFrame) {
            Debug.Log("Start Pressed");
        }
        if (gamepad.selectButton.wasPressedThisFrame) {
            Debug.Log("Select/Back Pressed");
        }**/
    }
}