using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class MonStormPlayerCameraController : MonoBehaviour
{
    [SerializeField] CinemachineOrbitalFollow orbitalCamera;
    [SerializeField] float yawSpeedDegPerSec = 120;
    [SerializeField] float [] pitchPresets = {-5, 17.5f, 40};
    [SerializeField] int pitchIndex = 1;
    [SerializeField] float snapDurationSec = 0.15f;

    private bool _snapping;
    private float _snapStartYaw;
    private float _snapTargetYaw;
    private float _snapTimer;
    private float _prevLookY;
    InputSystem_Actions inputActions; 

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    void OnEnable()
    {
        inputActions.Player.Enable();
    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lookInput = inputActions.Player.Look.ReadValue<Vector2>();
        
        if(lookInput.x != 0)
        {
            orbitalCamera.HorizontalAxis.Value += lookInput.x * yawSpeedDegPerSec * Time.deltaTime;
            _snapping = false;
        }

        if (_snapping)
        {
            _snapTimer += Time.deltaTime;
            float t = Mathf.Clamp01(_snapTimer / snapDurationSec);
            float delta = Mathf.DeltaAngle(_snapStartYaw, _snapTargetYaw);
            orbitalCamera.HorizontalAxis.Value = _snapStartYaw + delta * t;
            if (t >= 1f) _snapping = false;
        }

    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    public void SnapTo(Transform target, Transform player)
    {
        
    }
}
