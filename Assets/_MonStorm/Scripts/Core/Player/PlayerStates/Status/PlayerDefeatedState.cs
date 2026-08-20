namespace MonStorm.Core.Player
{
    public class PlayerDefeatedState : PlayerBaseState
    {
        protected override void SetupTransitions(PlayerContext context) { }
        public override void Enter(PlayerContext context)
        {
            base.Enter(context);
            context.AnimationIntent = context.DeathAnimHash;
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
