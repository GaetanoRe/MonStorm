using UnityEngine;
using UnityEngine.AI;
using System;
using MonStorm.Adapters;
using MonStorm.Core.StateMachine;
using MonStorm.Core.Combat;

public class CreatureBehavior : MonoBehaviour
{
    /// <summary>Invoked when the creature registers a hit, different from OnDamaged because the creature could get hit while immune, or some other restriction.</summary>
    /// <remarks>Always called first, before OnDamaged.</remarks>
    public event Action<HitApplierComponentConfigured, HitDetectorComponentConfigured> OnHit;

    /// <summary>Invoked when the creature's Health component takes damage.</summary>
    public event Action<HealthComponentConfigured, float> OnDamaged;

    [field: SerializeField] public CreatureDefinition CreatureDefinition { get; private set; }

    public CreatureContext CreatureContext { get; private set; }
    public StateMachine<CreatureContext> StateMachine { get; private set; }

    HitReceiver hitReceiver;
    HealthComponentConfigured health;
    ItemDropperComponentConfigured itemDropper;

    IFSMAdapterAnimator adapterAnimator;
    IFSMAdapterNavMeshAgent adapterNavMeshAgent;
    IFSMAdapterLogger adapterLogger;

    bool gotHitThisFrame;


    public bool ConditionGotHitThisFrame() => gotHitThisFrame;
    public bool ConditionIsDead() => health != null && health.Data.IsDead;
    public bool ConditionIsAnimationFinished() => adapterAnimator.IsAnimationFinished;
    public bool ConditionHasReachedDestination() => adapterNavMeshAgent.HasActivePath;

    void Awake()
    {
        if (CreatureDefinition == null)
        {
            Debug.LogWarning($"CreatureDefinition asset not assigned to {gameObject.name}.");
            return;
        }

        TryGetComponent(out hitReceiver);

        if (TryGetComponent(out health))
        {
            health.Initialize(new(CreatureDefinition.MaxHealth));
        }

        if (TryGetComponent(out itemDropper))
        {
            itemDropper.Initialize(new(CreatureDefinition.LootTable));
        }
        
        adapterAnimator = TryGetComponent(out Animator animator) ? new FSMAdapterAnimator(animator) : new FSMAdapterAnimatorNull();
        adapterNavMeshAgent = TryGetComponent(out NavMeshAgent navMeshAgent) ? new FSMAdapterNavMeshAgent(navMeshAgent) : new FSMAdapterNavMeshAgentNull();
        adapterLogger = new FSMAdapterLogger();

        CreatureContext = new(null, adapterAnimator, adapterNavMeshAgent, new FSMAdapterTransform(transform), adapterLogger, new FSMAdapterTransform(FindAnyObjectByType<MonStormCharacterController>().transform));
        StateMachine = CreatureDefinition.BuildStateMachine(CreatureContext, this);
    }

    void OnEnable()
    {
        if (hitReceiver != null) hitReceiver.OnHit += HandleHit;

        if (health != null) health.Data.OnDeath += HandleDeath;
    }

    void OnDisable()
    {
        if (hitReceiver != null) hitReceiver.OnHit -= HandleHit;

        if (health != null) health.Data.OnDeath -= HandleDeath;
    }

    void Update()
    {
        StateMachine.Tick(Time.deltaTime);

        gotHitThisFrame = false;
    }

    void HandleHit(HitApplierComponentConfigured applier, HitDetectorComponentConfigured detector)
    {
        gotHitThisFrame = true;
        OnHit?.Invoke(applier, detector);

        if (health == null) return;

        if (health.Data.IsDead) return; // Hit can still occur after the Health has died and the gameobject hasn't been destroyed, hence this guard

        float damageDealt = DamageCalculator.Resolve(applier.Data.Damage, detector.Data.DamageMultiplier);
        health.Data.Damage(damageDealt);
        OnDamaged?.Invoke(health, damageDealt);
    }

    void HandleDeath()
    {
        if (itemDropper != null)
        {
            itemDropper.Activate(1.5f);
        }

        Destroy(gameObject, 5f);
    }
}
