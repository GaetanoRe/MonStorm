using UnityEngine;
using System;

public class HitDetectionManager : MonoBehaviour
{
    public event Action<HitApplier, HitDetector> OnHit;


    public void InvokeHit(HitApplier applier, HitDetector detector)
    {
        OnHit?.Invoke(applier, detector);
        Debug.Log($"Hit: {detector.gameObject.name}, damage dealt: {applier.Damage * detector.DamageMultiplier}");
    }
}
