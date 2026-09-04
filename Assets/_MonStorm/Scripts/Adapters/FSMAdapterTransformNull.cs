using MonStorm.Core.StateMachine;
using System.Numerics;

namespace MonStorm.Adapters
{
    public class FSMAdapterTransformNull : IFSMAdapterTransform
    {
        public Vector3 Position => Vector3.Zero;
        public Vector2 PositionV2 => Vector2.Zero;
        public Vector3 Forward => Vector3.Zero;
    }
}
