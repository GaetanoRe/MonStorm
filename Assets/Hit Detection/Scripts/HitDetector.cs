using UnityEngine;

public class HitDetector : MonoBehaviour
{
    /// <summary>Multiplies any incoming damage.</summary>
    /// <remarks>For example the head of an animal can take 1.5x damage or the legs can take 0.5x damage, 1x remains unchanged.</remarks>
    [field: SerializeField] public float DamageMultiplier { get; private set; } = 1f;
    public HitDetectionManager HitDetectionManager { get; private set; }


    void Awake()
    {
        HitDetectionManager = GetComponentInParent<HitDetectionManager>();

        if (HitDetectionManager == null)
            Debug.LogWarning("Hit detection manager not assigned to " + gameObject.name);
    }
}
