using UnityEngine;
using System.Collections.Generic;
using System;

public class HitApplier : MonoBehaviour
{
    /// <summary>The amount of damage this will do BEFORE any other calculations.</summary>
    [field: SerializeField] public float Damage { get; private set; }

    readonly HashSet<HitDetector> currentDetections = new();
    bool isActive;

    public event Action<bool> OnActiveChanged;


    /// <summary>Activates the HitApplier so it can detect collisions.</summary>
    /// <param name="active">Whether to activate or deactivate the HitApplier.</param>
    public void SetActive(bool active)
    {
        currentDetections.Clear();
        isActive = active;
        OnActiveChanged?.Invoke(active);
    }

    void ApplyHit(HitDetector newDetector)
    {
        currentDetections.Add(newDetector);
        newDetector.HitReceiver.HandleHit(this, newDetector);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        if (!other.transform.TryGetComponent(out HitDetector hitDetector)) return;

        // Only the first part of the object the weapon makes contact with will be hit.
        // If we have already hit another part of the same object then we don't apply the hit.
        // Here we check if the same object has already been hit in the current attack, and if so we return.
        foreach (HitDetector detector in currentDetections)
        {
            if (detector.HitReceiver == hitDetector.HitReceiver) return;
        }

        ApplyHit(hitDetector);
    }
}
