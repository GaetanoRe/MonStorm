using UnityEngine;
using System.Collections.Generic;
using MonStorm.Core.StateMachine;

public abstract class CreatureDefinition : ScriptableObject
{
    [field: Header("Creature Properties"), SerializeField] public string Name { get; private set; }
    [field: SerializeField] public float MaxHealth { get; private set; }

    [field: Header("Loot Table"), SerializeField] public List<ItemDropper.LootTableItem> LootTable { get; private set; }


    /// <summary>Create the specific creature state machine with all of it's states, values and transitions.</summary>
    /// <param name="creatureContext">The CreatureContext associated with the state machine.</param>
    /// <param name="creatureBehavior">The CreatureBehavior associated with the state machine.</param>
    /// <returns>The built state machine.</returns>
    public abstract StateMachine<CreatureContext> BuildStateMachine(CreatureContext creatureContext, CreatureBehavior creatureBehavior);
}
