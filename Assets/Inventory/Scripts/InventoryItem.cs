using UnityEngine;
using System;

public class InventoryItem
{
    /// <summary>Invoked when the amount of the held InventoryItem changes.</summary>
    public event Action OnAmountUpdated;

    /// <summary>The data associated with this InventoryItem.</summary>
    public IInventoryItemData Data { get; private set; }

    /// <summary>The amount of an InventoryItem held.</summary>
    public int Amount
    {
        get => amount;
        private set
        {
            amount = value;
            OnAmountUpdated?.Invoke();
        }
    }

    int amount;


    /// <summary>Initializes the InventoryItem with the provided data and amount.</summary>
    public InventoryItem(IInventoryItemData data, int amount)
    {
        if (amount < 1)
        {
            Debug.LogWarning($"Item amount can't be less than 1!\nItem name: {data.Name}");
            amount = 1;
        }
        if (amount > data.MaxStackSize)
        {
            Debug.LogWarning($"Item amount can't be larger than it's max stack size!\nItem name: {data.Name}");
            amount = data.MaxStackSize;
        }
        if (!data.IsStackable && amount > 1)
        {
            Debug.LogWarning($"Item amount can't be larger than 1 if the item is not stackable!\nItem name: {data.Name}");
            amount = 1;
        }

        Data = data;
        Amount = amount;
    }

    /// <summary>Adds the provided amount to the InventoryItem.</summary>
    /// <param name="amountToAdd">The amount to add.</param>
    /// <returns>An InventoryItem with the remainder amount if added amount exceeds MaxStackSize, null otherwise.</returns>
    public InventoryItem AddAmount(int amountToAdd)
    {
        if (!Data.IsStackable || amountToAdd <= 0) return null;

        int remainder = Mathf.Max(0, amountToAdd + Amount - Data.MaxStackSize);
        Amount = Mathf.Min(Amount + amountToAdd, Data.MaxStackSize);
        return remainder == 0 ? null : new(Data, remainder);
    }

    /// <summary>Removes the provided amount from the InventoryItem.</summary>
    /// <param name="amountToRemove">The amount to remove.</param>
    /// <returns>True if the amount was successfully removed, false otherwise.</returns>
    public bool RemoveAmount(int amountToRemove)
    {
        if (amountToRemove <= 0) return false;

        if (amountToRemove > Amount) return false;

        Amount -= amountToRemove;
        return true;
    }
}
