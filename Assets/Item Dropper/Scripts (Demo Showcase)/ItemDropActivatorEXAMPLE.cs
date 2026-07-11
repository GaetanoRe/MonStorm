using UnityEngine;
using UnityEngine.InputSystem;

public class ItemDropActivatorEXAMPLE : MonoBehaviour
{
    [SerializeField] ItemDropper itemDropper;


    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
            itemDropper.Activate();
    }
}
