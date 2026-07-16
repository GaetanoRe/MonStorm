using UnityEngine;
using System;
using System.Collections.Generic;

public class ItemDropper : MonoBehaviour
{
    // Class used to configure the loot table in the inspector.
    [Serializable] class InspectorItem
    {
        public InventoryItemData data;
        public int minAmount = 1;
        public int maxAmount = 1;
        [Range(0f, 1f)] public float percentChance = 1f;
    }

    // List of all items in this ItemDropper's loot table
    [SerializeField] List<InspectorItem> items;

    // The game object that spawns, which holds the item data
    // In the future it can also hold the mesh associated with the item and whatever else is necessary
    DroppedItem prefab;

    readonly float yOffset = 1f;


    /// <summary>Activates the ItemDropper to drop it's loot table.</summary>
    public void Activate()
    {
        foreach (InspectorItem item in items)
        {
            if (UnityEngine.Random.Range(0f, 1f) > item.percentChance) continue;

            int amount = UnityEngine.Random.Range(item.minAmount, item.maxAmount + 1);

            DroppedItem droppedItem = Instantiate(prefab, transform.position + new Vector3(0f, yOffset, 0f), Quaternion.identity);
            droppedItem.Initialize(new InventoryItem(item.data, amount));
        }
    }

    void Awake()
    {
        string path = "Prefabs/Dropped Item Default Prefab";
        prefab = Resources.Load<DroppedItem>(path);

        foreach (InspectorItem item in items)
        {
            // Max amount should never be smaller than min amount, so this is only for precaution.
            item.maxAmount = Mathf.Max(item.minAmount, item.maxAmount);
        }
    }
}
