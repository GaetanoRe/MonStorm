using UnityEngine;
using System;

public class HitProcessing : MonoBehaviour, IHitDetectionManager
{
    public event Action<HitApplier, HitDetector> OnHit;

    Health health;


    public void InvokeHit(HitApplier applier, HitDetector detector)
    {
        float damageDealt = applier.Damage * detector.DamageMultiplier;
        health.Damage(damageDealt);

        OnHit?.Invoke(applier, detector);
        Debug.Log($"Hit: {detector.gameObject.name}, damage dealt: {damageDealt}, remaining health: {health.CurrentHealth}");
    }

    void Awake()
    {
        health = GetComponent<Health>();
        health.OnDeath += Die;
    }

    void Die()
    {
        health.OnDeath -= Die;
        Destroy(gameObject);
    }
}
