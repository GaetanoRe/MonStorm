using System.Numerics;
using MonStorm.Core.StateMachine;

namespace MonStorm.Core.Player
{
    public class PlayerContext
    {
        // State machine
        public StateMachine<PlayerContext> StateMachine { get; set; }

        // Adapters
        public IFSMAdapterAnimator AdapterAnimator { get; set; }
        public IFSMAdapterHitApplier AdapterHitApplier { get; set; }

        // Animation Hashes
        public int IdleAnimHash;
        public int WalkAnimHash;
        public int RunAnimHash;

        public int DamagedAnimHash;
        public int DeathAnimHash;

        public int AttackAnimHash;


        // Player Stats
        const float maxHealth = 300;
        const float maxStamina = 300;
        public const float walkSpeed = 3;
        public float health = 100;
        public float healthCap = 100;
        public float stamina = 100;
        public float staminaCap = 100;
        public float moveSpeed = walkSpeed;

        // Player timers
        public float damagedTimer = 0.667f;
        public float attackTimer = 0.667f;
        

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

        public ActionInput weaponAction;

        // State
        public bool inBattle;
        public bool isWeaponSheathed;
        public bool isDead;
        public bool isTargeting;
        public bool isHit;
        public bool attackPressed;
        public bool isSprinting;
        public bool sprintHeld;
        public float attackCoolDown;
        public float dodgeCoolDown;
        public Vector3 velocity;

    }
}
