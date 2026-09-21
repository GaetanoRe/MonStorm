using MonStorm.Core.StateMachine;

namespace MonStorm.Adapters
{
    public class FSMAdapterSensorVisionNull : IFSMAdapterSensorVision
    {
        public bool IsFacingTarget => false;
        public bool IsTargetInVision => false;
    }
}
