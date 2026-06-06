using UnityEngine;
using UnityEngine.InputSystem;

public class HealthDemoEXAMPLE : MonoBehaviour
{
    Health health;


    void Awake()
    {
        health = FindFirstObjectByType<Health>();
    }

    void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            health.Damage(10f);
            Debug.Log("CURRENT: " + health.CurrentHealth);

            if (health.CurrentHealth == 0f)
                Debug.Log("Dead");
        }
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            health.Heal(4f);
            Debug.Log("CURRENT: " + health.CurrentHealth);
        }
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            health.ChangeMaxHealth(10f);
            Debug.Log("MAX: " + health.MaxHealth);
        }
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            health.ChangeMaxHealth(-10f);
            Debug.Log("MAX: " + health.MaxHealth);
        }
    }
}
