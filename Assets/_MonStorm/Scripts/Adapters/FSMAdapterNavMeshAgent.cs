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
        public float? DistanceToTarget => HasActivePath ? navMeshAgent.remainingDistance : null;
        public System.Numerics.Vector3? TargetPosition => HasActivePath ? UnityToNumericsVector3(navMeshAgent.destination) : null;
        public bool HasActivePath =>
            navMeshAgent.pathPending ||
            navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance ||
            (navMeshAgent.hasPath && navMeshAgent.velocity.sqrMagnitude != 0f);

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

        public async void RotateTowardsPosition(System.Numerics.Vector3 targetPosition)
        {
            float rotationDuration = 0.33f;
            Vector3 direction = NumericsToUnityVector3(targetPosition) - navMeshAgent.transform.position;

            if (direction == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion startRotation = navMeshAgent.transform.rotation;
            float elapsed = 0f;

            while (elapsed < rotationDuration && navMeshAgent.transform != null)
            {
                elapsed += Time.deltaTime;
                float percentage = elapsed / rotationDuration;

                navMeshAgent.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, percentage);

                await System.Threading.Tasks.Task.Yield();
            }

            navMeshAgent.transform.rotation = targetRotation;
        }

        public void CancelMove()
        {
            navMeshAgent.ResetPath();
            navMeshAgent.velocity = Vector3.zero;
        }

        public void ChangeSpeed(float newValue)
        {
            navMeshAgent.speed = newValue;
        }
    }
}
