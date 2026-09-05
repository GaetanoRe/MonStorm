using UnityEngine;
using MonStorm.Core.Combat;
using System;
using MonStorm.Core.StateMachine;
using MonStorm.Core.Player;

public class PlayerCombat : MonoBehaviour, IHitReceiver
{
    public PlayerContext context;

    private Health health;

    [SerializeField] private ProgressBar healthBar;


    private void Awake()
    {
        health = GetComponent<Health>();
    }
    private void OnEnable()
    {
        health.OnCurrentHealthUpdated += healthBar.SetValue;
        health.OnMaxHealthUpdated += healthBar.SetMax;       
        health.OnDeath += OnPlayerDeath;
    }
    
    private void Start()
    {
        healthBar.SetValue(health.CurrentHealth);
        healthBar.SetMax(health.MaxHealth);
    }

    
    void OnPlayerDeath()
    {
        context.isDead = true;
    }
    
    public void HandleHit(HitApplier applier, HitDetector detector)
    {
        
        if (health.IsDead) return;
        Vector3 hitDir = transform.position - applier.transform.position;
        hitDir.y = 0;
        hitDir.Normalize();
        context.knockbackDirection = new System.Numerics.Vector3(hitDir.x, hitDir.y, hitDir.z);
        float damageDealt = DamageCalculator.Resolve(applier.Damage, detector.DamageMultiplier);
        context.isHit = true;
        health.Damage(damageDealt);
        Debug.Log($"Hit: {detector.gameObject.name}, damage dealt: {damageDealt}, remaining health: {health.CurrentHealth}");

		
    }

    public void OnHitWindowOpen()
    {
        context.AdapterHitApplier.SetActive(true);
    }

    public void OnHitWindowClose()
    {
        context.AdapterHitApplier.SetActive(false);
    }

    private void OnDisable()
    {
        health.OnCurrentHealthUpdated -= healthBar.SetValue;
        health.OnMaxHealthUpdated -= healthBar.SetMax;
    }

    void OnDestroy()
    {
      health.OnDeath -= OnPlayerDeath;
    }
  

}