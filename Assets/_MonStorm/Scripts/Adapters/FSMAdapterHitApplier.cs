using MonStorm.Core.StateMachine;

namespace MonStorm.Adapters
{
    public class FSMAdapterHitApplier : IFSMAdapterHitApplier
    {
        readonly HitApplierComponentConfigured hitApplier;

        public FSMAdapterHitApplier(HitApplierComponentConfigured hitApplier) => this.hitApplier = hitApplier;

        public void SetActive(bool active) => hitApplier.Data.SetActive(active);
    }
}
