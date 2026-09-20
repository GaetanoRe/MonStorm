using UnityEngine;
using MonStorm.Core.Combat;
using MonStorm.Core.Player;

public class PlayerCombat : MonoBehaviour, IHitReceiver
{
    public PlayerContext context;

    private HealthComponentConfigured health;

    [SerializeField] HitApplierComponentConfigured hitApplier;
    [SerializeField] private ProgressBar healthBar;


    private void Awake()
    {
        health = GetComponent<HealthComponentConfigured>();
        health.Initialize(new(100f));
        hitApplier.Initialize(new(50f, this));
    }

    private void OnEnable()
    {
        health.Data.OnCurrentUpdated += healthBar.SetValue;
        health.Data.OnMaxUpdated += healthBar.SetMax;
        health.Data.OnDeath += OnPlayerDeath;
    }
    
    private void Start()
    {
        healthBar.SetValue(health.Data.Current);
        healthBar.SetMax(health.Data.Max);
    }

    
    void OnPlayerDeath()
    {
        context.isDead = true;
    }
    
    public void HandleHit(HitApplierComponentConfigured applier, HitDetectorComponentConfigured detector)
    {
        if (health.Data.IsDead) return;

        Vector3 hitDir = transform.position - applier.transform.position;
        hitDir.y = 0;
        hitDir.Normalize();
        context.knockbackDirection = new System.Numerics.Vector3(hitDir.x, hitDir.y, hitDir.z);
        float damageDealt = DamageCalculator.Resolve(applier.Data.Damage, detector.Data.DamageMultiplier);
        context.isHit = true;
        health.Data.Damage(damageDealt);
        Debug.Log($"Hit: {detector.gameObject.name}, damage dealt: {damageDealt}, remaining health: {health.Data.Current}");
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
        health.Data.OnCurrentUpdated -= healthBar.SetValue;
        health.Data.OnMaxUpdated -= healthBar.SetMax;
    }

    void OnDestroy()
    {
        health.Data.OnDeath -= OnPlayerDeath;
    }
}