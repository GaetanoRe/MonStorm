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
    [SerializeField] float pitchSmoothTime = 0.2f;
    [SerializeField] float mousePitchSensitivity = 0.1f;
    [SerializeField] float controllerPitchSensitivity = 0.5f;
    [SerializeField] bool classicCam;

    private bool _snapping;
    private float _snapStartYaw;
    private float _snapTargetYaw;
    private float _snapTimer;
    private float _prevLookY;
    private float _pitchCurrentVel;
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

        float mouseY = Mouse.current?.delta.y.ReadValue() ?? 0f;
        if(mouseY != 0f)
        {
            // Camera vertically works regular
            float mouseMoveY = mouseY * mousePitchSensitivity;
            orbitalCamera.VerticalAxis.Value = Mathf.Clamp(orbitalCamera.VerticalAxis.Value + mouseMoveY, pitchPresets[0], pitchPresets[pitchPresets.Length - 1]);
        }
        else if (!classicCam)
        {
            float analogueY = lookInput.y * controllerPitchSensitivity * Time.deltaTime;
            orbitalCamera.VerticalAxis.Value = Mathf.Clamp(orbitalCamera.VerticalAxis.Value + analogueY, pitchPresets[0], pitchPresets[pitchPresets.Length - 1]);
        }
        else
        {

            // Classic MH Style Cam (Classic Cam)
            if (lookInput.y > 0.5f && _prevLookY <= 0.5f)
                pitchIndex = Mathf.Clamp(pitchIndex + 1, 0, pitchPresets.Length - 1);
            else if (lookInput.y < -0.5f && _prevLookY >= -0.5f)
                pitchIndex = Mathf.Clamp(pitchIndex - 1, 0, pitchPresets.Length - 1);                
            _prevLookY = lookInput.y;
            orbitalCamera.VerticalAxis.Value = Mathf.SmoothDamp(
                orbitalCamera.VerticalAxis.Value,
                pitchPresets[pitchIndex],
                ref _pitchCurrentVel,
                pitchSmoothTime
            );
        }

        

    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    public void SnapTo(Transform target, Transform player)
    {
        float desiredYaw = 0;
        if(target != null)
        {
            Vector3 camPos = target.position - player.position;
            camPos.y = 0;
            desiredYaw = Mathf.Atan2(camPos.x, camPos.z) * Mathf.Rad2Deg;
        }
        else
        {
            desiredYaw = player.eulerAngles.y;
        }

        _snapStartYaw = orbitalCamera.HorizontalAxis.Value;
        _snapTargetYaw = desiredYaw;
        _snapTimer = 0f;
        _snapping = true;
    }
}
