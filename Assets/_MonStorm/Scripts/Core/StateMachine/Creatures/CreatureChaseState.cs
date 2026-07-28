namespace MonStorm.Core.StateMachine
{
    public class CreatureChaseState : CreatureBaseState
    {
        readonly IFSMAdapterTransform targetTransform;

        readonly static float recalculatePathTime = 0.5f;
        float pathTimer = recalculatePathTime;


        public CreatureChaseState(CreatureContext creatureContext, StateTransitionManager<CreatureContext> transitionManager, int animationHash,
            IFSMAdapterTransform targetTransform)
            : base(creatureContext, transitionManager, animationHash)
        {
            this.targetTransform = targetTransform;
        }

        public override void Exit(CreatureContext context)
        {
            base.Exit(context);

            navMeshAgent.CancelMove();
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
