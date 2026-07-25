namespace MonStorm.Core.StateMachine
{
    public class CreatureIdleState : CreatureBaseState
    {
        public CreatureIdleState(CreatureContext creatureContext, StateTransitionManager<CreatureContext> transitionManager, int animationHash) : base(creatureContext, transitionManager, animationHash)
        {

        }
    }
}
