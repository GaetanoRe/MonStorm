using UnityEngine;

public class HitProcessingEXAMPLE : MonoBehaviour, IHitReceiver
{
    HealthComponentConfigured health;


    public void HandleHit(HitApplierComponentConfigured applier, HitDetectorComponentConfigured detector)
    {
        float damageDealt = applier.Data.Damage * detector.Data.DamageMultiplier;
        health.Data.Damage(damageDealt);

        Debug.Log($"Hit: {detector.gameObject.name}, damage dealt: {damageDealt}, remaining health: {health.Data.Current}");
    }

    void Awake()
    {
        health = GetComponent<HealthComponentConfigured>();
        health.Data.OnDeath += Die;
    }

    void Die()
    {
        health.Data.OnDeath -= Die;
        Destroy(gameObject);
    }
}
