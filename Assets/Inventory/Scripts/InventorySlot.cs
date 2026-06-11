using UnityEngine;
using System;
using static Inventory;

public class InventorySlot
{
    /// <summary>Invoked when the InventoryItem held by this InventorySlot changes.</summary>
    public event Action OnItemUpdated;

    /// <summary>Checks if this InventorySlot's InventoryItem is null or contains an InventoryItem.</summary>
    public bool IsEmpty => Item == null;

    /// <summary>The InventoryItem that this InventorySlot holds.</summary>
    public InventoryItem Item
    {
        get => item;
        private set
        {
            if (Item != null) Item.OnAmountUpdated -= OnItemAmountChanged;

            item = value;
            OnItemUpdated?.Invoke();

            if (value != null) Item.OnAmountUpdated += OnItemAmountChanged;
        }
    }

    // Don't change this directly, use the property, which correctly notifies any events necessary
    InventoryItem item;


    public InventorySlot(InventoryItem initializedItem = null) => Item = initializedItem;

    /// <summary>
    /// Handles how the InventoryItem held in hand (or on the cursor) interacts with the InventorySlot when the slot UI gets clicked (interacted with), for example:
    /// Item in slot == empty, item in hand (parameter) == some item - The item in hand gets placed in the slot and empty gets returned
    /// Item in slot == some item 2, item in hand == some item 2 - The item in hand gets stacked together and now the item in the slot counts 4, etc.
    /// </summary>
    /// <param name="newItem">The InventoryItem to interact with (the item held in hand or held on cursor).</param>
    /// <returns>The swapped InventoryItem, or null if there isn't any.</returns>
    public InventoryItem Interact(InventoryItem newItem)
    {
        if (!IsItemValid(newItem))
        {
            Debug.LogWarning("Trying to add an empty item!");
            return null;
        }

        InventoryItem returnItem = Item;

        if (AreItemsTheSameAndStackable(Item, newItem))
        {
            returnItem = Item.AddAmount(newItem.Amount);
        }
        else
        {
            Item = newItem;
        }

        return returnItem;
    }

    /// <summary>Removes the InventoryItem held by this InventorySlot.</summary>
    /// <returns>The removed InventoryItem.</returns>
    public InventoryItem Remove()
    {
        InventoryItem returnItem = Item;
        Item = null;
        return returnItem;
    }

    /// <summary>Removes half of the InventoryItem held by this InventorySlot.</summary>
    /// <remarks>If the InventoryItem has an uneven number amount then the removed amount is always rounded up.</remarks>
    /// <returns>The InventoryItem removed.</returns>
    public InventoryItem RemoveHalf()
    {
        int amountRemoved = Mathf.CeilToInt(Item.Amount / 2f);
        InventoryItem returnItem = new(Item.Data, amountRemoved);
        Item.RemoveAmount(amountRemoved);
        return returnItem;
    }

    /// <summary>Places the provided InventoryItem in this InventorySlot regardless if it is occupied.</summary>
    /// <param name="newItem">The InventoryItem to place in this InventorySlot.</param>
    /// <returns>The InventoryItem that occupied this InventorySlot prior to replacing it if it was not empty, otherwise null.</returns>
    public InventoryItem Replace(InventoryItem newItem)
    {
        InventoryItem returnItem = Item;
        Item = newItem;
        return returnItem;
    }

    /// <summary>Gets the amount of free spaces available with the provided InventoryItem.</summary>
    /// <remarks>
    /// Example: InventoryItem in InventorySlot - amount = 4, max stack size = 10, returns 10-4=6 free spaces available.
    /// The provided InventoryItem is necessary to check if they are the same and can be stackable.
    /// </remarks>
    /// <param name="newItem">The InventoryItem for which we check how many free spaces are available.</param>
    /// <returns>The amount of free spaces available.</returns>
    public int GetFreeStackAmount(InventoryItem newItem)
    {
        if (IsEmpty) return newItem.Data.MaxStackSize;

        if (AreItemsTheSame(Item, newItem)) return Item.Data.MaxStackSize - Item.Amount;

        return 0;
    }

    void OnItemAmountChanged()
    {
        if (Item.Amount <= 0)
        {
            Item = null;
            return;
        }

        if (!Item.Data.IsStackable)
        {
            Debug.LogWarning("Changing item amount on a non-stackable item!");
            return;
        }

        OnItemUpdated?.Invoke();
    }
}
