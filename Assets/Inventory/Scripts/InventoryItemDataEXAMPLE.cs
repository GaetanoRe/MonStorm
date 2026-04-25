using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Item", menuName = "SO/InventoryItem")]
public class InventoryItemDataEXAMPLE : ScriptableObject, IInventoryItemData
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public bool IsStackable { get; private set; }
    [field: SerializeField] public int MaxStackSize { get; private set; }

    // The rest of the item functionality example
    // int weight = 10;
    // EquipSlot equipSlot = EquipSlot.Head;
    // StatEffect[] statEffectArray;
}
