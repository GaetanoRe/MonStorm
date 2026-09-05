using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class MonStormPlayerCameraController : MonoBehaviour
{
    [SerializeField] CinemachineOrbitalFollow orbitalCamera;
    [SerializeField] float yawSpeedDegPerSec = 120;
    [SerializeField] bool invertControllerX = false;
    [SerializeField] bool invertControllerY = false;
    [SerializeField] bool invertMouseX = false;
    [SerializeField] bool invertMouseY = false;
    [SerializeField] float [] pitchPresets = {-5, 17.5f, 40};
    [SerializeField] int pitchIndex = 1;
    [SerializeField] float snapDurationSec = 0.15f;
    [SerializeField] float pitchSmoothTime = 0.2f;
    [SerializeField] float mousePitchSensitivity = 0.1f;
    [SerializeField] float mouseYawSensitivity = 0.2f;
    [SerializeField] float controllerPitchSensitivity = 0.5f;
    [SerializeField] float lookDeadzone = 0.15f;
    [SerializeField] float dominanceRatio = 0.5f;
    [SerializeField] bool classicCam;
    [SerializeField] private ControlSchemeSettings _controlScheme;

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
        Cursor.visible = false;
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
        Vector2 lookInput;
        float invertControllerValueX = invertControllerX ? -1f : 1f;
        float invertControllerValueY = invertControllerY ? -1f : 1f;
        float invertMouseValueX = invertMouseX ? -1f : 1f;
        float invertMouseValueY = invertMouseY ? -1f : 1f;

        

        if(_controlScheme.gamepadScheme == MonStorm.Core.Player.ControlScheme.Button)
        {
            lookInput = inputActions.Player.LookStick.ReadValue<Vector2>();
            classicCam = false;
        }
        else
        {
            lookInput = inputActions.Player.Look.ReadValue<Vector2>();
            classicCam = true;
        }

        float absX = Mathf.Abs(lookInput.x);
        float absY = Mathf.Abs(lookInput.y);
        if(absX < lookDeadzone || absX < dominanceRatio * absY)
        {
            lookInput.x = 0;
        }
        if(absY < lookDeadzone || absY < dominanceRatio * absX)
        {
            lookInput.y = 0;
        }

        float localYaw;
        float localPitch;

        lookInput.x *= invertControllerValueX;
        lookInput.y *= invertControllerValueY;

        float mouseX = (Mouse.current?.delta.x.ReadValue() ?? 0f) * invertMouseValueX;
        localYaw = (mouseX * mouseYawSensitivity) + (lookInput.x * yawSpeedDegPerSec * Time.deltaTime);

        if(localYaw != 0)
        {
            _snapping = false;
        }

        orbitalCamera.HorizontalAxis.Value += localYaw;


        if (_snapping)
        {
            _snapTimer += Time.deltaTime;
            float t = Mathf.Clamp01(_snapTimer / snapDurationSec);
            float delta = Mathf.DeltaAngle(_snapStartYaw, _snapTargetYaw);
            orbitalCamera.HorizontalAxis.Value = _snapStartYaw + delta * t;
            if (t >= 1f) _snapping = false;
        }

        if (!classicCam)
        {
            float mouseY = (Mouse.current?.delta.y.ReadValue() ?? 0f) * invertMouseValueY;

            localPitch = (mouseY * mousePitchSensitivity) + (lookInput.y * controllerPitchSensitivity * Time.deltaTime);
            orbitalCamera.VerticalAxis.Value = Mathf.Clamp(orbitalCamera.VerticalAxis.Value + localPitch, pitchPresets[0], pitchPresets[pitchPresets.Length - 1]);
        }  
        else
        {

            // Classic MH Style Cam (Classic Cam)
            if (lookInput.y > 0.5f && _prevLookY <= 0.5f)
                pitchIndex = Mathf.Clamp(pitchIndex + 1, 0, pitchPresets.Length - 1);
            else if (lookInput.y < -0.5f && _prevLookY >= -0.5f)
                pitchIndex = Mathf.Clamp(pitchIndex - 1, 0, pitchPresets.Length - 1);                
            orbitalCamera.VerticalAxis.Value = Mathf.SmoothDamp(
                orbitalCamera.VerticalAxis.Value,
                pitchPresets[pitchIndex],
                ref _pitchCurrentVel,
                pitchSmoothTime
            );
        }

        _prevLookY = lookInput.y;

        

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
