using System.Numerics;

namespace MonStorm.Core.Player
{
    public class PlayerContext
    {
        // Inputs
        public Vector2 moveInput;
        public Vector2 actionInput;
        public bool isGrounded;
        public bool dodgePressed;
        public float targetDistance;
        public enum ActionInput
        {
            None,
            Action1,
            Action2,
            Action3,
            Action4,
            Action5,
            SpecialAction
        }  
        

        public ActionInput currentAction;

        // State
        public bool inBattle;
        public float attackCoolDown;
        public float dodgeCoolDown;
        public Vector3 velocity;


        // Config
        public PlayerConfig config;
    }
}