using System.Linq.Expressions;
using System.Numerics;
using MonStorm.Core.StateMachine;
using Unity.Plastic.Antlr3.Runtime.Tree;
using Unity.VisualScripting.IonicZip;

namespace MonStorm.Core.Player
{
    public class PlayerIdleState : IState<PlayerContext>
    {
        public void Enter(PlayerContext context)
        {
            
        }

        public IState<PlayerContext> Tick(PlayerContext context, float deltaTime)
        {
            if (context.isHit)
            {
                return new PlayerDamagedState();
            }
            if(context.moveInput != Vector2.Zero)
            {
                if (context.dodgePressed && context.dodgeCoolDown <= 0)
                {
                    return new PlayerDodgeState();
                }
                if (context.isSprinting)
                {
                    return new PlayerRunState();
                }
                return new PlayerWalkState();   
            }
            else if(context.dodgePressed)
            {
                return new PlayerSneakState();
            }
            return this;
        }

        public void Exit(PlayerContext context)
        {
            
        }
    }
}