using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using MonStorm.Core.Player;
using MonStorm.Core.StateMachine;
using MonStorm.Adapters;

[RequireComponent(typeof(CharacterController))]
public class MonStormCharacterController : MonoBehaviour
{
    [Header("Cinemachine Camera Settings")]
    public CinemachineCamera playerCamera;
    [SerializeField] private MonStormPlayerCameraController _cameraController;

    [Header("Movement Settings")]
    public float rotationSpeed = 720f;
    public float gravity = -9.81f;

    [SerializeField] public float rearmThreshold = 0.2f;
    [SerializeField] public float deadzone = 0.5f;

    [SerializeField] public WeaponAsset _equippedWeapon;
    public HitApplier weapon;

    private CharacterController controller;
    private Vector3 velocity;
    [SerializeField] private float _lockOnRadius;
    [SerializeField] private LayerMask _enemyLayer;
    

    private InputSystem_Actions m_inputActions;
    private StateMachine<PlayerContext> m_stateMachine;
    private PlayerContext m_playerContext;

    private ActionResolver actionResolver;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        m_inputActions = new InputSystem_Actions();
        m_inputActions.Player.Enable();

        m_playerContext = new PlayerContext();
        GetComponent<PlayerCombat>().context = m_playerContext;
        GetComponent<PlayerAnimationDriver>().context = m_playerContext;

        m_stateMachine = new StateMachine<PlayerContext>(m_playerContext);
        m_playerContext.StateMachine = m_stateMachine;
        m_playerContext.AdapterAnimator = new FSMAdapterAnimator(GetComponent<Animator>());
        m_playerContext.equippedWeapon = _equippedWeapon.Build();
        m_playerContext.AdapterHitApplier = new FSMAdapterHitApplier(weapon);
        m_playerContext.WalkAnimHash = Animator.StringToHash("RunForward");
        m_playerContext.RunAnimHash = Animator.StringToHash("Sprint");
        m_playerContext.IdleAnimHash = Animator.StringToHash("Idle");
        m_playerContext.DamagedAnimHash = Animator.StringToHash("GetHit");
        m_playerContext.DeathAnimHash = Animator.StringToHash("Death");
        m_playerContext.AttackAnimHash = Animator.StringToHash("MeleeAttack_TwoHanded");
        actionResolver = new ActionResolver(rearmThreshold, deadzone);
        

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
        Vector2 raw = m_inputActions.Player.Move.ReadValue<Vector2>();

        Vector2 rightStickGestures = m_inputActions.Player.AttackActions.ReadValue<Vector2>();
        bool rightStickPressed = m_inputActions.Player.Action5.WasPerformedThisFrame();
        bool specialAttackPressed = m_inputActions.Player.SpecialAction.WasPerformedThisFrame();

        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0;
        camRight.Normalize();
        Vector3 worldMove = camForward * raw.y + camRight * raw.x;

        m_playerContext.moveInput.X = worldMove.x;
        m_playerContext.moveInput.Y = worldMove.z;
        m_playerContext.isGrounded = controller.isGrounded;
        m_playerContext.isSprinting = m_inputActions.Player.Sprint.IsPressed();
        m_playerContext.dodgePressed = m_inputActions.Player.Dodge.WasPressedThisFrame();
        m_playerContext.attackPressed = m_inputActions.Player.EnableAttack.WasPressedThisFrame();
        m_playerContext.weaponAction = actionResolver.actionResolve(rightStickGestures.x, rightStickGestures.y, rightStickPressed, specialAttackPressed);
        
    }

 void HandleMovement()
  {
      if (controller.isGrounded && velocity.y < 0)
          velocity.y = -2f;

      Vector3 move = new Vector3(m_playerContext.velocity.X, 0, m_playerContext.velocity.Z);

      if (move.magnitude >= 0.1f)
      {
          Quaternion targetRotation = Quaternion.LookRotation(move);
          transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed *
        Time.deltaTime);
      }

      velocity.y += gravity * Time.deltaTime;
      controller.Move(new Vector3(m_playerContext.velocity.X, velocity.y, m_playerContext.velocity.Z) *
    Time.deltaTime);
  }


    void HandleButtons()
    {
        if (m_inputActions.Player.Interact.WasPressedThisFrame())
            Debug.Log("Pressed Interact");
        if (m_inputActions.Player.SpecialAction.WasPressedThisFrame())
            Debug.Log("Pressed Special Action");
        if (m_inputActions.Player.CenterCamera.WasPressedThisFrame())
        {
            Transform target = FindNearestTarget();
            m_playerContext.isTargeting = target != null;
            _cameraController.SnapTo(target, transform);
        }
           
        if (m_inputActions.Player.EnableAttack.WasPressedThisFrame())
            Debug.Log("Pressed Attack");
    }

    Transform FindNearestTarget(){
        Collider [] hits = Physics.OverlapSphere(transform.position, _lockOnRadius, _enemyLayer);
        Transform nearest = null;
        float nearestDist = float.MaxValue;
        foreach(Collider hit in hits)
        {
            Health enemyHealth = hit.GetComponentInParent<Health>();
            if (enemyHealth == null) continue;
            Transform root = enemyHealth.transform;
            float dist = Vector3.Distance(transform.position, root.position);
            if(dist < nearestDist)
            {
                nearestDist = dist;
                nearest = root;
            }

        }

        return nearest;
    }
}
