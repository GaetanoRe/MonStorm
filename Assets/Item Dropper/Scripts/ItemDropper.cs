using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class ItemDropper : MonoBehaviour
{
    [Serializable] class InspectorItem
    {
        public InventoryItemDataEXAMPLE data;
        public int minAmount = 1;
        public int maxAmount = 1;
        [Range(0f, 1f)] public float percentChance = 1f;
    }

    [SerializeField] List<InspectorItem> items;

    DroppedItemEXAMPLE prefab;


    void Awake()
    {
        // This is only a temporary way to load an asset and works in the editor only
        string path = "Assets/Item Dropper/Prefabs/Dropped Item Default Prefab.prefab";
        prefab = AssetDatabase.LoadAssetAtPath<DroppedItemEXAMPLE>(path);
    }
    
    public void Activate()
    {
        foreach (var item in items)
        {
            if (UnityEngine.Random.Range(0f, 1f) > item.percentChance) continue;

            // Max amount should never be smaller than min amount
            item.maxAmount = Mathf.Max(item.minAmount, item.maxAmount);

            int amount = UnityEngine.Random.Range(item.minAmount, item.maxAmount + 1);

            DroppedItemEXAMPLE droppedItem = Instantiate(prefab);
            droppedItem.Initialize(new InventoryItem(item.data, amount));
        }
    }
}
