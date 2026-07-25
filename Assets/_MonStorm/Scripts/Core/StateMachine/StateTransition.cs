using System;

namespace MonStorm.Core.StateMachine
{
    public class StateTransition<TContext> where TContext : class
    {
        public IState<TContext> ToState { get; }
        public FuncPredicate Condition { get; }


        public StateTransition(IState<TContext> toState, Func<bool> condition)
        {
            ToState = toState;
            Condition = new(condition);
        }
    }
}
