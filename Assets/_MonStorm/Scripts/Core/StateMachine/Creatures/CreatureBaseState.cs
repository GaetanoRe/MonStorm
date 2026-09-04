using System;

namespace MonStorm.Core.StateMachine
{
    public abstract class CreatureBaseState : IState<CreatureContext>
    {
        public float StateTimer;

        protected readonly StateMachine<CreatureContext> stateMachine;
        protected readonly IFSMAdapterAnimator animator;
        protected readonly IFSMAdapterNavMeshAgent navMeshAgent;
        protected readonly IFSMAdapterTransform transform;
        protected readonly IFSMAdapterLogger logger;
        protected readonly IFSMAdapterTransform targetTransform;
        protected readonly StateTransitionManager<CreatureContext> transitionManager;
        protected readonly int animationHash;

        protected readonly Random random = new();


        public CreatureBaseState(CreatureContext creatureContext, StateTransitionManager<CreatureContext> transitionManager, int animationHash)
        {
            stateMachine = creatureContext.StateMachine;
            animator = creatureContext.AdapterAnimator;
            navMeshAgent = creatureContext.AdapterNavMeshAgent;
            transform = creatureContext.AdapterTransform;
            logger = creatureContext.AdapterLogger;
            targetTransform = creatureContext.TargetAdapterTransform;
            this.transitionManager = transitionManager;
            this.animationHash = animationHash;
        }

        public virtual void Enter(CreatureContext context)
        {
            StateTimer = 0f;
            animator.Play(animationHash);
        }

        public virtual void Exit(CreatureContext context)
        {

        }

        public virtual void Tick(CreatureContext context, float deltaTime)
        {
            StateTimer += deltaTime;

            StateTransition<CreatureContext> nextTransition = transitionManager.GetNextTransition();
            if (nextTransition == null) return;

            stateMachine.TransitionTo(nextTransition.ToState);
        }

        protected float GetRandomFloat(double min, double max) => (float)GetRandomDouble(min, max);

        protected double GetRandomDouble(double min, double max) => (random.NextDouble() * (max - min)) + min;
    }
}
