using System.Numerics;
using MonStorm.Core.StateMachine;

namespace MonStorm.Core.Player
{
    public class PlayerDodgeState : PlayerBaseState
    {
        protected override void SetupTransitions(PlayerContext context)
        {
            transitionManager.Initialize(
                new StateTransition<PlayerContext>(new PlayerIdleState(), () => context.moveInput == Vector2.Zero),
                new StateTransition<PlayerContext>(new PlayerWalkState(), () => context.moveInput != Vector2.Zero)
            );
        }

        public override void Enter(PlayerContext context)
        {
            base.Enter(context);
        }

        public override void Tick(PlayerContext context, float deltaTime)
        {
            base.Tick(context, deltaTime);
        }

        public override void Exit(PlayerContext context)
        {
        }
    }
}
