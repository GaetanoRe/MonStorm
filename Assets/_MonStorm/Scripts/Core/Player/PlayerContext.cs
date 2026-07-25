using UnityEngine;

namespace MonStorm.Core.Player
{
    public class PlayerContext
    {
        // Player Stats
        public float health = 100;
        public float healthCap = 100;
        public float stamina = 100;
        public float staminaCap = 100;
        const float maxHealth = 300;
        const float maxStamina = 300;

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
