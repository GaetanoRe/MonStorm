using UnityEngine;
using MonStorm.Core.StateMachine;

namespace MonStorm.Adapters
{
    /// <inheritdoc/>
    public class FSMAdapterAnimator : IFSMAdapterAnimator
    {
        public bool IsAnimationFinished => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !animator.IsInTransition(0);

        readonly Animator animator;
        readonly float transitionDuration = 0.2f;


        public FSMAdapterAnimator(Animator animator) => this.animator = animator;

        public void Play(int hash)
        {
            animator.CrossFadeInFixedTime(hash, transitionDuration);
        }
    }
}
