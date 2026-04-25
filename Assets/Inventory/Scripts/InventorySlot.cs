using UnityEngine;
using System;
using static Inventory;

public class InventorySlot
{
    public event Action OnItemUpdated;
    public bool IsEmpty => Item == null;
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

    InventoryItem item;


    public InventorySlot(InventoryItem initializedItem = null) => Item = initializedItem;

    // Handles how item held in hand interacts with inventory slot when slot is clicked (interacted with), for example:
    // Item in slot == empty, item in hand (parameter) == some item - The item in hand gets placed in the slot and empty gets returned
    // Item in slot == some item 2, item in hand == some item 2 - The item in hand gets stacked together and now the item in the slot counts 4, etc.
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

    public InventoryItem Remove()
    {
        InventoryItem returnItem = Item;
        Item = null;
        return returnItem;
    }

    public InventoryItem RemoveHalf()
    {
        int amountRemoved = Mathf.CeilToInt(Item.Amount / 2f);
        InventoryItem returnItem = new(Item.Data, amountRemoved);
        Item.RemoveAmount(amountRemoved);
        return returnItem;
    }

    public InventoryItem Replace(InventoryItem newItem)
    {
        InventoryItem returnItem = Item;
        Item = newItem;
        return returnItem;
    }

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
