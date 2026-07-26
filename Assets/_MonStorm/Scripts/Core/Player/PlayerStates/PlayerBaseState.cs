using MonStorm.Core.StateMachine;

namespace MonStorm.Core.Player
{
    public abstract class PlayerBaseState : IState<PlayerContext>
    {
        protected StateTransitionManager<PlayerContext> transitionManager;
        protected StateMachine<PlayerContext> stateMachine;

        protected IFSMAdapterAnimator animator;

        public virtual void Enter(PlayerContext context)
        {
            stateMachine = context.StateMachine;
            animator = context.AdapterAnimator;
            transitionManager = new StateTransitionManager<PlayerContext>();
            SetupTransitions(context);
        }

        public virtual void Tick(PlayerContext context, float deltaTime)
        {
            HandleCoolDowns(context, deltaTime);
            StateTransition<PlayerContext> next = transitionManager.GetNextTransition();
            if (next == null) return;
            stateMachine.TransitionTo(next.ToState);
        }

        public virtual void Exit(PlayerContext context) { }

        protected abstract void SetupTransitions(PlayerContext context);


        private void HandleCoolDowns(PlayerContext context, float deltaTime)
        {
            if(context.attackCoolDown > 0) context.attackCoolDown -= deltaTime;
        }
    }
}
