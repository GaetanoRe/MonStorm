using UnityEngine;
using UnityEngine.AI;
using MonStorm.Adapters;
using MonStorm.Core.StateMachine;

public class DragonBehavior : MonoBehaviour, IHitDetectionManager
{
    [Header("Stats")]
    [SerializeField, Range(3f, 20f)] float chaseDetectionRange = 10f;

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
    FSMAdapterTransform adapterTarget;
    StateMachine<CreatureContext> stateMachine;
    CreatureContext creatureContext;

    CreatureIdleState idleState;
    CreatureChaseState chaseState;
    CreatureAttackState attackState;
    CreatureDamagedState damagedState;
    CreatureDeadState deadState;

    readonly static int idleHash = Animator.StringToHash("Idle");
    readonly static int chaseHash = Animator.StringToHash("Walk");
    readonly static int attackHash = Animator.StringToHash("Attack");
    readonly static int damagedHash = Animator.StringToHash("Damaged");
    readonly static int deadHash = Animator.StringToHash("Dead");

    bool gotDamaged;

    readonly float attackRange = 6f;
    Transform target;

    float attackRechargeTime = 5f;
    float attackTimer;


    void Awake()
    {
        health = GetComponent<Health>();
        itemDropper = GetComponent<ItemDropper>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        adapterAnimator = new(animator);
        adapterNavMeshAgent = new(navMeshAgent);
        adapterLogger = new(logStateTransitions);

        target = FindFirstObjectByType<MonStormCharacterController>().transform;
        adapterTarget = new(target);

        creatureContext = new(null, adapterAnimator, adapterNavMeshAgent, adapterLogger);
        stateMachine = new(creatureContext);
        creatureContext.StateMachine = stateMachine;
        StateTransitionManager<CreatureContext> idleTransitions = new();
        StateTransitionManager<CreatureContext> chaseTransitions = new();
        StateTransitionManager<CreatureContext> attackTransitions = new();
        StateTransitionManager<CreatureContext> damagedTransitions = new();
        StateTransitionManager<CreatureContext> deadTransitions = new();

        idleState = new(creatureContext, idleTransitions, idleHash);
        chaseState = new(creatureContext, chaseTransitions, chaseHash, adapterTarget);
        attackState = new(creatureContext, attackTransitions, attackHash);
        damagedState = new(creatureContext, damagedTransitions, damagedHash);
        deadState = new(creatureContext, deadTransitions, deadHash);

        StateTransition<CreatureContext> idleToChaseTransition = new(chaseState, ConditionTargetInChaseRange);
        StateTransition<CreatureContext> idleToAttackTransition = new(attackState, ConditionAttackPossible);
        StateTransition<CreatureContext> chaseToIdleTransition = new(idleState, ConditionTargetNotInChaseRange);
        StateTransition<CreatureContext> toIdleTransition = new(idleState, ConditionAnimationFinished);
        StateTransition<CreatureContext> toDamagedTransition = new(damagedState, ConditionGotHit);
        StateTransition<CreatureContext> toDeadTransition = new(deadState, ConditionDead);

        idleTransitions.Initialize(toDamagedTransition, toDeadTransition, idleToChaseTransition, idleToAttackTransition);
        chaseTransitions.Initialize(toDamagedTransition, toDeadTransition, chaseToIdleTransition);
        attackTransitions.Initialize(toDamagedTransition, toDeadTransition, toIdleTransition);
        damagedTransitions.Initialize(toDamagedTransition, toDeadTransition, toIdleTransition);

        stateMachine.TransitionTo(idleState);
    }

    void Update()
    {
        stateMachine.Tick(Time.deltaTime);

        attackTimer += Time.deltaTime;
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
        itemDropper.Activate(2f);
    }

    bool ConditionGotHit()
    {
        if (!gotDamaged) return false;

        gotDamaged = false;
        return true;
    }

    bool ConditionDead() => health.IsDead;

    bool ConditionAnimationFinished() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !animator.IsInTransition(0);

    bool ConditionTargetInChaseRange()
    {
        if (adapterTarget == null) return false;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        return distanceToTarget <= chaseDetectionRange && distanceToTarget >= attackRange;
    }

    bool ConditionTargetNotInChaseRange() => !ConditionTargetInChaseRange();

    bool ConditionAttackPossible()
    {
        if (adapterTarget == null) return false;

        if (attackTimer >= attackRechargeTime && Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            attackTimer = 0f;
            return true;
        }

        return false;
    }
}
