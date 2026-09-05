using UnityEngine;
using MonStorm.Core.Player;


[CreateAssetMenu(menuName = "MonStorm/Control Scheme Settings", fileName = "New Control Scheme Setting Config")]
public class ControlSchemeSettings : ScriptableObject
{
    [SerializeField] public ControlScheme gamepadScheme;
    [SerializeField] public float rearmThreshold;
    [SerializeField] public float deadzone;
    [SerializeField] public float comboBufferWindow;

    public ActionResolverConfig Build()
    {
        return new ActionResolverConfig(rearmThreshold, deadzone, comboBufferWindow);
    }
}