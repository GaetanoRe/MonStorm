using UnityEngine;
using System;

public class HitReceiver : MonoBehaviour, IHitReceiver
{
    public event Action<HitApplierComponentConfigured, HitDetectorComponentConfigured> OnHit;


    public void HandleHit(HitApplierComponentConfigured applier, HitDetectorComponentConfigured detector) => OnHit?.Invoke(applier, detector);
}
