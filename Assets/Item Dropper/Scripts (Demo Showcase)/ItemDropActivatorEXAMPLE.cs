using UnityEngine;
using UnityEngine.InputSystem;

public class ItemDropActivatorEXAMPLE : MonoBehaviour
{
    ItemDropper itemDropper;


    void Awake()
    {
        itemDropper = FindFirstObjectByType<ItemDropper>();
    }

    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
            itemDropper.Activate();
    }
}
