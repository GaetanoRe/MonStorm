using UnityEngine;

public class HealthComponentConfigured : MonoBehaviour
{
    public Health Data { get; private set; }


    public void Initialize(Health health)
    {
        Data = health;
    }
}
