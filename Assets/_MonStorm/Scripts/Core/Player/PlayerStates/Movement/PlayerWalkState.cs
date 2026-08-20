using System.Numerics;
using MonStorm.Core.StateMachine;

namespace MonStorm.Core.Player
{
    public class PlayerWalkState : PlayerBaseState
    {
        protected override void SetupTransitions(PlayerContext context)
        {
            transitionManager.Initialize(
                new StateTransition<PlayerContext>(new PlayerDamagedState(), () => context.isHit),
                new StateTransition<PlayerContext>(new PlayerIdleState(), () => context.moveInput == Vector2.Zero),
                new StateTransition<PlayerContext>(new PlayerRunState(), () => context.isSprinting),
                new StateTransition<PlayerContext>(new PlayerAttackState(), () => (context.weaponAction != ActionInput.None || context.attackPressed) && context.attackCoolDown <= 0)
            );
        }

        public override void Enter(PlayerContext context)
        {
            base.Enter(context);
            context.moveSpeed = PlayerContext.walkSpeed;
            context.AnimationIntent = context.MovementTreeHash;
            
        }

        public override void Tick(PlayerContext context, float deltaTime)
        {
            context.velocity.X = context.moveInput.X * context.moveSpeed;
            context.velocity.Z = context.moveInput.Y * context.moveSpeed;
            base.Tick(context, deltaTime);
        }

        public override void Exit(PlayerContext context)
        {
        }
    }

    
}
