using UnityEngine;
using MonStorm.Core.StateMachine;

[CreateAssetMenu(fileName = "SkittishCreatureDefinition", menuName = "SO/CreatureDefinitions/Skittish")]
public class SkittishCreatureDefinition : CreatureDefinition
{
    public static readonly int IDLE_ANIM_HASH = Animator.StringToHash("Idle");
    public static readonly int WALK_ANIM_HASH = Animator.StringToHash("Walk");
    public static readonly int RUN_ANIM_HASH = Animator.StringToHash("Run");
    public static readonly int DAMAGED_ANIM_HASH = Animator.StringToHash("Damaged");
    public static readonly int DEAD_ANIM_HASH = Animator.StringToHash("Dead");

    [field: SerializeField] public float WalkSpeed { get; private set; }
    [field: SerializeField] public float RunSpeed { get; private set; }
    [field: SerializeField] public float IdleDuration { get; private set; }
    [field: SerializeField] public float MaxWanderRange { get; private set; }
    [field: SerializeField] public float RunAwayDistance { get; private set; }


    public override StateMachine<CreatureContext> BuildStateMachine(CreatureContext creatureContext, CreatureBehavior creatureBehavior)
    {
        System.Numerics.Vector2 wanderCenter = new(creatureBehavior.transform.position.x, creatureBehavior.transform.position.z);

        StateMachine<CreatureContext> stateMachine = new(creatureContext);
        creatureContext.StateMachine = stateMachine;

        StateTransitionManager<CreatureContext> idleTransitions = new();
        StateTransitionManager<CreatureContext> wanderTransitions = new();
        StateTransitionManager<CreatureContext> runTransitions = new();
        StateTransitionManager<CreatureContext> damagedTransitions = new();
        StateTransitionManager<CreatureContext> deadTransitions = new();

        CreatureIdleState idleState = new(creatureContext, idleTransitions, IDLE_ANIM_HASH);
        CreatureWanderState wanderState = new(creatureContext, wanderTransitions, WALK_ANIM_HASH, wanderCenter, MaxWanderRange);
        CreatureRunAwayState runAwayState = new(creatureContext, runTransitions, RUN_ANIM_HASH, RunAwayDistance, RunSpeed);
        CreatureDamagedState damagedState = new(creatureContext, damagedTransitions, DAMAGED_ANIM_HASH);
        CreatureDeadState deadState = new(creatureContext, deadTransitions, DEAD_ANIM_HASH);

        idleTransitions.Initialize(
            new StateTransition<CreatureContext>(damagedState, creatureBehavior.ConditionGotHitThisFrame),
            new StateTransition<CreatureContext>(deadState, creatureBehavior.ConditionIsDead),
            new StateTransition<CreatureContext>(wanderState, () => idleState.StateTimer >= IdleDuration)
            );

        wanderTransitions.Initialize(
            new StateTransition<CreatureContext>(damagedState, creatureBehavior.ConditionGotHitThisFrame),
            new StateTransition<CreatureContext>(deadState, creatureBehavior.ConditionIsDead),
            new StateTransition<CreatureContext>(idleState, creatureBehavior.ConditionHasReachedDestionation)
            );

        runTransitions.Initialize(
            new StateTransition<CreatureContext>(damagedState, creatureBehavior.ConditionGotHitThisFrame),
            new StateTransition<CreatureContext>(deadState, creatureBehavior.ConditionIsDead),
            new StateTransition<CreatureContext>(idleState, creatureBehavior.ConditionHasReachedDestionation)
            );

        damagedTransitions.Initialize(
            new StateTransition<CreatureContext>(damagedState, creatureBehavior.ConditionGotHitThisFrame),
            new StateTransition<CreatureContext>(deadState, creatureBehavior.ConditionIsDead),
            new StateTransition<CreatureContext>(runAwayState, creatureBehavior.ConditionIsAnimationFinished)
            );

        stateMachine.TransitionTo(idleState);
        return stateMachine;
    }
}
