using System.Numerics;

namespace MonStorm.Core.Player
{
    public class PlayerContext
    {
        // Player Stats
        public float health = 100; // Current health of the player
        public float healthCap = 100; // The capacity of the health bar
        public float stamina = 100;
        public float staminaCap = 100;
        const float maxHealth = 300; // The maximum amount of health a player can recieve.
        const float maxStamina = 300; // The maximum amount of stamina the player can have



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

        public IWeaponData currentWeapon;

        // State
        public bool inBattle;
        public bool isWeaponSheathed;
        public bool isTargeting;
        public bool isHit;
        public bool isSprinting;
        public bool sprintHeld;
        public float attackCoolDown;
        public float dodgeCoolDown;
        public Vector3 velocity;


        // Config
        public PlayerConfig config;
    }
}