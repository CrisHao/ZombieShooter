using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth = 10f;

    //Event
    public UnityAction<float, GameObject> onDamaged;
    public UnityAction onDie;


    public float currentHealth { get; set; }
    public bool invincible { get; set; }

    bool m_IsDead;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage, GameObject damageSource)
    {
        if (invincible)
            return;

        float healthBefore = currentHealth;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        float trueDamageAmount = healthBefore - currentHealth;
        if (trueDamageAmount > 0f && onDamaged != null)
        {
            onDamaged.Invoke(trueDamageAmount, damageSource);
        }

        HandleDeath();
    }

    private void HandleDeath()
    {
        if (m_IsDead)
            return;

        // call OnDie action
        if (currentHealth <= 0f)
        {
            if (onDie != null)
            {
                m_IsDead = true;
                onDie.Invoke();
            }
        }
    }
}
