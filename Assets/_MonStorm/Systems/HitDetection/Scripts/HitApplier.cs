using System;
using System.Collections.Generic;

public class HitApplier
{
    /// <summary>Invoked when the HitApplier's detection status changes, e.g from inactive to active or otherwise.</summary>
    public event Action<bool> OnActiveChanged;

    /// <summary>The amount of damage this will do BEFORE any other calculations.</summary>
    public float Damage { get; }

    readonly IHitReceiver ownerReceiver;
    readonly HashSet<HitDetector> currentDetections = new();
    bool isActive;


    public HitApplier(float damage, IHitReceiver ownerReceiver = null)
    {
        Damage = damage;
        this.ownerReceiver = ownerReceiver;
    }

    /// <summary>Activates the HitApplier so it can detect collisions.</summary>
    /// <param name="active">Whether to activate or deactivate the HitApplier.</param>
    public void SetActive(bool active)
    {
        if (isActive == active) return;

        currentDetections.Clear();
        isActive = active;
        OnActiveChanged?.Invoke(active);
    }

    /// <summary>Update the component with the currently hit HitDetector</summary>
    /// <param name="hitDetector"></param>
    public void UpdateCollisions(HitDetector hitDetector, HitApplierComponentConfigured hitApplier)
    {
        if (!isActive) return;

        if (hitDetector.HitReceiver == ownerReceiver) return;

        // Only the first part of the object the weapon makes contact with will be hit.
        // If we have already hit another part of the same object then we don't apply the hit.
        // Here we check if the same object has already been hit in the current attack, and if so we return.
        // For example: The sword swings and hits some creature's legs, the swing continues and hits the body next,
        // but since we already hit that creature's legs, we don't apply damage again to the body.
        foreach (HitDetector detector in currentDetections)
        {
            if (detector.HitReceiver == hitDetector.HitReceiver) return;
        }

        ApplyHit(hitDetector, hitApplier);
    }

    void ApplyHit(HitDetector newDetector, HitApplierComponentConfigured hitApplier)
    {
        currentDetections.Add(newDetector);
        newDetector.HitReceiver.HandleHit(hitApplier, newDetector);
    }
}
