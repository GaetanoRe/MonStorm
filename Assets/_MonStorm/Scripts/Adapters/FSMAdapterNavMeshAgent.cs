using UnityEngine;
using UnityEngine.AI;
using MonStorm.Core.StateMachine;
using static MonStorm.Adapters.AdaptersUtils;

namespace MonStorm.Adapters
{
    /// <inheritdoc/>
    public class FSMAdapterNavMeshAgent : IFSMAdapterNavMeshAgent
    {
        public float AgentSpeed => navMeshAgent.speed;
        public float? DistanceToTarget => HasActivePath() ? navMeshAgent.remainingDistance : null;
        public System.Numerics.Vector3? TargetPosition => HasActivePath() ? UnityToNumericsVector3(navMeshAgent.destination) : null;

        readonly NavMeshAgent navMeshAgent;


        public FSMAdapterNavMeshAgent(NavMeshAgent navMeshAgent) => this.navMeshAgent = navMeshAgent;

        public bool MoveRelative(System.Numerics.Vector3 relativePosition)
        {
            Vector3 destination = AddUnityWithNumericsVector3(navMeshAgent.transform.position, relativePosition);
            return navMeshAgent.SetDestination(destination);
        }

        public bool MoveToWorldPosition(System.Numerics.Vector3 position)
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
        }

        public void ChangeSpeed(float newValue)
        {
            navMeshAgent.speed = newValue;
        }

        /// <summary>Unity doesn't have a reliable one method to check if an agent currently has a path, so this is a good way to check for that.</summary>
        /// <returns>True if the agent has a path, false otherwise.</returns>
        public bool HasActivePath()
        {
            if (!navMeshAgent.pathPending)
            {
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
