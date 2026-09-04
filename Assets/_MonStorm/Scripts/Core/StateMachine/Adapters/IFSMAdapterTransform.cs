namespace MonStorm.Core.StateMachine
{
    public interface IFSMAdapterTransform
    {
        /// <summary>The world position of the transform.</summary>
        public System.Numerics.Vector3 Position { get; }
        /// <summary>The world position of the transform as a Vector2, without the Y position, from the Vector3 to the Vector2, X maps to X, and Z maps to Y.</summary>
        public System.Numerics.Vector2 PositionV2 { get; }
        /// <summary>The transform's forward direction.</summary>
        public System.Numerics.Vector3 Forward { get; }
    }
}
