using MonStorm.Core.StateMachine;

namespace MonStorm.Core.Player
{
    public class PlayerAttackState : PlayerBaseState
    {
        protected override void SetupTransitions(PlayerContext context)
        {
            transitionManager.Initialize(
                new StateTransition<PlayerContext>(new PlayerDamagedState(), () => context.isHit),
                new StateTransition<PlayerContext>(new PlayerIdleState(), () => context.attackTimer <= 0)
            );
        }

        public override void Enter(PlayerContext context)
        {
            base.Enter(context);
            context.velocity.X = 0;
            context.velocity.Z = 0;
            context.attackTimer = 1.0f;
            animator.Play(context.AttackAnimHash);
        }

        public override void Tick(PlayerContext context, float deltaTime)
        {
            context.attackTimer -= deltaTime;
            base.Tick(context, deltaTime);
        }

        public override void Exit(PlayerContext context)
        {
            context.AdapterHitApplier.SetActive(false);
            context.attackCoolDown = 0.5f;   
        }
    }
}
