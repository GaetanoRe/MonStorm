using MonStorm.Core.StateMachine;
using System.Numerics;

namespace MonStorm.Adapters
{
    /// <summary>Used as a null object for the FSM if the Nav Mesh Agent is missing.</summary>
    /// <inheritdoc/>
    public class FSMAdapterNavMeshAgentNull : IFSMAdapterNavMeshAgent
    {
        public float AgentSpeed => 5f;
        public float? DistanceToTarget => 0f;
        public Vector3? TargetPosition => new();
        public bool HasActivePath => true;


        public bool MoveRelative(Vector3 relativePosition) => true;

        public bool MoveToWorldPosition(Vector3 position) => true;

        public bool FollowTarget(IFSMAdapterTransform target) => true;

        public void CancelMove() { } // noop

        public void ChangeSpeed(float newValue) { } // noop
    }
}
