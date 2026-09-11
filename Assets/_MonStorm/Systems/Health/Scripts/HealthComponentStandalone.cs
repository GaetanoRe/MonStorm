using UnityEngine;

public class HealthComponentStandalone : HealthComponentConfigured
{
    [SerializeField] float maxHealth = 100f;


    void Awake()
    {
        Initialize(new(maxHealth));
    }
}
