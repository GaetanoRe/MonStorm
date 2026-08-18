namespace MonStorm.Core.StateMachine
{
    public interface IFSMAdapterAnimator
    {
        public bool IsAnimationFinished { get; }


        public void Play(int hash);
    }
}
