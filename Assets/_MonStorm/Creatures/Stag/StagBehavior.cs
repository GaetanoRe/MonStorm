using UnityEngine;
using UnityEngine.AI;
using MonStorm.Adapters;
using MonStorm.Core.StateMachine;

public class StagBehavior : MonoBehaviour, IHitDetectionManager
{
    [Header("Debug")]
    [SerializeField] bool logStateTransitions;
    [SerializeField] bool logDamageTaken;

    Health health;
    ItemDropper itemDropper;
    NavMeshAgent navMeshAgent;
    Animator animator;

    FSMAdapterAnimator adapterAnimator;
    FSMAdapterNavMeshAgent adapterNavMeshAgent;
    FSMAdapterLogger adapterLogger;
    StateMachine<CreatureContext> stateMachine;
    CreatureContext creatureContext;

    CreatureIdleState idleState;
    CreatureWanderState wanderState;
    CreatureWanderState runState;
    CreatureDamagedState damagedState;
    CreatureDeadState deadState;

    readonly static int idleHash = Animator.StringToHash("Idle");
    readonly static int wanderHash = Animator.StringToHash("Walk");
    readonly static int runHash = Animator.StringToHash("Run");
    readonly static int damagedHash = Animator.StringToHash("Damaged");
    readonly static int deadHash = Animator.StringToHash("Dead");

    readonly float idleWaitTime = 5f;
    float idleTimer;

    bool gotDamaged;

    readonly float wanderRadiusMin = 3f;
    readonly float wanderRadiusMax = 6f;

    readonly float runRadiusMin = 10f;
    readonly float runRadiusMax = 12f;
    readonly float runSpeedMultiplier = 2f;


    void Awake()
    {
        health = GetComponent<Health>();
        itemDropper = GetComponent<ItemDropper>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        adapterAnimator = new(animator);
        adapterNavMeshAgent = new(navMeshAgent);
        adapterLogger = new(logStateTransitions);
        
        creatureContext = new(null, adapterAnimator, adapterNavMeshAgent, adapterLogger);
        stateMachine = new(creatureContext);
        creatureContext.StateMachine = stateMachine;
        StateTransitionManager<CreatureContext> idleTransitions = new();
        StateTransitionManager<CreatureContext> wanderTransitions = new();
        StateTransitionManager<CreatureContext> runTransitions = new();
        StateTransitionManager<CreatureContext> damagedTransitions = new();
        StateTransitionManager<CreatureContext> deadTransitions = new();

        idleState = new(creatureContext, idleTransitions, idleHash);
        wanderState = new(creatureContext, wanderTransitions, wanderHash, wanderRadiusMin, wanderRadiusMax, 1f);
        runState = new(creatureContext, runTransitions, runHash, runRadiusMin, runRadiusMax, runSpeedMultiplier);
        damagedState = new(creatureContext, damagedTransitions, damagedHash);
        deadState = new(creatureContext, deadTransitions, deadHash);

        StateTransition<CreatureContext> wanderToIdleTransition = new(idleState, ConditionHasReachedDestination);
        StateTransition<CreatureContext> runToIdleTransition = new(idleState, ConditionHasReachedDestination);
        StateTransition<CreatureContext> toWanderTransition = new(wanderState, ConditionIdleWait);
        StateTransition<CreatureContext> damagedToRunTransition = new(runState, ConditionAnimationFinished);
        StateTransition<CreatureContext> toDamagedTransition = new(damagedState, ConditionGotHit);
        StateTransition<CreatureContext> toDeadTransition = new(deadState, ConditionDead);

        idleTransitions.Initialize(toDamagedTransition, toDeadTransition, toWanderTransition);
        wanderTransitions.Initialize(toDamagedTransition, toDeadTransition, wanderToIdleTransition);
        runTransitions.Initialize(toDamagedTransition, toDeadTransition, runToIdleTransition);
        damagedTransitions.Initialize(toDamagedTransition, toDeadTransition, damagedToRunTransition);

        stateMachine.TransitionTo(idleState);
    }

    void Update()
    {
        stateMachine.Tick(Time.deltaTime);
    }

    void OnEnable()
    {
        health.OnDeath += DropItems;
    }

    void OnDisable()
    {
        health.OnDeath -= DropItems;
    }

    public void HandleHit(HitApplier applier, HitDetector detector)
    {
        if (health.IsDead) return;

        float damageDealt = applier.Damage * detector.DamageMultiplier;
        health.Damage(damageDealt);
        gotDamaged = true;

        if (logDamageTaken)
        {
            Debug.Log($"Hit: {detector.gameObject.name} (Transform sibling index: {detector.transform.GetSiblingIndex()}), damage dealt: {damageDealt}, remaining health: {health.CurrentHealth}");
        }
    }

    void DropItems()
    {
        itemDropper.Activate(1f);
    }

    bool ConditionIdleWait()
    {
        if (idleTimer < idleWaitTime)
        {
            idleTimer += Time.deltaTime;
            return false;
        }

        idleTimer = 0f;
        return true;
    }

    bool ConditionGotHit()
    {
        if (!gotDamaged) return false;

        gotDamaged = false;
        return true;
    }

    bool ConditionDead() => health.IsDead;

    bool ConditionAnimationFinished() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !animator.IsInTransition(0);

    bool ConditionHasReachedDestination()
    {
        if (adapterNavMeshAgent.HasActivePath()) return false;

        idleTimer = 0f;
        return true;
    }
}
