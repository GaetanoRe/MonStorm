using UnityEngine;
using System.Collections.Generic;

public class ItemDropperComponentStandalone : ItemDropperComponentConfigured
{
    [SerializeField] List<ItemDropper.LootTableItem> lootTable;


    void Awake()
    {
        Initialize(new(lootTable));
    }
}
