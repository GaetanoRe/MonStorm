public class HitDetector
{
    /// <summary>Multiplies any incoming damage.</summary>
    /// <remarks>For example the head of an animal can take 1.5x damage or the legs can take 0.5x damage, 1x remains unchanged.</remarks>
    public float DamageMultiplier { get; }
    public IHitReceiver HitReceiver { get; }


    public HitDetector(float damageMultiplier, IHitReceiver hitReceiver)
    {
        DamageMultiplier = damageMultiplier;
        HitReceiver = hitReceiver;
    }
}
