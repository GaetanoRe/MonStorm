namespace MonStorm.Core.StateMachine
{
    public interface IFSMAdapterSensorVision
    {
        public bool IsFacingTarget { get; }
        public bool IsTargetInVision { get; }
    }
}
