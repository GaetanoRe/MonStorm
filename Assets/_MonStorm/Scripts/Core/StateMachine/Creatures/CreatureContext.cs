namespace MonStorm.Core.StateMachine
{
    public class CreatureContext
    {
        public StateMachine<CreatureContext> StateMachine { get; set; }
        public IFSMAdapterAnimator AdapterAnimator { get; }
        public IFSMAdapterNavMeshAgent AdapterNavMeshAgent { get; }
        public IFSMAdapterLogger AdapterLogger { get; }


        public CreatureContext(StateMachine<CreatureContext> stateMachine, IFSMAdapterAnimator adapterAnimator, IFSMAdapterNavMeshAgent adapterNavMeshAgent, IFSMAdapterLogger adapterLogger)
        {
            StateMachine = stateMachine;
            AdapterAnimator = adapterAnimator;
            AdapterNavMeshAgent = adapterNavMeshAgent;
            AdapterLogger = adapterLogger;
        }
    }
}
