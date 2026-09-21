using UnityEngine;

public class HitApplierComponentStandalone : HitApplierComponentConfigured
{
    [SerializeField] float damage = 50f;


    void Awake()
    {
        IHitReceiver ownerReceiver = GetComponentInParent<IHitReceiver>();

        Initialize(new(damage, ownerReceiver));
    }
}
