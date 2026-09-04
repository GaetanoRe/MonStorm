using UnityEngine;
using MonStorm.Core.StateMachine;

[CreateAssetMenu(fileName = "TerritorialCreatureDefinition", menuName = "SO/CreatureDefinitions/Territorial")]
/// <inheritdoc/>
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
        // Territorial creatures don't update their wander center, they always return to their starting location
        CreatureChaseState chaseState = new(creatureContext, chaseTransitions, RUN_ANIM_HASH, ChaseSpeed, null);
        CreatureAttackState attackState = new(creatureContext, attackTransitions, ATTACK_ANIM_HASH, attackTimer);
        CreatureDamagedState damagedState = new(creatureContext, damagedTransitions, DAMAGED_ANIM_HASH);
        CreatureDeadState deadState = new(creatureContext, deadTransitions, DEAD_ANIM_HASH);

        bool IsInAttackRange() => creatureContext.DistanceToTarget <= AttackRange;
        bool CanStartChase() => creatureContext.IsTargetInVision && IsWithinReEngageBuffer(homeCenter, creatureContext);

        idleTransitions.Initialize(
            new(damagedState, () => creatureContext.GotHitThisFrame),
            new(deadState, () => creatureContext.IsDead),
            new(wanderState, () => idleState.StateTimer >= IdleDuration && !CanStartChase()),
            new(chaseState, CanStartChase),
            new(attackState, () => IsInAttackRange() && attackTimer.IsReady && creatureContext.IsFacingTarget)
        );

        wanderTransitions.Initialize(
            new(damagedState, () => creatureContext.GotHitThisFrame),
            new(deadState, () => creatureContext.IsDead),
            new(idleState, () => creatureContext.HasReachedDestination),
            new(chaseState, CanStartChase)
        );

        chaseTransitions.Initialize(
            new(damagedState, () => creatureContext.GotHitThisFrame),
            new(deadState, () => creatureContext.IsDead),
            new(wanderState, () => !creatureContext.IsTargetInVision || IsOutsideMaxTether(homeCenter, creatureContext)),
            new(idleState, () => IsInAttackRange() && !attackTimer.IsReady && creatureContext.IsFacingTarget),
            new(attackState, () => IsInAttackRange() && attackTimer.IsReady && creatureContext.IsFacingTarget)
        );

        attackTransitions.Initialize(
            new(damagedState, () => creatureContext.GotHitThisFrame),
            new(deadState, () => creatureContext.IsDead),
            new(wanderState, () => creatureContext.IsAnimationFinished && IsTargetOutsideMaxTether(homeCenter, creatureContext)),
            new(chaseState, () => creatureContext.IsAnimationFinished)
        );

        damagedTransitions.Initialize(
            new(damagedState, () => creatureContext.GotHitThisFrame),
            new(deadState, () => creatureContext.IsDead),
            new(chaseState, () => creatureContext.IsAnimationFinished)
        );

        stateMachine.TransitionTo(idleState);
        return stateMachine;
    }

    // Whether the creature is in range to re-engage the target again
    bool IsWithinReEngageBuffer(System.Numerics.Vector2 homeCenter, CreatureContext context)
    {
        System.Numerics.Vector2 currentPos = new(context.AdapterTransform.Position.X, context.AdapterTransform.Position.Z);
        return System.Numerics.Vector2.Distance(currentPos, homeCenter) <= ReEngageTetherRange;
    }

    // Whether the creature is outside of it's guarding range
    bool IsOutsideMaxTether(System.Numerics.Vector2 homeCenter, CreatureContext context)
    {
        System.Numerics.Vector2 currentPos = new(context.AdapterTransform.Position.X, context.AdapterTransform.Position.Z);
        return System.Numerics.Vector2.Distance(currentPos, homeCenter) > MaxTetherRange;
    }

    // Whether the target has run outside the creature's guarding range
    bool IsTargetOutsideMaxTether(System.Numerics.Vector2 homeCenter, CreatureContext context)
    {
        System.Numerics.Vector2 targetPos = new(context.TargetAdapterTransform.Position.X, context.TargetAdapterTransform.Position.Z);
        return System.Numerics.Vector2.Distance(targetPos, homeCenter) > MaxTetherRange;
    }
}
