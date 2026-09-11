using System;

public class Health
{
    public event Action<float> OnMaxUpdated;
    public event Action<float> OnCurrentUpdated;
    public event Action OnDeath;

    public float Max
    {
        get => max;
        set
        {
            max = value;
            OnMaxUpdated?.Invoke(Max);
        }
    }

    public float Current
    {
        get => current;
        set
        {
            if (current == value) return;

            current = value;
            OnCurrentUpdated?.Invoke(value);

            if (Current == 0f)
                SetDeathState(true);
        }
    }

    public bool IsDead
    {
        get => isDead;
        set
        {
            isDead = value;
            OnDeath?.Invoke();
        }
    }

    float max;
    float current;
    bool isDead;

    readonly float minMaxHealth = 1f; // The minimum amount max health can be


    public Health(float maxHealth)
    {
        Max = maxHealth;
        Current = maxHealth;
    }

    /// <summary>Damages the Health by the amount provided.</summary>
    public void Damage(float amount)
    {
        if (isDead) return;

        if (amount <= 0f)
        {
            //Debug.LogWarning("Trying to damage health component with an invalid value!");
            return;
        }
        
        Current = Math.Max(0f, Current - amount);
    }

    /// <summary>Heals the Health by the amount provided.</summary>
    public void Heal(float amount)
    {
        if (isDead) return;

        if (amount <= 0f)
        {
            //Debug.LogWarning("Trying to heal health component with an invalid value!");
            return;
        }

        Current = Math.Min(Max, Current + amount);
    }

    /// <summary>
    /// Changes the current max health BY the amount provided, example:
    /// amount = 50, changes max health to max health + 50
    /// amount = -30, changes max health to max health - 30
    /// </summary>
    public void ChangeMax(float amount)
    {
        if (isDead) return;
        if (amount == 0f) return;

        Max = Math.Max(minMaxHealth, Max + amount);

        if (Current > Max)
        {
            Current = Max;
        }
    }

    /// <summary>Sets "isDead" directly to the value provided.</summary>
    public void SetDeathState(bool value) => IsDead = value;
}
