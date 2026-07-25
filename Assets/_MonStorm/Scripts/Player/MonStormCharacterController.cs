using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using MonStorm.Core.Player;
using MonStorm.Core.StateMachine;

[RequireComponent(typeof(CharacterController))]
public class MonStormCharacterController : MonoBehaviour
{
    [Header("Cinemachine Camera Settings")]
    public CinemachineCamera playerCamera;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;

    private InputSystem_Actions m_inputActions;
    private StateMachine<PlayerContext> m_stateMachine;
    private PlayerContext m_playerContext;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        m_inputActions = new InputSystem_Actions();
        m_inputActions.Player.Enable();

        m_playerContext = new PlayerContext
        {
            config = new PlayerConfig()
        };

        m_stateMachine = new StateMachine<PlayerContext>(m_playerContext);
        m_stateMachine.TransitionTo(new PlayerIdleState());
    }

    void OnDestroy()
    {
        m_inputActions.Player.Disable();
    }

    void Update()
    {
        UpdateContext();
        m_stateMachine.Tick(Time.deltaTime);
        HandleMovement();
        HandleButtons();
    }

    void UpdateContext()
    {
        m_playerContext.moveInput = m_inputActions.Player.Move.ReadValue<Vector2>();
        m_playerContext.isGrounded = controller.isGrounded;
        m_playerContext.isSprinting = m_inputActions.Player.Sprint.IsPressed();
        m_playerContext.dodgePressed = m_inputActions.Player.Dodge.WasPressedThisFrame();
    }

    void HandleMovement()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        Vector2 moveInput = m_playerContext.moveInput;
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        if (move.magnitude >= 0.1f)
        {
            controller.Move(move * moveSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleButtons()
    {
        if (m_inputActions.Player.Interact.WasPressedThisFrame())
            Debug.Log("Pressed Interact");
        if (m_inputActions.Player.SpecialAction.WasPressedThisFrame())
            Debug.Log("Pressed Special Action");
        if (m_inputActions.Player.CenterCamera.WasPressedThisFrame())
            Debug.Log("Pressed Center Camera");
        if (m_inputActions.Player.EnableAttack.WasPressedThisFrame())
            Debug.Log("Pressed Attack");
    }
}
