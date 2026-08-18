using System;

namespace MonStorm.Core.StateMachine
{
    public class StateMachine<TContext> where TContext : class
    {
        public event Action<IState<TContext>, IState<TContext>> OnStateChanged;

        TContext context;
        IState<TContext> currentState;
        IState<TContext> previousState;


        public StateMachine(TContext context)
        {
            this.context = context;
        }

        public void TransitionTo(IState<TContext> newState)
        {  
            currentState?.Exit(context);

            previousState = currentState;

            currentState = newState;

            currentState?.Enter(context);

            OnStateChanged?.Invoke(previousState, newState);
        }

        public void Tick(float deltaTime)
        {
            currentState?.Tick(context, deltaTime);
        }
    }
}

