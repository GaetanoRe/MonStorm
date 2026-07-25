namespace MonStorm.Core.StateMachine
{
    public class CreatureWanderState : CreatureBaseState
    {
        readonly float wanderRadiusMin;
        readonly float wanderRadiusMax;
        readonly float moveSpeedMultiplier;

        float previousSpeed;


        public CreatureWanderState(CreatureContext creatureContext, StateTransitionManager<CreatureContext> transitionManager, int animationHash, float wanderRadiusMin, float wanderRadiusMax, float moveSpeedMultiplier)
            : base(creatureContext, transitionManager, animationHash)
        {
            this.wanderRadiusMin = wanderRadiusMin;
            this.wanderRadiusMax = wanderRadiusMax;
            this.moveSpeedMultiplier = moveSpeedMultiplier;
        }

        public override void Enter(CreatureContext context)
        {
            base.Enter(context);

            previousSpeed = navMeshAgent.AgentSpeed;
            navMeshAgent.ChangeSpeed(navMeshAgent.AgentSpeed * moveSpeedMultiplier);

            float x = GetRandomFloat(wanderRadiusMin, wanderRadiusMax);
            float z = GetRandomFloat(wanderRadiusMin, wanderRadiusMax);

            for (int i = 0; i <= 10; i++)
            {
                if (navMeshAgent.MoveRelative(x, 0f, z)) break;

                x = GetRandomFloat(wanderRadiusMin, wanderRadiusMax);
                z = GetRandomFloat(wanderRadiusMin, wanderRadiusMax);

                if (i == 10) logger.LogMessage("Can't find a path!");
            }
        }

        public override void Exit(CreatureContext context)
        {
            base.Exit(context);

            navMeshAgent.ChangeSpeed(previousSpeed);
            navMeshAgent.CancelMove();
        }
    }
}
