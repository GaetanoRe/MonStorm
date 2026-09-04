using System;
using System.Numerics;

namespace MonStorm.Core.StateMachine
{
    public class CreatureChaseState : CreatureBaseState
    {
        readonly Action<Vector2> onExitChase;

        readonly static float recalculatePathTime = 0.1f;
        float pathTimer;

        readonly float chaseSpeed;
        float speedChange;


        public CreatureChaseState(CreatureContext creatureContext, StateTransitionManager<CreatureContext> transitionManager, int animationHash, float chaseSpeed,
            Action<Vector2> onExitChase)
            : base(creatureContext, transitionManager, animationHash)
        {
            this.chaseSpeed = chaseSpeed;
            this.onExitChase = onExitChase;
        }

        public override void Enter(CreatureContext context)
        {
            base.Enter(context);

            pathTimer = recalculatePathTime;
            speedChange = chaseSpeed - navMeshAgent.AgentSpeed;
            navMeshAgent.ChangeSpeed(chaseSpeed);
        }

        public override void Exit(CreatureContext context)
        {
            base.Exit(context);

            navMeshAgent.ChangeSpeed(navMeshAgent.AgentSpeed - speedChange);
            navMeshAgent.CancelMove();
            onExitChase?.Invoke(transform.PositionV2);
        }

        public override void Tick(CreatureContext context, float deltaTime)
        {
            base.Tick(context, deltaTime);

            pathTimer += deltaTime;

            if (pathTimer >= recalculatePathTime)
            {
                pathTimer = 0f;
                navMeshAgent.FollowTarget(targetTransform);
            }
        }
    }
}
