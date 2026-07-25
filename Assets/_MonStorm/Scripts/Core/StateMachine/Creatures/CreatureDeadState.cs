namespace MonStorm.Core.StateMachine
{
    public class CreatureDeadState : CreatureBaseState
    {
        public CreatureDeadState(CreatureContext creatureContext, StateTransitionManager<CreatureContext> transitionManager, int animationHash) : base(creatureContext, transitionManager, animationHash)
        {

        }
    }
}
