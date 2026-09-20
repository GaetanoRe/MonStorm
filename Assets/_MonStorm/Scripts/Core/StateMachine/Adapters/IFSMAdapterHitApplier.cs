namespace MonStorm.Core.StateMachine
{
    public interface IFSMAdapterHitApplier
    {
        /// <summary>Activates the HitApplier so it can detect collisions.</summary>
        /// <param name="active">Whether to activate or deactivate the HitApplier.</param>
        public void SetActive(bool active);
    }
}
