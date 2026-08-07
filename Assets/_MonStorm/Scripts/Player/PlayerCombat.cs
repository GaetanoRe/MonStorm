using UnityEngine;
using MonStorm.Core.Combat;
using System;
using MonStorm.Core.StateMachine;
using MonStorm.Core.Player;

public class PlayerCombat : MonoBehaviour, IHitReceiver
{
    public PlayerContext context;

    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();
        health.OnDeath += OnPlayerDeath;
    }

    void OnPlayerDeath()
    {
        context.isDead = true;
    }
    
    public void HandleHit(HitApplier applier, HitDetector detector)
    {
        if (health.IsDead) return;
        float damageDealt = DamageCalculator.Resolve(applier.Damage, detector.DamageMultiplier);
        context.isHit = true;
        health.Damage(damageDealt);
        Debug.Log($"Hit: {detector.gameObject.name}, damage dealt: {damageDealt}, remaining health: {health.CurrentHealth}");

		
    }

    void OnDestroy()
    {
      health.OnDeath -= OnPlayerDeath;
    }
  

}