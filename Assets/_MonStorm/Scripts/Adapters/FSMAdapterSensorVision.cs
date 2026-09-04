using System;
using System.Numerics;
using MonStorm.Core.StateMachine;

namespace MonStorm.Adapters
{
    public class FSMAdapterSensorVision : IFSMAdapterSensorVision
    {
        /// <summary>
        /// Whether the creature is rotated towards the target, within a margin of error (facingAngleThreshold)
        /// Also returns true if the distance to the target is within closeFacingDistanceSq.
        /// </summary>
        public bool IsFacingTarget
        {
            get
            {
                Vector3 directionToTarget = target.Position - transform.Position;
                directionToTarget.Y = 0f;

                if (directionToTarget.LengthSquared() <= closeFacingDistanceSq) return true;

                Vector3 dirNormalized = Vector3.Normalize(directionToTarget);
                Vector3 forwardNormalized = Vector3.Normalize(new Vector3(transform.Forward.X, 0f, transform.Forward.Z));

                float dot = Vector3.Dot(forwardNormalized, dirNormalized);

                float dotThreshold = MathF.Cos(facingAngleThreshold * (MathF.PI / 180f));

                return dot >= dotThreshold;
            }
        }

        /// <summary>
        /// Whether the target is in vision, within range and within the max angle
        /// Currently does not take LOS (line of sight) into account
        /// </summary>
        public bool IsTargetInVision
        {
            get
            {
                if (Vector3.Distance(transform.Position, target.Position) > visionRadius) return false;

                Vector3 directionToTarget = target.Position - transform.Position;
                float angle = AdaptersUtils.NumericsAngle(transform.Forward, directionToTarget);

                if (angle > visionMaxAngle) return false;

                return true;
            }
        }

        readonly IFSMAdapterTransform transform;
        readonly IFSMAdapterTransform target;
        readonly float visionRadius;
        readonly float visionMaxAngle;

        const float facingAngleThreshold = 15f;
        const float closeFacingDistanceSq = 0.5f;


        public FSMAdapterSensorVision(IFSMAdapterTransform transform, IFSMAdapterTransform target, float visionRadius, float visionMaxAngle)
        {
            this.transform = transform;
            this.target = target;
            this.visionRadius = visionRadius;
            this.visionMaxAngle = visionMaxAngle;
        }
    }
}
