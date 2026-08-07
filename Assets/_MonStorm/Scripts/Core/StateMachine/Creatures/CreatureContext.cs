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
