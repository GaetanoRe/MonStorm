using UnityEngine;
using MonStorm.Core.StateMachine;

[CreateAssetMenu(fileName = "SkittishCreatureDefinition", menuName = "SO/CreatureDefinitions/Skittish")]
/// <inheritdoc/>
public class SkittishCreatureDefinition : CreatureDefinition
{
    readonly int IDLE_ANIM_HASH = Animator.StringToHash("Idle");
    readonly int WALK_ANIM_HASH = Animator.StringToHash("Walk");
    readonly int RUN_ANIM_HASH = Animator.StringToHash("Run");
    readonly int DAMAGED_ANIM_HASH = Animator.StringToHash("Damaged");
    readonly int DEAD_ANIM_HASH = Animator.StringToHash("Dead");

    /// <summary>Normal walking speed.</summary>
    [field: SerializeField] public float WalkSpeed { get; private set; }
    /// <summary>Run away speed.</summary>
    [field: SerializeField] public float RunSpeed { get; private set; }
    /// <summary>How long the creature stays idle for after wandering, and then going back to wander again.</summary>
    [field: SerializeField] public float IdleDuration { get; private set; }
    /// <summary>The maximum range the creature can wander away from it's starting position.</summary>
    [field: SerializeField] public float MaxWanderRange { get; private set; }
    /// <summary>How far the creature runs away after being scared.</summary>
    [field: SerializeField] public float RunAwayDistance { get; private set; }


    public override StateMachine<CreatureContext> BuildStateMachine(CreatureContext creatureContext, CreatureBehavior creatureBehavior)
    {
        System.Numerics.Vector2 wanderCenter = new(creatureBehavior.transform.position.x, creatureBehavior.transform.position.z);

        StateMachine<CreatureContext> stateMachine = new(creatureContext);
        creatureContext.StateMachine = stateMachine;

        StateTransitionManager<CreatureContext> idleTransitions = new();
        StateTransitionManager<CreatureContext> wanderTransitions = new();
        StateTransitionManager<CreatureContext> runAwayTransitions = new();
        StateTransitionManager<CreatureContext> damagedTransitions = new();
        StateTransitionManager<CreatureContext> deadTransitions = new();

        CreatureIdleState idleState = new(creatureContext, idleTransitions, IDLE_ANIM_HASH);
        CreatureWanderState wanderState = new(creatureContext, wanderTransitions, WALK_ANIM_HASH, wanderCenter, MaxWanderRange, WalkSpeed);
        CreatureRunAwayState runAwayState = new(creatureContext, runAwayTransitions, RUN_ANIM_HASH, RunAwayDistance, RunSpeed);
        CreatureDamagedState damagedState = new(creatureContext, damagedTransitions, DAMAGED_ANIM_HASH);
        CreatureDeadState deadState = new(creatureContext, deadTransitions, DEAD_ANIM_HASH);

        idleTransitions.Initialize(
            new(damagedState, () => creatureContext.GotStaggeredThisFrame),
            new(deadState, () => creatureContext.IsDead),
            new(wanderState, () => idleState.StateTimer >= IdleDuration),
            new(runAwayState, () => creatureContext.IsTargetInVision)
            );

        wanderTransitions.Initialize(
            new(damagedState, () => creatureContext.GotStaggeredThisFrame),
            new(deadState, () => creatureContext.IsDead),
            new(idleState, () => creatureContext.HasReachedDestination),
            new(runAwayState, () => creatureContext.IsTargetInVision)
            );

        runAwayTransitions.Initialize(
            new(damagedState, () => creatureContext.GotStaggeredThisFrame),
            new(deadState, () => creatureContext.IsDead),
            new(idleState, () => creatureContext.HasReachedDestination)
            );

        damagedTransitions.Initialize(
            new(damagedState, () => creatureContext.GotStaggeredThisFrame),
            new(deadState, () => creatureContext.IsDead),
            new(runAwayState, () => creatureContext.IsAnimationFinished)
            );

        stateMachine.TransitionTo(idleState);
        return stateMachine;
    }
}
