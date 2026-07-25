using UnityEngine;
using MonStorm.Core.StateMachine;

namespace MonStorm.Core.Player
{
    public class PlayerRunState : IState<PlayerContext>
    {
        public void Enter(PlayerContext context) { }

        public IState<PlayerContext> Tick(PlayerContext context, float deltaTime)
        {
            if (context.isHit)
                return new PlayerDamagedState();
            if (context.moveInput == Vector2.zero)
                return new PlayerIdleState();
            if (!context.isSprinting)
                return new PlayerWalkState();
            return this;
        }

        public void Exit(PlayerContext context) { }
    }
}
