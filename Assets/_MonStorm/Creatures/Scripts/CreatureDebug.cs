using UnityEngine;
using MonStorm.Core.StateMachine;

public class CreatureDebug : MonoBehaviour
{
    [SerializeField] bool logOnBeingHit;
    [SerializeField] bool logOnDamageTaken;
    [SerializeField] bool logOnStateTransition;

    CreatureBehavior creatureBehavior;


    void Awake()
    {
        if (!TryGetComponent(out creatureBehavior))
        {
            Debug.LogWarning("CreatureBehavior script must be attached for CreatureDebug to work!");
        }
    }

    void Start()
    {
        if (creatureBehavior == null) return;

        creatureBehavior.OnHit += LogHit;
        creatureBehavior.OnDamaged += LogDamaged;
        creatureBehavior.StateMachine.OnStateChanged += LogStateTransition;
    }

    void OnDestroy()
    {
        if (creatureBehavior == null) return;

        creatureBehavior.OnHit -= LogHit;
        creatureBehavior.OnDamaged -= LogDamaged;
        creatureBehavior.StateMachine.OnStateChanged -= LogStateTransition;
    }

    void LogHit(HitApplierComponentConfigured applier, HitDetectorComponentConfigured detector)
    {
        if (!logOnBeingHit) return;

        Debug.Log($"{detector.gameObject} got hit by {applier.gameObject}.");
    }

    void LogDamaged(HealthComponentConfigured health, float damageDealt)
    {
        if (!logOnDamageTaken) return;

        Debug.Log($"{health.gameObject} got damaged.\nDamage dealt: {damageDealt}, remaining health: {health.Data.Current}.");
    }

    void LogStateTransition(IState<CreatureContext> stateFrom, IState<CreatureContext> stateTo)
    {
        if (!logOnStateTransition) return;

        Debug.Log($"{gameObject} state changed.\n{stateFrom} -> {stateTo}.");
    }
}
