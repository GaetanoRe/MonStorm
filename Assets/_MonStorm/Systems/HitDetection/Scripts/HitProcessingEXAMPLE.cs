using UnityEngine;

public class HitProcessingEXAMPLE : MonoBehaviour, IHitReceiver
{
    Health health;


    public void HandleHit(HitApplier applier, HitDetector detector)
    {
        float damageDealt = applier.Damage * detector.DamageMultiplier;
        health.Damage(damageDealt);

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
