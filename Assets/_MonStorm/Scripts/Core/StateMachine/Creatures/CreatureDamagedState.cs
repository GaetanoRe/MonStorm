namespace MonStorm.Core.StateMachine
{
    public class CreatureDamagedState : CreatureBaseState
    {
        public CreatureDamagedState(CreatureContext creatureContext, StateTransitionManager<CreatureContext> transitionManager, int animationHash) : base(creatureContext, transitionManager, animationHash)
        {

        }
    }
}
