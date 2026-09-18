using UnityEngine;

public class HitApplierComponentConfigured : MonoBehaviour
{
    public HitApplier Data { get; private set; }


    public void Initialize(HitApplier hitApplier)
    {
        Data = hitApplier;
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.transform.TryGetComponent(out HitDetector hitDetector)) return;

        Data.UpdateCollisions(hitDetector, this);
    }
}
