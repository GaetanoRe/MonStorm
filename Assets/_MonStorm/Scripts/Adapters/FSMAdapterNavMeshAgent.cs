using UnityEngine.AI;
using System.Numerics;
using MonStorm.Core.StateMachine;
using static MonStorm.Adapters.AdaptersUtils;

namespace MonStorm.Adapters
{
    /// <inheritdoc/>
    public class FSMAdapterNavMeshAgent : IFSMAdapterNavMeshAgent
    {
        public float AgentSpeed => navMeshAgent.speed;
        public float? DistanceToTarget => HasActivePath ? navMeshAgent.remainingDistance : null;
        public Vector3? TargetPosition => HasActivePath ? UnityToNumericsVector3(navMeshAgent.destination) : null;
        public bool HasActivePath =>
            !navMeshAgent.pathPending &&
            navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance &&
            (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f);

        readonly NavMeshAgent navMeshAgent;


        public FSMAdapterNavMeshAgent(NavMeshAgent navMeshAgent) => this.navMeshAgent = navMeshAgent;

        public bool MoveRelative(Vector3 relativePosition)
        {
            UnityEngine.Vector3 destination = AddUnityWithNumericsVector3(navMeshAgent.transform.position, relativePosition);
            return navMeshAgent.SetDestination(destination);
        }

        public bool MoveToWorldPosition(Vector3 position)
        {
            return navMeshAgent.SetDestination(NumericsToUnityVector3(position));
        }

        public bool FollowTarget(IFSMAdapterTransform target)
        {
            return navMeshAgent.SetDestination(NumericsToUnityVector3(target.Position));
        }

        public void CancelMove()
        {
            navMeshAgent.ResetPath();
            navMeshAgent.velocity = UnityEngine.Vector3.zero;
        }

        public void ChangeSpeed(float newValue)
        {
            navMeshAgent.speed = newValue;
        }
    }
}
