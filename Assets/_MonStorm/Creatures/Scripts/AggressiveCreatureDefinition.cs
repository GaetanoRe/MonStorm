using UnityEngine;
using MonStorm.Core.StateMachine;

[CreateAssetMenu(fileName = "AggressiveCreatureDefinition", menuName = "SO/CreatureDefinitions/Aggressive")]
/// <inheritdoc/>
public class AggressiveCreatureDefinition : CreatureDefinition
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


    public override StateMachine<CreatureContext> BuildStateMachine(CreatureContext creatureContext, CreatureBehavior creatureBehavior)
    {
        System.Numerics.Vector2 wanderCenter = new(creatureBehavior.transform.position.x, creatureBehavior.transform.position.z);
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
        CreatureWanderState wanderState = new(creatureContext, wanderTransitions, WALK_ANIM_HASH, wanderCenter, MaxWanderRange, WalkSpeed);
        // Aggressive creatures update their wander center to wherever the chase ended
        CreatureChaseState chaseState = new(creatureContext, chaseTransitions, RUN_ANIM_HASH, ChaseSpeed, wanderState.ChangeWanderCenter);
        CreatureAttackState attackState = new(creatureContext, attackTransitions, ATTACK_ANIM_HASH, attackTimer);
        CreatureDamagedState damagedState = new(creatureContext, damagedTransitions, DAMAGED_ANIM_HASH);
        CreatureDeadState deadState = new(creatureContext, deadTransitions, DEAD_ANIM_HASH);

        bool IsInAttackRange() => creatureContext.DistanceToTarget <= AttackRange;

        idleTransitions.Initialize(
            new(damagedState, () => creatureContext.GotStaggeredThisFrame),
            new(deadState, () => creatureContext.IsDead),
            new(wanderState, () => idleState.StateTimer >= IdleDuration && !creatureContext.IsTargetInVision),
            new(chaseState, () => creatureContext.IsTargetInVision && (creatureContext.DistanceToTarget > AttackRange || !creatureContext.IsFacingTarget)),
            new(attackState, () => IsInAttackRange() && attackTimer.IsReady && creatureContext.IsFacingTarget)
        );

        wanderTransitions.Initialize(
            new(damagedState, () => creatureContext.GotStaggeredThisFrame),
            new(deadState, () => creatureContext.IsDead),
            new(idleState, () => creatureContext.HasReachedDestination),
            new(chaseState, () => creatureContext.IsTargetInVision && creatureContext.DistanceToTarget > AttackRange)
        );

        chaseTransitions.Initialize(
            new(damagedState, () => creatureContext.GotStaggeredThisFrame),
            new(deadState, () => creatureContext.IsDead),
            new(idleState, () => IsInAttackRange() && !attackTimer.IsReady && creatureContext.IsFacingTarget),
            new(attackState, () => IsInAttackRange() && attackTimer.IsReady && creatureContext.IsFacingTarget)
        );

        attackTransitions.Initialize(
            new(damagedState, () => creatureContext.GotStaggeredThisFrame),
            new(deadState, () => creatureContext.IsDead),
            new(idleState, () => creatureContext.IsAnimationFinished)
        );

        damagedTransitions.Initialize(
            new(damagedState, () => creatureContext.GotStaggeredThisFrame),
            new(deadState, () => creatureContext.IsDead),
            new(chaseState, () => creatureContext.IsAnimationFinished)
        );

        stateMachine.TransitionTo(idleState);
        return stateMachine;
    }
}
