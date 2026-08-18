using MonStorm.Core.StateMachine;

namespace MonStorm.Adapters
{
    /// <summary>Used as a null object for the FSM if the Animator is missing.</summary>
    public class FSMAdapterAnimatorNull : IFSMAdapterAnimator
    {
        public bool IsAnimationFinished => true;


        public void Play(int hash) { } // noop
    }
}
