using System.Numerics;
using MonStorm.Core.StateMachine;

namespace MonStorm.Core.Player
{
    public class PlayerIdleState : PlayerBaseState
    {
        protected override void SetupTransitions(PlayerContext context)
        {
            transitionManager.Initialize(
                new StateTransition<PlayerContext>(new PlayerDamagedState(), () => context.isHit),
                new StateTransition<PlayerContext>(new PlayerDodgeState(), () => context.moveInput != Vector2.Zero && context.dodgePressed && context.dodgeCoolDown <= 0),
                new StateTransition<PlayerContext>(new PlayerRunState(), () => context.moveInput != Vector2.Zero && context.isSprinting),
                new StateTransition<PlayerContext>(new PlayerWalkState(), () => context.moveInput != Vector2.Zero),
                new StateTransition<PlayerContext>(new PlayerSneakState(), () => context.dodgePressed),
                new StateTransition<PlayerContext>(new PlayerAttackState(), () => (context.weaponAction != ActionInput.None || context.attackPressed) && context.attackCoolDown <= 0)
            );
        }

        public override void Enter(PlayerContext context)
        {
            base.Enter(context);
            context.velocity.X = 0;
            context.velocity.Z = 0;
            animator.Play(context.IdleAnimHash);
            
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
