using UnityEngine;

public class HitDetectorComponentStandalone : HitDetectorComponentConfigured
{
    [SerializeField] float damageMultiplier = 1f;


    void Awake()
    {
        IHitReceiver hitReceiver = GetComponentInParent<IHitReceiver>();

        if (hitReceiver == null)
        {
            Debug.LogWarning("IHitReceiver not assigned to " + gameObject.name);
            return;
        }

        Initialize(new(damageMultiplier, hitReceiver));
    }
}
