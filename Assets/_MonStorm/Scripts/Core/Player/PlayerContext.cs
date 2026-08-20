using System.Numerics;
using MonStorm.Core.Combat;
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
        public int AnimationIntent;
        public int MovementTreeHash;

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
        
        // Combat-Oriented Fields
        public WeaponDefinition equippedWeapon;
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
