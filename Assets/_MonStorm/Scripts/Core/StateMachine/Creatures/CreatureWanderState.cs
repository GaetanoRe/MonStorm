using System;
using System.Numerics;

namespace MonStorm.Core.StateMachine
{
    public class CreatureWanderState : CreatureBaseState
    {
        readonly Vector2 wanderCenter;
        readonly float maxWanderRange;


        public CreatureWanderState(CreatureContext creatureContext, StateTransitionManager<CreatureContext> transitionManager, int animationHash, Vector2 wanderCenter, float maxWanderRange)
            : base(creatureContext, transitionManager, animationHash)
        {
            this.wanderCenter = wanderCenter;
            this.maxWanderRange = maxWanderRange;
        }

        public override void Enter(CreatureContext context)
        {
            base.Enter(context);

            Wander();
        }

        public override void Exit(CreatureContext context)
        {
            base.Exit(context);

            navMeshAgent.CancelMove();
        }

        void Wander()
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 movePosition = GetRandomPointInsideCircle(maxWanderRange) + wanderCenter;

                if (navMeshAgent.MoveToWorldPosition(movePosition)) return;
            }

            logger.LogMessage("Can't find a path!");
        }

        Vector2 GetRandomPointInsideCircle(float radius)
        {
            float angle = (float)(GetRandomFloat(0, 2) * Math.PI);
            float r = (float)(Math.Sqrt(GetRandomFloat(0, 1)) * radius);

            return new(r * MathF.Cos(angle), r * MathF.Sin(angle));
        }
    }
}
