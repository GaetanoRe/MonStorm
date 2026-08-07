using System.Numerics;

namespace MonStorm.Core.StateMachine
{
    public class CreatureRunAwayState : CreatureBaseState
    {
        readonly float distance;
        readonly float runSpeed;

        float speedChange;


        public CreatureRunAwayState(CreatureContext creatureContext, StateTransitionManager<CreatureContext> transitionManager, int animationHash, float distance, float runSpeed)
            : base(creatureContext, transitionManager, animationHash)
        {
            this.distance = distance;
            this.runSpeed = runSpeed;
        }

        public override void Enter(CreatureContext context)
        {
            base.Enter(context);

            speedChange = runSpeed - navMeshAgent.AgentSpeed;
            navMeshAgent.ChangeSpeed(runSpeed);

            Vector3 relativeMove = Vector3.Normalize(transform.Position - playerTransform.Position) * distance;
            navMeshAgent.MoveRelative(relativeMove);
        }

        public override void Exit(CreatureContext context)
        {
            base.Exit(context);

            navMeshAgent.ChangeSpeed(navMeshAgent.AgentSpeed - speedChange);
            navMeshAgent.CancelMove();
        }
    }
}
