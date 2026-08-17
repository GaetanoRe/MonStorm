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
        if (creatureBehavior != null) creatureBehavior.CreatureContext.OnAttackAnimationEnd += CloseHitWindow;
    }

    void OnDestroy()
    {
        if (creatureBehavior != null) creatureBehavior.CreatureContext.OnAttackAnimationEnd -= CloseHitWindow;
    }

    /// <summary>
    /// Activates the associated weapon to register hits.
    /// The same hitbox can only be hit once even if the weapon makes multiple contacts during a single attack,
    /// when the attack ends (DeactivateAttack is called) this resets.
    /// </summary>
    /// <remarks>Also called from Unity via events.</remarks>
    public void OpenHitWindow()
    {
        if (creatureBehavior == null || hitApplier == null) return;

        hitApplier.SetActive(true);
    }

    /// <summary>Deactivates the associated weapon to no longer register hits.</summary>
    /// <remarks>Also called from Unity via events.</remarks>
    public void CloseHitWindow()
    {
        if (creatureBehavior == null || hitApplier == null) return;

        hitApplier.SetActive(false);
    }
}
