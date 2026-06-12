using UnityEngine;

public class DroppedItemEXAMPLE : MonoBehaviour
{
    public void Initialize(InventoryItem item)
    {
        Inventory inventory = FindFirstObjectByType<Inventory>();
        inventory.Add(item);
        Destroy(gameObject);
    }
}
