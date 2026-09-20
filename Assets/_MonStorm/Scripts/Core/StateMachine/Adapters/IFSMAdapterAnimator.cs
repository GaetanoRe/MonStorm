namespace MonStorm.Core.StateMachine
{
    public interface IFSMAdapterAnimator
    {
        /// <summary>Whether the animator's current animation has finished playing.</summary>
        public bool IsAnimationFinished { get; }


        /// <summary>Instantly play the animation that corresponds to the provided hash.</summary>
        /// <param name="hash">The animation's name as a hash.</param>
        public void Play(int hash);
    }
}
