using MonStorm.Core.StateMachine;
using System.Numerics;

namespace MonStorm.Core.Player
{
    public class PlayerWeaponIdleState : PlayerIdleState
    {
        protected override void SetupTransitions(PlayerContext context)
        {
            transitionManager.Initialize(
                 new StateTransition<PlayerContext>(new PlayerDamagedState(), () => context.isHit), //Just putting this here in order to prevent null errors, this will include PlayerWeaponIdleState
                 new StateTransition<PlayerContext>(new PlayerWeaponDodgeState(), () => context.moveInput != Vector2.Zero && context.dodgePressed && context.dodgeCoolDown <= 0),
                 new StateTransition<PlayerContext>(new PlayerWeaponWalkState(), () => context.moveInput != Vector2.Zero),
                 new StateTransition<PlayerContext>(new PlayerSheatheState(), () => (context.moveInput != Vector2.Zero && context.isSprinting) || context.isWeaponSheathed), //Needs to identify if the player wants to sprint, which therefore is determined if the player wants to sheathe
                 new StateTransition<PlayerContext>(new PlayerAttackState(), () => (context.weaponAction != ActionInput.None || context.attackPressed) && context.attackCoolDown <= 0)
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
