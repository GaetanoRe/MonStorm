using UnityEngine;

public class CreatureCombat : MonoBehaviour
{
    CreatureBehavior creatureBehavior;
    HitApplier hitApplier;


    void Awake()
    {
        hitApplier = transform.GetComponentInChildren<HitApplier>();

        if (hitApplier == null) Debug.LogWarning($"HitApplier not assigned to {gameObject}.");

        if (!TryGetComponent(out creatureBehavior)) Debug.LogWarning($"CreatureBehavior not assigned to {gameObject}.");
    }

    void Start()
    {
        if (creatureBehavior != null) creatureBehavior.CreatureContext.OnAttackAnimationEnd += DeactivateAttack;
    }

    void OnDestroy()
    {
        if (creatureBehavior != null) creatureBehavior.CreatureContext.OnAttackAnimationEnd -= DeactivateAttack;
    }

    public void ActivateAttack()
    {
        if (creatureBehavior == null || hitApplier == null) return;

        hitApplier.SetActive(true);
    }

    public void DeactivateAttack()
    {
        if (creatureBehavior == null || hitApplier == null) return;

        hitApplier.SetActive(false);
    }
}
