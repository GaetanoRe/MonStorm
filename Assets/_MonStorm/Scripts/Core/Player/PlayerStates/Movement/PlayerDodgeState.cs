using System;
using System.Numerics;
using MonStorm.Core.StateMachine;

namespace MonStorm.Core.Player
{
    public class PlayerDodgeState : PlayerBaseState
    {
        protected override void SetupTransitions(PlayerContext context)
        {
            transitionManager.Initialize(
                new StateTransition<PlayerContext>(new PlayerIdleState(), () => context.dodgeTimer <=0 && context.moveInput == Vector2.Zero),
                new StateTransition<PlayerContext>(new PlayerWalkState(), () => context.dodgeTimer <= 0 && context.moveInput != Vector2.Zero)
            );
        }

        public override void Enter(PlayerContext context)
        {
            base.Enter(context);
            context.dodgePressed = false;
            context.dodgeTimer = context.dodgeDuration;
            context.iFrameActive = true;
            context.crossFadeOverride = 0f;
            context.AnimationIntent = context.DodgeAnimHash;
            Vector3 direction = new Vector3(context.moveInput.X, 0, context.moveInput.Y);
            if(direction.LengthSquared() != 0)
            {
                context.dodgeDirection = Vector3.Normalize(direction);
            }
            else
            {
                context.dodgeDirection = Vector3.Zero;
            }
            
            
            context.forceAnimationRestart = true;
        }

        public override void Tick(PlayerContext context, float deltaTime)
        {
            float progress = Math.Clamp(1f - (context.dodgeTimer / context.dodgeDuration), 0f, 1f);
            float speedMultiplier = (float) Math.Sin(progress * Math.PI);
            context.velocity.X = context.dodgeDirection.X * context.dodgeSpeed * speedMultiplier;
            context.velocity.Z = context.dodgeDirection.Z * context.dodgeSpeed * speedMultiplier;
            context.dodgeTimer -= deltaTime;
            base.Tick(context, deltaTime);
        }

        public override void Exit(PlayerContext context)
        {
            context.iFrameActive = false;
            context.dodgeCoolDown = context.dodgeDuration;
        }
    }
}
