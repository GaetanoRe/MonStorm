using System.Numerics;

namespace MonStorm.Core.StateMachine
{
    public interface IFSMAdapterNavMeshAgent
    {
        /// <summary>The speed in units/s at which the agent moves through the nav mesh.</summary>
        public float AgentSpeed { get; }

        /// <summary>The current distance from this agent to it's current target, null if it currently doesn't have a target.</summary>
        public float? DistanceToTarget { get; }

        /// <summary>The world position of this agent's current target, null if it currently doesn't have a target.</summary>
        public Vector3? TargetPosition { get; }


        /// <summary>Moves the agent relative to it's current position by the provided relative position.</summary>
        /// <param name="relativePosition">The amount that will be added to the agent's current position.</param>
        /// <returns>Whether the new position is valid, and subsequently whether the move command was issued.</returns>
        public bool MoveRelative(Vector3 relativePosition);

        /// <summary>Moves the agent to the provided world position.</summary>
        /// <param name="position">The world position which the agent will move to.</param>
        /// <returns>Whether the world position is valid, and subsequently whether the move command was issued.</returns>
        public bool MoveToWorldPosition(Vector3 position);

        /// <summary>Moves the agent to the provided target.</summary>
        /// <param name="target">The target which the agent will move to.</param>
        /// <returns>Whether the position of the target is valid, and subsequently whether the move command was issued.</returns>
        public bool FollowTarget(IFSMAdapterTransform target);

        /// <summary>Cancels the current movement command of the agent if it has any.</summary>
        public void CancelMove();

        /// <summary>Changes the speed at which the agent moves through the nav mesh.</summary>
        /// <param name="newValue"></param>
        public void ChangeSpeed(float newValue);
    }
}
