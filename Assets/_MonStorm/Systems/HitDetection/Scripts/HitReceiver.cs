using UnityEngine;
using System;

public class HitReceiver : MonoBehaviour, IHitReceiver
{
    public event Action<HitApplier, HitDetector> OnHit;


    public void HandleHit(HitApplier applier, HitDetector detector) => OnHit?.Invoke(applier, detector);
}
