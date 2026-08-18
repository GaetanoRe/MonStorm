using UnityEngine;
using System;
using System.Collections.Generic;
using MonStorm.Core.StateMachine;

public abstract class CreatureDefinition : ScriptableObject
{
    [SerializeField] string creatureName;
    //[SerializeField] GameObject prefab;
    //[SerializeField] List<InspectorItem> lootDropTableItems;

    // Class used to configure the loot drop table in the inspector.
    [Serializable] class InspectorItem
    {
        public InventoryItemData data;
        public int minAmount = 1;
        public int maxAmount = 1;
        [Range(0f, 1f)] public float percentChance = 1f;
    }

    public abstract StateMachine<CreatureContext> BuildStateMachine(CreatureContext creatureContext, CreatureBehavior creatureBehavior);
}
