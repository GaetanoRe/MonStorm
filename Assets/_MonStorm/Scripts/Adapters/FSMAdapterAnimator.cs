using UnityEngine;
using MonStorm.Core.StateMachine;

namespace MonStorm.Adapters
{
    public class FSMAdapterAnimator : IFSMAdapterAnimator
    {
        readonly Animator animator;
        readonly float transitionDuration = 0.2f;


        public FSMAdapterAnimator(Animator animator) => this.animator = animator;

        public void Play(int hash)
        {
            animator.CrossFadeInFixedTime(hash, transitionDuration);
        }
    }
}
