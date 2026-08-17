using System.Numerics;
using System;

namespace MonStorm.Core.StateMachine
{
    public class CreatureContext
    {
        public StateMachine<CreatureContext> StateMachine { get; set; }
        public IFSMAdapterAnimator AdapterAnimator { get; }
        public IFSMAdapterNavMeshAgent AdapterNavMeshAgent { get; }
        public IFSMAdapterTransform AdapterTransform { get; }
        public IFSMAdapterLogger AdapterLogger { get; }
        public IFSMAdapterTransform PlayerTransform { get; }

        public Action OnAttackAnimationEnd;

        public float DistanceToPlayer => Vector2.Distance(AdapterTransform.PositionV2, PlayerTransform.PositionV2);

        /// <summary>
        /// Whether the creature is rotated towards the player, within a margin of error (facingAngleThreshold)
        /// Also returns true if the distance to the player is within closeFacingDistanceSq.
        /// </summary>
        public bool IsFacingPlayer
        {
            get
            {
                if (AdapterTransform == null || PlayerTransform == null) return false;

                Vector3 directionToPlayer = PlayerTransform.Position - AdapterTransform.Position;
                directionToPlayer.Y = 0f;

                if (directionToPlayer.LengthSquared() <= closeFacingDistanceSq) return true;

                Vector3 dirNormalized = Vector3.Normalize(directionToPlayer);
                Vector3 forwardNormalized = Vector3.Normalize(new Vector3(AdapterTransform.Forward.X, 0f, AdapterTransform.Forward.Z));

                float dot = Vector3.Dot(forwardNormalized, dirNormalized);

                float dotThreshold = MathF.Cos(facingAngleThreshold * (MathF.PI / 180f));

                return dot >= dotThreshold;
            }
        }

        const float facingAngleThreshold = 15f;
        const float closeFacingDistanceSq = 0.5f;


        public CreatureContext(StateMachine<CreatureContext> stateMachine, IFSMAdapterAnimator adapterAnimator, IFSMAdapterNavMeshAgent adapterNavMeshAgent, IFSMAdapterTransform adapterTransform, IFSMAdapterLogger adapterLogger, IFSMAdapterTransform playerTransform)
        {
            StateMachine = stateMachine;
            AdapterAnimator = adapterAnimator;
            AdapterNavMeshAgent = adapterNavMeshAgent;
            AdapterTransform = adapterTransform;
            AdapterLogger = adapterLogger;
            PlayerTransform = playerTransform;
        }
    }
}
