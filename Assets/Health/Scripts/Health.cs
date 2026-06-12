using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public Action<float> OnCurrentHealthUpdated;
    public Action<float> OnMaxHealthUpdated;
    public Action OnDeath;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDead
    {
        get => isDead;
        set
        {
            isDead = value;
            OnDeath?.Invoke();
        }
    }

    [SerializeField] float maxHealth = 100f;
    [SerializeField] bool isMaxHealthChangeable = true;

    float currentHealth;
    bool isDead;

    readonly float minMaxHealth = 1f; // The minimum amount max health can be


    public void Damage(float amount)
    {
        if (isDead) return;

        if (amount <= 0f)
        {
            Debug.LogWarning("Trying to damage health component with an invalid value!");
            return;
        }

        currentHealth = Mathf.Max(0f, currentHealth - amount);
        OnCurrentHealthUpdated?.Invoke(currentHealth);

        if (currentHealth == 0f)
            SetDeathState(true);
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        if (amount <= 0f)
        {
            Debug.LogWarning("Trying to heal health component with an invalid value!");
            return;
        }

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnCurrentHealthUpdated?.Invoke(currentHealth);
    }


    /// <summary>
    /// Changes the current max health BY the amount provided, example:
    /// amount = 50, changes max health to max health + 50
    /// amount = -30, changes max health to max health - 30
    /// </summary>
    public void ChangeMaxHealth(float amount)
    {
        if (isDead) return;
        if (amount == 0f) return;

        if (!isMaxHealthChangeable)
        {
            Debug.LogWarning("Trying to change max health when it is not changeable!");
            return;
        }

        maxHealth = Mathf.Max(minMaxHealth, maxHealth + amount);
        OnMaxHealthUpdated?.Invoke(maxHealth);

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            OnCurrentHealthUpdated?.Invoke(currentHealth);
        }
    }

    /// <summary>
    /// Sets "isDead" directly to the value provided
    /// </summary>
    public void SetDeathState(bool value) => IsDead = value;

    void Awake()
    {
        currentHealth = maxHealth;
    }
}
