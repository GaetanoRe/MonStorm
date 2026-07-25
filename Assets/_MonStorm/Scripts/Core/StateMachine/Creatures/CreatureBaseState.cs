using System;

namespace MonStorm.Core.StateMachine
{
    public abstract class CreatureBaseState : IState<CreatureContext>
    {
        protected readonly StateMachine<CreatureContext> stateMachine;
        protected readonly IFSMAdapterAnimator animator;
        protected readonly IFSMAdapterNavMeshAgent navMeshAgent;
        protected readonly IFSMAdapterLogger logger;
        protected readonly StateTransitionManager<CreatureContext> transitionManager;
        protected readonly int animationHash;

        protected readonly Random random = new();


        public CreatureBaseState(CreatureContext creatureContext, StateTransitionManager<CreatureContext> transitionManager, int animationHash)
        {
            stateMachine = creatureContext.StateMachine;
            animator = creatureContext.AdapterAnimator;
            navMeshAgent = creatureContext.AdapterNavMeshAgent;
            logger = creatureContext.AdapterLogger;
            this.transitionManager = transitionManager;
            this.animationHash = animationHash;
        }

        public virtual void Enter(CreatureContext context)
        {
            animator.Play(animationHash);
        }

        public virtual void Exit(CreatureContext context)
        {

        }

        public virtual void Tick(CreatureContext context, float deltaTime)
        {
            StateTransition<CreatureContext> nextTransition = transitionManager.GetNextTransition();
            if (nextTransition == null) return;

            logger.LogMessage("Transitioning to " + nextTransition.ToState);
            stateMachine.TransitionTo(nextTransition.ToState);
        }

        protected float GetRandomFloat(float min, float max)
        {
            return (float)(((random.NextDouble() * (max - min)) + min) * (random.Next(2) == 0 ? 1 : -1));
        }
    }
}
