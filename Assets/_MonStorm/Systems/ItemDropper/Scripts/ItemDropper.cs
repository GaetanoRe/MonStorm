using System;
using System.Collections.Generic;

public class ItemDropper
{
    // Configures an InventoryItemData that can be selected by the ItemDropper.
    [Serializable] public class LootTableItem
    {
        public InventoryItemData data;
        public int minAmount = 1;
        public int maxAmount = 1;
        public float chance = 1f; // 0f through 1f, 0f = 0%, 1f = 100%
    }

    // List of LootTableItems which can be drawn from.
    readonly List<LootTableItem> lootTable;

    readonly Random random = new();


    /// <summary>Initialize the component with the provided list of items it will draw from.</summary>
    /// <param name="lootTable">The loot table this component will draw from.</param>
    public ItemDropper(List<LootTableItem> lootTable)
    {
        this.lootTable = lootTable;

        foreach (LootTableItem item in this.lootTable)
        {
            // Max amount should never be smaller than min amount, so this is only for precaution.
            item.maxAmount = Math.Max(item.minAmount, item.maxAmount);
        }
    }

    /// <returns>A list of random InventoryItems from the loot table.</returns>
    public List<InventoryItem> GetDrops()
    {
        List<InventoryItem> returnItems = new();
        foreach (LootTableItem item in lootTable)
        {
            if (random.NextDouble() > item.chance) continue;

            int amount = random.Next(item.minAmount, item.maxAmount + 1);
            returnItems.Add(new(item.data, amount));
        }

        return returnItems;
    }
}
