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
    public event Action<HitApplier, HitDetector> OnHit;

    /// <summary>Invoked when the creature's Health component takes damage.</summary>
    public event Action<Health, float> OnDamaged;

    public StateMachine<CreatureContext> StateMachine => stateMachine;
    public CreatureContext CreatureContext => creatureContext;

    [SerializeField] CreatureDefinition creatureDefinition;

    HitReceiver hitReceiver;
    Health health;
    ItemDropper itemDropper;

    CreatureContext creatureContext;
    StateMachine<CreatureContext> stateMachine;

    IFSMAdapterAnimator adapterAnimator;
    IFSMAdapterNavMeshAgent adapterNavMeshAgent;
    IFSMAdapterLogger adapterLogger;
    IFSMAdapterTransform adapterTransform;
    IFSMAdapterTransform adapterTransformTarget;
    IFSMAdapterSensorVision adapterSensorVision;

    bool gotStaggeredThisFrame;
    float staggerDamageStored;


    void Awake()
    {
        if (creatureDefinition == null)
        {
            Debug.LogWarning($"CreatureDefinition asset not assigned to {gameObject.name}.");
            return;
        }

        Transform target = FindAnyObjectByType<MonStormCharacterController>().transform;

        TryGetComponent(out hitReceiver);
        TryGetComponent(out health);
        TryGetComponent(out itemDropper);
        
        adapterAnimator = TryGetComponent(out Animator animator) ? new FSMAdapterAnimator(animator) : new FSMAdapterAnimatorNull();
        adapterNavMeshAgent = TryGetComponent(out NavMeshAgent navMeshAgent) ? new FSMAdapterNavMeshAgent(navMeshAgent) : new FSMAdapterNavMeshAgentNull();
        adapterLogger = new FSMAdapterLogger();
        adapterTransform = new FSMAdapterTransform(transform);
        adapterTransformTarget = target != null ? new FSMAdapterTransform(target) : new FSMAdapterTransformNull();
        adapterSensorVision = new FSMAdapterSensorVision(adapterTransform, adapterTransformTarget, creatureDefinition.VisionRadius, creatureDefinition.VisionMaxAngle);

        creatureContext = new(null, adapterAnimator, adapterNavMeshAgent, new FSMAdapterTransform(transform), adapterLogger, adapterTransformTarget, adapterSensorVision);
        stateMachine = creatureDefinition.BuildStateMachine(creatureContext, this);
    }

    void OnEnable()
    {
        if (hitReceiver != null) hitReceiver.OnHit += HandleHit;

        if (health != null) health.OnDeath += HandleDeath;
    }

    void OnDisable()
    {
        if (hitReceiver != null) hitReceiver.OnHit -= HandleHit;

        if (health != null) health.OnDeath -= HandleDeath;
    }

    void Update()
    {
        creatureContext.UpdateContextValues(gotStaggeredThisFrame, health != null && health.IsDead);
        stateMachine.Tick(Time.deltaTime);

        gotStaggeredThisFrame = false;
    }

    void HandleHit(HitApplier applier, HitDetector detector)
    {
        OnHit?.Invoke(applier, detector);

        if (health == null) return;

        if (health.IsDead) return; // Hit can still occur after the Health has died and the gameobject hasn't been destroyed, hence this guard

        float damageDealt = DamageCalculator.Resolve(applier.Damage, detector.DamageMultiplier);

        staggerDamageStored += damageDealt;
        if (staggerDamageStored >= creatureDefinition.StaggerDamageThreshold)
        {
            staggerDamageStored = 0f;
            gotStaggeredThisFrame = true;
        }

        health.Damage(damageDealt);
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
