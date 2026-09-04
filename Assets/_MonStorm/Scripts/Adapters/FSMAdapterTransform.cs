using UnityEngine;
using MonStorm.Core.StateMachine;
using static MonStorm.Adapters.AdaptersUtils;

namespace MonStorm.Adapters
{
    /// <inheritdoc/>
    public class FSMAdapterTransform : IFSMAdapterTransform
    {
        public System.Numerics.Vector3 Position => UnityToNumericsVector3(t.position);
        public System.Numerics.Vector2 PositionV2 => new(t.position.x, t.position.z);
        public System.Numerics.Vector3 Forward => new(t.forward.x, t.forward.y, t.forward.z);

        readonly Transform t;


        public FSMAdapterTransform(Transform t) => this.t = t;
    }
}
