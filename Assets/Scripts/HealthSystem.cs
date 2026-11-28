using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour, IDamageable
{
    [Header("Variables públicas")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Eventos")]
    public UnityEvent<int> onHealthChanged;
    public UnityEvent onDeath;
    public UnityEvent onTakeDamage;

    [Header("Invencibilidad")]
    public float invincibleTime = 1f;
    private float invincibleTimer = 0f;
    public bool isInvincible => invincibleTimer > 0f;

    void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth);
    }

    void Update()
    {
       if (invincibleTimer > 0f) invincibleTimer -= Time.deltaTime;
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;
        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);
        invincibleTimer = invincibleTime;
        onHealthChanged?.Invoke(currentHealth);
        onTakeDamage?.Invoke();
        if (currentHealth <= 0) onDeath?.Invoke();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        onHealthChanged?.Invoke(currentHealth);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        invincibleTimer = 0f;
        onHealthChanged?.Invoke(currentHealth);
    }
}