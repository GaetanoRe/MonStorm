using UnityEngine;

// Inventory Item Data can be a scriptable object that inherits from this interface.
// Everything else about the Item Data can be stored on it, but the inventory doesn't care about
// any of that and only communicates with the item through this interface.
public interface IInventoryItemData
{
    public string Name { get; }
    public Sprite Icon { get; }
    public bool IsStackable { get; }

    // Applicable only if IsStackable == true
    public int MaxStackSize { get; }
}
