using UnityEngine;
using UnityEngine.AI;
using MonStorm.Core.StateMachine;

namespace MonStorm.Adapters
{
    public class FSMAdapterNavMeshAgent : IFSMAdapterNavMeshAgent
    {
        public float AgentSpeed => navMeshAgent.speed;

        readonly NavMeshAgent navMeshAgent;


        public FSMAdapterNavMeshAgent(NavMeshAgent navMeshAgent) => this.navMeshAgent = navMeshAgent;

        public bool MoveRelative(float x, float y, float z)
        {
            Vector3 destination = navMeshAgent.transform.position;
            destination += new Vector3(x, y, z);

            return navMeshAgent.SetDestination(destination);
        }

        public bool FollowTarget(IFSMAdapterTransform target) => navMeshAgent.SetDestination(new(target.XPos, target.YPos, target.ZPos));

        public void CancelMove()
        {
            navMeshAgent.ResetPath();
        }

        public void ChangeSpeed(float newValue)
        {
            navMeshAgent.speed = newValue;
        }
    }
}
