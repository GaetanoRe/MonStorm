using UnityEngine;
using MonStorm.Core.StateMachine;
using static MonStorm.Adapters.AdaptersUtils;

namespace MonStorm.Adapters
{
    public class FSMAdapterTransform : IFSMAdapterTransform
    {
        public System.Numerics.Vector3 Position => UnityToNumericsVector3(t.position);

        readonly Transform t;


        public FSMAdapterTransform(Transform t) => this.t = t;
    }
}
