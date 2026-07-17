using UnityEngine;
using MonStorm.Core.Combat;
using System;

public class PlayerCombat : MonoBehaviour, IHitDetectionManager
{

    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();
        if (health)
        {
            
        }
    }
    
    public void HandleHit(HitApplier applier, HitDetector detector)
    {
        if (health.IsDead) return;
        float damageDealt = DamageCalculator.Resolve(applier.Damage, detector.DamageMultiplier);
        health.Damage(damageDealt);
        Debug.Log($"Hit: {detector.gameObject.name}, damage dealt: {damageDealt}, remaining health: {health.CurrentHealth}");

		
    }
}