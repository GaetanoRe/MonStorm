using UnityEngine;
using System;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    /// <summary>Invoked when ItemSlotArray[]'s size changes.</summary>
    public event Action OnSizeChanged;

    /// <summary>The array that holds all InventoryItems and acts as the inventory.</summary>
    public InventorySlot[] ItemSlotArray { get; private set; }

    [Serializable] class InspectorItem
    {
        public InventoryItemDataEXAMPLE data;
        public int amount;
    }

    [Header("Properties")]
    [SerializeField] int startingSlotAmount;
    [SerializeField] List<InspectorItem> startingItemList = new();


    /// <summary>
    /// Checks if the provided InventoryItems are of the same type using the InventoryItem's names.
    /// Note that if any InventoryItem (or both) is null (empty) then this returns false.
    /// </summary>
    /// <remarks>We can change this to use an item ID or anything else in the future.</remarks>
    /// <param name="item1">The first InventoryItem to compare.</param>
    /// <param name="item2">The second InventoryItem to compare.</param>
    /// <returns>True if the provided InventoryItem are of the same type, false otherwise.</returns>
    public static bool AreItemsTheSame(InventoryItem item1, InventoryItem item2) => IsItemValid(item1) && IsItemValid(item2) && string.Equals(item1.Data.Name, item2.Data.Name);

    /// <summary>
    /// Checks if the provided InventoryItems are of the same type (using the InventoryItem's names) and if they are stackable.
    /// Note that if any InventoryItem (or both) is null (empty) then this returns false.
    /// </summary>
    /// <param name="item1">The first InventoryItem to compare.</param>
    /// <param name="item2">The second InventoryItem to compare.</param>
    /// <returns>True if the provided InventoryItems are of the same type and stackable, false otherwise.</returns>
    public static bool AreItemsTheSameAndStackable(InventoryItem item1, InventoryItem item2) => AreItemsTheSame(item1, item2) && item1.Data.IsStackable;

    /// <summary>Checks if the provided InventoryItem has data associated with it, and if it's amount is above 0.</summary>
    /// <param name="item">The InventoryItem to check.</param>
    /// <returns>True if the InventoryItem is valid, false otherwise.</returns>
    public static bool IsItemValid(InventoryItem item) => item != null && item.Data != null && item.Amount > 0;

    /// <summary>Adds the provided InventoryItem to the inventory.</summary>
    /// <remarks>First tries to stack with any InventoryItem of the same type, then adds the remainder (if any) of the InventoryItem to the first empty InventorySlot.</remarks>
    /// <param name="item">The InventoryItem to add.</param>
    /// <returns>
    /// The remainder InventoryItem if the InventoryItem has been partially added and there are no more free InventorySlots.
    /// Example: Meat (5) gets passed as a parameter, the inventory can only hold 2 more meat, Meat (2) gets added, Meat (3) gets returned.
    /// Null if there's no remainder.
    /// </returns>
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

    /// <summary>Adds the provided InventoryItem to the provided InventorySlot.</summary>
    /// <param name="item">The InventoryItem to add.</param>
    /// <param name="slot">The InventorySlot to which the InventoryItem will be added to.</param>
    /// <returns>The InventoryItem from the provided InventorySlot (null if empty).</returns>
    public InventoryItem Add(InventoryItem item, InventorySlot slot) => slot.Replace(item);

    /// <summary>Removes the InventoryItem from the provided InventorySlot.</summary>
    /// <param name="slot">The InventorySlot from which the InventoryItem will be removed.</param>
    /// <returns>The removed InventoryItem.</returns>
    public InventoryItem Remove(InventorySlot slot) => slot.Remove();

    /// <summary>Changes the amount of InventorySlots the inventory has.</summary>
    /// <param name="newSize">The new amount of InventorySlots.</param>
    /// <returns>A list of InventoryItems that would be lost if the provided size is smaller than the previous size, null otherwise.</returns>
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

    // Stacks the InventoryItem and returns the remainder
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

    // Stacks the InventoryItem only if it can be fully stacked
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

    // Returns the first InventorySlot from ItemSlotArray[] that doesn't contain an InventoryItem
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
