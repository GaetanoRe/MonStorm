using UnityEngine;
using System;

public class HitReceiver : MonoBehaviour, IHitReceiver
{
    public event Action<HitApplierComponentConfigured, HitDetector> OnHit;


    public void HandleHit(HitApplierComponentConfigured applier, HitDetector detector) => OnHit?.Invoke(applier, detector);
}
