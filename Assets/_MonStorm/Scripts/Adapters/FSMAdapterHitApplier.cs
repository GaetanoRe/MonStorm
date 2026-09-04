using MonStorm.Core.StateMachine;

namespace MonStorm.Adapters
{
    /// <inheritdoc/>
    public class FSMAdapterHitApplier : IFSMAdapterHitApplier
    {
        readonly HitApplier hitApplier;


        public FSMAdapterHitApplier(HitApplier hitApplier) => this.hitApplier = hitApplier;

        public void SetActive(bool active) => hitApplier.SetActive(active);
    }
}
