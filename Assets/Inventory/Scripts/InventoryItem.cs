using UnityEngine;
using System;

public class InventoryItem
{
    public event Action OnAmountUpdated;
    public IInventoryItemData Data { get; private set; }
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

    // Adds an amount if the item is stackable
    // returns an InventoryItem with the remainder amount if added amount exceeds MaxStackSize, else returns null
    public InventoryItem AddAmount(int amountToAdd)
    {
        if (!Data.IsStackable || amountToAdd <= 0) return null;

        int remainder = Mathf.Max(0, amountToAdd + Amount - Data.MaxStackSize);
        Amount = Mathf.Min(Amount + amountToAdd, Data.MaxStackSize);
        return remainder == 0 ? null : new(Data, remainder);
    }

    // Returns true if the amount was successfully removed, false otherwise
    public bool RemoveAmount(int amountToRemove)
    {
        if (amountToRemove <= 0) return false;

        if (amountToRemove > Amount) return false;

        Amount -= amountToRemove;
        return true;
    }
}
