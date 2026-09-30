using MonStorm.Core.StateMachine;

namespace MonStorm.Core.Player
{
    public class PlayerSheatheState : PlayerBaseState
    {
        protected override void SetupTransitions(PlayerContext context)
        {
            transitionManager.Initialize(
                 new StateTransition<PlayerContext>(new PlayerIdleState(), () => context.sheatheTimer <= 0), //Needs to transition to a state where the player isn't wielding a weapon
                 new StateTransition<PlayerContext>(new PlayerDamagedState(), () => context.isHit)

                );

        }


        public override void Enter(PlayerContext context)
        {
            base.Enter(context);
            //Stays still when sheathing
            context.velocity.X = 0;
            context.velocity.Z = 0;

        }


        public override void Tick(PlayerContext context, float deltaTime)
        {
            context.sheatheTimer -= deltaTime;
            base.Tick(context, deltaTime);
        }


        public override void Exit(PlayerContext context)
        {
            context.isWeaponWielding = false;
            context.isWeaponSheathed = false;
            context.sheatheTimer = 0.25f; //This is so the timer returns to normal to sheathe again, no info on where the variable would be when it comes to sheathing
        }
    }
}
