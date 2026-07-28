namespace MonStorm.Core.StateMachine
{
    public interface IFSMAdapterTransform
    {
        public System.Numerics.Vector3 Pos => new(XPos, YPos, ZPos);
        public float XPos { get; }
        public float YPos { get; }
        public float ZPos { get; }
    }
}
