namespace MonStorm.Core.StateMachine
{
    public interface IFSMAdapterNavMeshAgent
    {
        public float AgentSpeed { get; }


        public bool MoveRelative(float x, float y, float z);
        public void CancelMove();
        public void ChangeSpeed(float newValue);
    }
}
