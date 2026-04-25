using UnityEngine;
using System;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public event Action OnSizeChanged;
    public InventorySlot[] ItemSlotArray { get; private set; }

    [Serializable] class InspectorItem
    {
        public InventoryItemDataEXAMPLE data;
        public int amount;
    }

    [Header("Properties")]
    [SerializeField] int startingSlotAmount;
    [SerializeField] List<InspectorItem> startingItemList = new();


    // Checks if the provided items are of the same type using the item's name.
    // We can change this to use an item ID or anything else in the future.
    // Note that if any item (or both) is null (empty) then this returns false.
    // Returns true if the provided items are of the same type, false otherwise.
    public static bool AreItemsTheSame(InventoryItem item1, InventoryItem item2) => IsItemValid(item1) && IsItemValid(item2) && string.Equals(item1.Data.Name, item2.Data.Name);

    public static bool AreItemsTheSameAndStackable(InventoryItem item1, InventoryItem item2) => AreItemsTheSame(item1, item2) && item1.Data.IsStackable;

    public static bool IsItemValid(InventoryItem item) => item != null && item.Data != null && item.Amount > 0;

    // Adds an item, first tries to stack with any item of the same type, then tries to add to an empty slot
    // Returns the remainder item if the item has been partially stacked and the rest of the slots are occupied
    public InventoryItem Add(InventoryItem item)
    {
        item = StackItem(item);
        if (item == null) return null;

        foreach (InventorySlot slot in ItemSlotArray)
        {
            if (!slot.IsEmpty) continue;

            slot.Interact(item);
            return null;
        }

        return item;
    }

    public void Add(InventoryItem item, InventorySlot slot)
    {
        slot.Interact(item);
    }

    public void Remove(InventorySlot slot)
    {
        slot.Remove();
    }

    public List<InventoryItem> ChangeInventorySize(int newSize)
    {
        if (ItemSlotArray.Length == newSize) return null;

        List<InventoryItem> returnItemList = new();
        int oldSize = ItemSlotArray.Length;

        if (newSize < oldSize)
        {
            for (int i = newSize; i < ItemSlotArray.Length; i++)
            {
                if (ItemSlotArray[i].IsEmpty) continue;

                returnItemList.Add(ItemSlotArray[i].Item);
            }
        }

        InventorySlot[] newArray = new InventorySlot[newSize];
        Array.Copy(ItemSlotArray, newArray, Math.Min(ItemSlotArray.Length, newSize));
        ItemSlotArray = newArray;

        if (newSize > oldSize)
        {
            for (int i = oldSize; i < newSize; i++)
            {
                ItemSlotArray[i] = new InventorySlot();
            }
        }

        OnSizeChanged?.Invoke();
        return returnItemList.Count == 0 ? null : returnItemList;
    }

    void Awake()
    {
        ItemSlotArray = new InventorySlot[startingSlotAmount];

        for (int i = 0; i < startingSlotAmount; i++)
        {
            ItemSlotArray[i] = new InventorySlot();
        }

        foreach (InspectorItem item in startingItemList)
        {
            Add(new InventoryItem(item.data, item.amount));
        }
    }

    // Stacks the item and returns the remainder
    InventoryItem StackItem(InventoryItem item)
    {
        foreach (InventorySlot slot in ItemSlotArray)
        {
            if (!AreItemsTheSame(item, slot.Item)) continue;

            item = slot.Interact(item);

            if (item == null) return null;
        }
        return item;
    }

    // Stacks the item only if it can be fully stacked
    bool TryStackItem(InventoryItem item)
    {
        int availableSlots = 0;
        List<InventorySlot> stackableSlots = new();

        foreach (InventorySlot slot in ItemSlotArray)
        {
            if (!AreItemsTheSame(item, slot.Item)) continue;

            availableSlots += slot.Item.Data.MaxStackSize - slot.Item.Amount;
            stackableSlots.Add(slot);

            if (availableSlots >= item.Amount) break;
        }

        if (availableSlots < item.Amount) return false;

        foreach (InventorySlot slot in stackableSlots)
        {
            item = slot.Interact(item);
            if (item == null) break;
        }

        return true;
    }

    InventorySlot GetFirstEmptySlot()
    {
        foreach (InventorySlot slot in ItemSlotArray)
        {
            if (!slot.IsEmpty) continue;

            return slot;
        }
        return null;
    }
}
