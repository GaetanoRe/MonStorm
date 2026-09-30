using MonStorm.Core.StateMachine;

namespace MonStorm.Core.Player
{
    public class PlayerUnsheatheState : PlayerBaseState
    {
        protected override void SetupTransitions(PlayerContext context)
		{
			transitionManager.Initialize(
				 new StateTransition<PlayerContext>(new PlayerWeaponIdleState(), () => context.unsheatheTimer <= 0), //Needs to transition to an idle state once unsheathed
                 new StateTransition<PlayerContext>(new PlayerDamagedState(), () => context.isHit) //Can get interupted when taking damage

                );

        }


		public override void Enter(PlayerContext context)
		{
			base.Enter(context);
            //Stays still when unsheathing
            context.velocity.X = 0;
            context.velocity.Z = 0;
        }


		public override void Tick(PlayerContext context, float deltaTime)
		{
            context.unsheatheTimer -= deltaTime;
            base.Tick(context, deltaTime);
		}


		public override void Exit(PlayerContext context)
		{
            context.isWeaponWielding = true;
            context.isWeaponSheathed = false;
            context.sheatheTimer = 0.75f; //This is so the timer returns to normal to sheathe again, no info on where the variable would be when it comes to sheathing
        }
    }
}
