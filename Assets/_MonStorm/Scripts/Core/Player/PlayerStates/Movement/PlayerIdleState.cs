using UnityEngine;
using MonStorm.Core.StateMachine;

namespace MonStorm.Core.Player
{
    public class PlayerIdleState : IState<PlayerContext>
    {
        public void Enter(PlayerContext context) { }

        public IState<PlayerContext> Tick(PlayerContext context, float deltaTime)
        {
            if (context.isHit)
                return new PlayerDamagedState();

            if (context.moveInput != Vector2.zero)
            {
                if (context.dodgePressed && context.dodgeCoolDown <= 0)
                    return new PlayerDodgeState();
                if (context.isSprinting)
                    return new PlayerRunState();
                return new PlayerWalkState();
            }
            else if (context.dodgePressed)
            {
                return new PlayerSneakState();
            }

            return this;
        }

        public void Exit(PlayerContext context) { }
    }
}
