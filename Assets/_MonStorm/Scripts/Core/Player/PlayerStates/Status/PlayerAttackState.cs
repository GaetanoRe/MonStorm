using MonStorm.Core.Combat;
using MonStorm.Core.StateMachine;

namespace MonStorm.Core.Player
{
    public class PlayerAttackState : PlayerBaseState
    {
        private AttackMove _currentMove;
        protected override void SetupTransitions(PlayerContext context)
        {
            transitionManager.Initialize(
                new StateTransition<PlayerContext>(new PlayerDamagedState(), () => context.isHit),
                new StateTransition<PlayerContext>(new PlayerIdleState(), () => context.attackTimer <= 0),
                new StateTransition<PlayerContext>(new PlayerAttackState(), () => context.chainInput != ActionInput.None)
            );
        }

        public override void Enter(PlayerContext context)
        {
            base.Enter(context);
            context.chainInput = ActionInput.None;
            string moveId;
            if(context.weaponAction != ActionInput.None) {
                moveId = context.equippedWeapon.DirectionalOpenerMap[context.weaponAction];
            }
            else
            {
                moveId = context.equippedWeapon.moves[0].Id;
            }
            _currentMove = context.equippedWeapon.GetMove(moveId);
            context.velocity.X = 0;
            context.velocity.Z = 0;
            context.attackTimer = _currentMove.Duration;
            context.AnimationIntent = _currentMove.AnimHash;
            context.forceAnimationRestart = true;
        }

        public override void Tick(PlayerContext context, float deltaTime)
        {
            context.attackTimer -= deltaTime;
            base.Tick(context, deltaTime);
        }

        public override void Exit(PlayerContext context)
        {
            context.AdapterHitApplier.SetActive(false);
            context.attackCoolDown = _currentMove.Cooldown;   
        }
    }
}
