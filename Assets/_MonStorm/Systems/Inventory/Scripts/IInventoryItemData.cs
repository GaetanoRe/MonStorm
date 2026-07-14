using UnityEngine;

public interface IInventoryItemData
{
    public string Name { get; }
    public Sprite Icon { get; }
    public bool IsStackable { get; }
    public int MaxStackSize { get; } // Applicable only if IsStackable == true
}
