using System;

public interface IHitDetectionManager
{
    public event Action<HitApplier, HitDetector> OnHit;


    public void InvokeHit(HitApplier applier, HitDetector detector);
}
