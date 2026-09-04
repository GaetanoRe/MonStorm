namespace MonStorm.Core.StateMachine
{
    public interface IFSMAdapterSensorVision
    {
        /// <summary>
        /// Whether the creature is rotated towards the target, within a margin of error (facingAngleThreshold)
        /// Also returns true if the distance to the target is within closeFacingDistanceSq.
        /// </summary>
        public bool IsFacingTarget { get; }

        /// <summary>
        /// Whether the target is in vision, within range and within the max angle
        /// Currently does not take LOS (line of sight) into account
        /// </summary>
        public bool IsTargetInVision { get; }
    }
}
