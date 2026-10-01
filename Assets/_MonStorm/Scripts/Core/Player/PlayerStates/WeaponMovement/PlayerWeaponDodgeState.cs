using MonStorm.Core.StateMachine;
using System.Numerics;

namespace MonStorm.Core.Player
{
    public class PlayerWeaponDodgeState : PlayerDodgeState
    {
        protected override void SetupTransitions(PlayerContext context)
        {
            transitionManager.Initialize(
                new StateTransition<PlayerContext>(new PlayerWeaponIdleState(), () => context.dodgeTimer <= 0 && context.moveInput == Vector2.Zero),
                new StateTransition<PlayerContext>(new PlayerWeaponWalkState(), () => context.dodgeTimer <= 0 && context.moveInput != Vector2.Zero)



                );

        }


        //public override void Enter(PlayerContext context)
        //{
        //    base.Enter(context);
        //}


        //public override void Tick(PlayerContext context, float deltaTime)
        //{
        //    base.Tick(context, deltaTime);
        //}


        //public override void Exit(PlayerContext context)
        //{
        //}
    }
}
