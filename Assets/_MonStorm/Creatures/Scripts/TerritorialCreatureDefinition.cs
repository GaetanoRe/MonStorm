using UnityEngine;
using MonStorm.Core.StateMachine;

[CreateAssetMenu(fileName = "TerritorialCreatureDefinition", menuName = "SO/CreatureDefinitions/Territorial")]
public class TerritorialCreatureDefinition : CreatureDefinition
{
    readonly int IDLE_ANIM_HASH = Animator.StringToHash("Idle");
    readonly int WALK_ANIM_HASH = Animator.StringToHash("Walk");
    readonly int RUN_ANIM_HASH = Animator.StringToHash("Run");
    readonly int ATTACK_ANIM_HASH = Animator.StringToHash("Attack");
    readonly int DAMAGED_ANIM_HASH = Animator.StringToHash("Damaged");
    readonly int DEAD_ANIM_HASH = Animator.StringToHash("Dead");

    [field: SerializeField] public float WalkSpeed { get; private set; }
    [field: SerializeField] public float ChaseSpeed { get; private set; }
    [field: SerializeField] public float IdleDuration { get; private set; }
    [field: SerializeField] public float MaxWanderRange { get; private set; }
    [field: SerializeField] public float DetectionRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public float AttackCooldownTime { get; private set; }

    [field: SerializeField] public float MaxTetherRange { get; private set; }
    [field: SerializeField] public float ReEngageTetherRange { get; private set; }


    public override StateMachine<CreatureContext> BuildStateMachine(CreatureContext creatureContext, CreatureBehavior creatureBehavior)
    {
        System.Numerics.Vector2 homeCenter = new(creatureBehavior.transform.position.x, creatureBehavior.transform.position.z);
        CooldownTimer attackTimer = new(AttackCooldownTime, () => Time.time);

        StateMachine<CreatureContext> stateMachine = new(creatureContext);
        creatureContext.StateMachine = stateMachine;

        StateTransitionManager<CreatureContext> idleTransitions = new();
        StateTransitionManager<CreatureContext> wanderTransitions = new();
        StateTransitionManager<CreatureContext> chaseTransitions = new();
        StateTransitionManager<CreatureContext> attackTransitions = new();
        StateTransitionManager<CreatureContext> damagedTransitions = new();
        StateTransitionManager<CreatureContext> deadTransitions = new();

        CreatureIdleState idleState = new(creatureContext, idleTransitions, IDLE_ANIM_HASH);
        CreatureWanderState wanderState = new(creatureContext, wanderTransitions, WALK_ANIM_HASH, homeCenter, MaxWanderRange, WalkSpeed);

        CreatureChaseState chaseState = new(creatureContext, chaseTransitions, RUN_ANIM_HASH, creatureContext.PlayerTransform, ChaseSpeed, null);
        CreatureAttackState attackState = new(creatureContext, attackTransitions, ATTACK_ANIM_HASH, attackTimer);
        CreatureDamagedState damagedState = new(creatureContext, damagedTransitions, DAMAGED_ANIM_HASH);
        CreatureDeadState deadState = new(creatureContext, deadTransitions, DEAD_ANIM_HASH);

        bool IsInAttackRange() => creatureContext.DistanceToPlayer <= AttackRange;
        bool CanStartChase() => creatureContext.DistanceToPlayer <= DetectionRange && IsWithinReEngageBuffer(homeCenter, creatureContext);

        idleTransitions.Initialize(
            new(damagedState, creatureBehavior.ConditionGotHitThisFrame),
            new(deadState, creatureBehavior.ConditionIsDead),
            new(wanderState, () => idleState.StateTimer >= IdleDuration && !CanStartChase()),
            new(chaseState, CanStartChase),
            new(attackState, () => IsInAttackRange() && attackTimer.IsReady && creatureContext.IsFacingPlayer)
        );

        wanderTransitions.Initialize(
            new(damagedState, creatureBehavior.ConditionGotHitThisFrame),
            new(deadState, creatureBehavior.ConditionIsDead),
            new(idleState, creatureBehavior.ConditionHasReachedDestination),
            new(chaseState, CanStartChase)
        );

        chaseTransitions.Initialize(
            new(damagedState, creatureBehavior.ConditionGotHitThisFrame),
            new(deadState, creatureBehavior.ConditionIsDead),
            new(wanderState, () => creatureContext.DistanceToPlayer > DetectionRange || IsOutsideMaxTether(homeCenter, creatureContext)),
            new(idleState, () => IsInAttackRange() && !attackTimer.IsReady && creatureContext.IsFacingPlayer),
            new(attackState, () => IsInAttackRange() && attackTimer.IsReady && creatureContext.IsFacingPlayer)
        );

        attackTransitions.Initialize(
            new(damagedState, creatureBehavior.ConditionGotHitThisFrame),
            new(deadState, creatureBehavior.ConditionIsDead),
            new(idleState, creatureBehavior.ConditionIsAnimationFinished)
        );

        damagedTransitions.Initialize(
            new(damagedState, creatureBehavior.ConditionGotHitThisFrame),
            new(deadState, creatureBehavior.ConditionIsDead),
            new(chaseState, creatureBehavior.ConditionIsAnimationFinished)
        );

        stateMachine.TransitionTo(idleState);
        return stateMachine;
    }

    private bool IsWithinReEngageBuffer(System.Numerics.Vector2 homeCenter, CreatureContext context)
    {
        System.Numerics.Vector2 currentPos = new(context.AdapterTransform.Position.X, context.AdapterTransform.Position.Z);
        return System.Numerics.Vector2.Distance(currentPos, homeCenter) <= ReEngageTetherRange;
    }

    private bool IsOutsideMaxTether(System.Numerics.Vector2 homeCenter, CreatureContext context)
    {
        System.Numerics.Vector2 currentPos = new(context.AdapterTransform.Position.X, context.AdapterTransform.Position.Z);
        return System.Numerics.Vector2.Distance(currentPos, homeCenter) > MaxTetherRange;
    }
}
