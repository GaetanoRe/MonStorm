namespace MonStorm.Core.StateMachine
{
    public class CreatureAttackState : CreatureBaseState
    {
        public CreatureAttackState(CreatureContext creatureContext, StateTransitionManager<CreatureContext> transitionManager, int animationHash) : base(creatureContext, transitionManager, animationHash)
        {

        }
    }
}
