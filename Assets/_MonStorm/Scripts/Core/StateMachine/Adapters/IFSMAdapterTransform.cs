namespace MonStorm.Core.StateMachine
{
    public interface IFSMAdapterTransform
    {
        public System.Numerics.Vector3 Position { get; }
        public System.Numerics.Vector2 PositionV2 { get; }
        public System.Numerics.Vector3 Forward { get; }
    }
}
