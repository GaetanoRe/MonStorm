using System.Collections.Generic;

namespace MonStorm.Core.StateMachine
{
    public class StateTransitionManager<TContext> where TContext : class
    {
        public List<StateTransition<TContext>> TransitionList = new();


        public void Initialize(params StateTransition<TContext>[] transitions) => TransitionList.AddRange(transitions);

        public StateTransition<TContext> GetNextTransition()
        {
            foreach (StateTransition<TContext> transition in TransitionList)
            {
                if (transition.Condition.Evaluate()) return transition;
            }

            return null;
        }
    }
}
