using UnityEngine;
using MonStorm.Core.StateMachine;

public abstract class CreatureDefinition : ScriptableObject
{
    /// <summary>The display name of the creature.</summary>
    [field: SerializeField] public string CreatureName { get; private set; }
    /// <summary>The amount of damage required for the creature to get staggered.</summary>
    [field: SerializeField] public float StaggerDamageThreshold { get; private set; }
    /// <summary>How far away a creature can detect it's target.</summary>
    [field: SerializeField] public float VisionRadius { get; private set; }
    /// <summary>The creature's vision angle, starting from the creature's forward direction.</summary>
    /// <remarks>A value of 10f means 10 degrees on each side (left and right), 180f means full vision all around the creature (whole circle).</remarks>
    [field: SerializeField, Range(0f, 180f)] public float VisionMaxAngle { get; private set; }


    /// <summary>Create the specific creature state machine with all of it's states, values and transitions.</summary>
    /// <param name="creatureContext">The CreatureContext associated with the state machine.</param>
    /// <param name="creatureBehavior">The CreatureBehavior associated with the state machine.</param>
    /// <returns>The built state machine.</returns>
    public abstract StateMachine<CreatureContext> BuildStateMachine(CreatureContext creatureContext, CreatureBehavior creatureBehavior);
}
