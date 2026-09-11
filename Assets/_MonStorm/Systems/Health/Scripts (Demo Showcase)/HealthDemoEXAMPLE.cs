using UnityEngine;
using UnityEngine.InputSystem;

public class HealthDemoEXAMPLE : MonoBehaviour
{
    HealthComponentConfigured health;


    void Awake()
    {
        health = FindFirstObjectByType<HealthComponentConfigured>();
    }

    void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            health.Data.Damage(10f);
            Debug.Log("CURRENT: " + health.Data.Current);

            if (health.Data.Current == 0f)
                Debug.Log("Dead");
        }
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            health.Data.Heal(4f);
            Debug.Log("CURRENT: " + health.Data.Current);
        }
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            health.Data.ChangeMax(10f);
            Debug.Log("MAX: " + health.Data.Max);
        }
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            health.Data.ChangeMax(-10f);
            Debug.Log("MAX: " + health.Data.Max);
        }
    }
}
