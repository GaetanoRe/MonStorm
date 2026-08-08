using UnityEngine;
using UnityEngine.AI;
using System;
using MonStorm.Adapters;
using MonStorm.Core.StateMachine;

public class CreatureBehavior : MonoBehaviour
{
    public event Action<HitApplier, HitDetector> OnHit;
    public event Action<Health, float> OnDamaged;

    public StateMachine<CreatureContext> StateMachine => stateMachine;

    [SerializeField] CreatureDefinition creatureDefinition;

    HitReceiver hitReceiver;
    Health health;
    ItemDropper itemDropper;

    CreatureContext creatureContext;
    StateMachine<CreatureContext> stateMachine;

    IFSMAdapterAnimator adapterAnimator;
    IFSMAdapterNavMeshAgent adapterNavMeshAgent;
    IFSMAdapterLogger adapterLogger;

    bool gotHitThisFrame;


    public bool ConditionGotHitThisFrame() => gotHitThisFrame;
    public bool ConditionIsDead() => health != null && health.IsDead;
    public bool ConditionIsAnimationFinished() => adapterAnimator.IsAnimationFinished;
    public bool ConditionHasReachedDestination() => adapterNavMeshAgent.HasActivePath;

    void Awake()
    {
        if (creatureDefinition == null)
        {
            Debug.LogWarning($"CreatureDefinition asset not assigned to {gameObject.name}.");
            return;
        }

        TryGetComponent(out hitReceiver);
        TryGetComponent(out health);
        TryGetComponent(out itemDropper);
        
        adapterAnimator = TryGetComponent(out Animator animator) ? new FSMAdapterAnimator(animator) : new FSMAdapterAnimatorNull();
        adapterNavMeshAgent = TryGetComponent(out NavMeshAgent navMeshAgent) ? new FSMAdapterNavMeshAgent(navMeshAgent) : new FSMAdapterNavMeshAgentNull();
        adapterLogger = new FSMAdapterLogger();

        creatureContext = new(null, adapterAnimator, adapterNavMeshAgent, new FSMAdapterTransform(transform), adapterLogger, new FSMAdapterTransform(FindAnyObjectByType<MonStormCharacterController>().transform));
        stateMachine = creatureDefinition.BuildStateMachine(creatureContext, this);
    }

    void OnEnable()
    {
        if (hitReceiver != null)
        {
            hitReceiver.OnHit += HandleHit;
        }
        if (health != null)
        {
            health.OnDeath += HandleDeath;
        }
    }

    void OnDisable()
    {
        if (hitReceiver != null)
        {
            hitReceiver.OnHit -= HandleHit;
        }
        if (health != null)
        {
            health.OnDeath -= HandleDeath;
        }
    }

    void Update()
    {
        stateMachine.Tick(Time.deltaTime);

        gotHitThisFrame = false;
    }

    void HandleHit(HitApplier applier, HitDetector detector)
    {
        gotHitThisFrame = true;
        OnHit?.Invoke(applier, detector);

        if (health == null) return;

        if (health.IsDead) return; // Hit can still occur after the Health has died and the gameobject hasn't been destroyed, hence this guard

        float damageDealt = applier.Damage * detector.DamageMultiplier;
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
