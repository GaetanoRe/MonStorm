using UnityEngine;
using MonStorm.Core.StateMachine;

namespace MonStorm.Adapters
{
    public class FSMAdapterTransform : IFSMAdapterTransform
    {
        public float XPos => t.position.x;
        public float YPos => t.position.y;
        public float ZPos => t.position.z;

        readonly Transform t;


        public FSMAdapterTransform(Transform t) => this.t = t;
    }
}
