using MonStorm.Core.StateMachine;

namespace MonStorm.Core.Player
{
    public class PlayerSneakState : IState<PlayerContext>
    {
        public void Enter(PlayerContext context) { }

        public IState<PlayerContext> Tick(PlayerContext context, float deltaTime)
        {
            return this;
        }

        public void Exit(PlayerContext context) { }
    }
}
