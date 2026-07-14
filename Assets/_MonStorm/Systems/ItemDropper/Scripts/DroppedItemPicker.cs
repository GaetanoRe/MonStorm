using UnityEngine;
using UnityEngine.InputSystem;

public class DroppedItemPicker : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] LayerMask pickupMask;
    [SerializeField] float pickupRange;

    readonly Collider[] colsInRange = new Collider[10];


    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryPickup();
        }
    }

    // Picks up the closest DroppedItem from the ground within the pickupRange and adds it to the inventory
    void TryPickup()
    {
        if (Physics.OverlapSphereNonAlloc(transform.position, pickupRange, colsInRange, pickupMask) == 0) return;

        if (!GetClosestCollider().transform.parent.TryGetComponent(out DroppedItem droppedItem)) return;

        if (!inventory.AddFully(droppedItem.Item)) return;

        droppedItem.DestroyItem();
    }

    // Returns the closest collider from the colsInRange array
    Collider GetClosestCollider()
    {
        float closestDistance = pickupRange;
        Collider closestCol = null;

        foreach (Collider col in colsInRange)
        {
            if (col == null) continue;

            float distance = Vector3.Distance(transform.position, col.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCol = col;
            }
        }

        return closestCol;
    }
}
