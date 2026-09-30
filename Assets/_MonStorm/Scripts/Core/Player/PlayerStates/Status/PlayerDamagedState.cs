using MonStorm.Core.StateMachine;

namespace MonStorm.Core.Player
{
    public class PlayerDamagedState : PlayerBaseState
    {
        protected override void SetupTransitions(PlayerContext context)
        {
            transitionManager.Initialize(
                new StateTransition<PlayerContext>(new PlayerIdleState(), () => context.damagedTimer <= 0 && !context.isWeaponWielding), //Needs to know if the player isn't holding a weapon to return to the suitable sheathe state
                new StateTransition<PlayerContext>(new PlayerWeaponIdleState(), () => context.damagedTimer <= 0 && context.isWeaponWielding), //Needs to know if the player is holding a weapon to return to the suitable unsheathe state
                new StateTransition<PlayerContext>(new PlayerDefeatedState(), () => context.isDead)
            );
        }

        public override void Enter(PlayerContext context)
        {
            base.Enter(context);
            context.isHit = false;
            context.damagedTimer = context.damagedDuration;
            context.velocity = context.knockbackDirection * context.knockbackForce;
            context.AnimationIntent = context.DamagedAnimHash;
        }

        public override void Tick(PlayerContext context, float deltaTime)
        {
            context.velocity.X *= 1f - (context.knockbackDeceleration * deltaTime);
            context.velocity.Z *= 1f - (context.knockbackDeceleration * deltaTime);
            context.damagedTimer -= deltaTime;
            base.Tick(context, deltaTime);
        }

        public override void Exit(PlayerContext context)
        {
        }
    }
}
