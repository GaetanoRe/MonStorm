namespace MonStorm.Core.StateMachine
{
    public class CreatureAttackState : CreatureBaseState
    {
        readonly CooldownTimer attackTimer;


        public CreatureAttackState(CreatureContext creatureContext, StateTransitionManager<CreatureContext> transitionManager, int animationHash,
            CooldownTimer attackTimer)
            : base(creatureContext, transitionManager, animationHash)
        {
            this.attackTimer = attackTimer;
        }

        public override void Enter(CreatureContext context)
        {
            base.Enter(context);

            attackTimer.StartCooldown();
        }
    }
}
