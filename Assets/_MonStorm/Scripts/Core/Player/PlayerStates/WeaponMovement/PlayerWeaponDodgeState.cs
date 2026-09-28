using MonStorm.Core.StateMachine;

namespace MonStorm.Core.Player
{
    public class PlayerWeaponDodgeState : PlayerBaseState
    {
        protected override void SetupTransitions(PlayerContext context)
        {
            transitionManager.Initialize(
                 new StateTransition<PlayerContext>(new PlayerDamagedState(), () => context.isHit) //Just putting this here in order to prevent null errors, this will include PlayerWeaponIdleState




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
