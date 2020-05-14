using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public Health health { get; private set; }

    void Awake()
    {
        // find the health component either at the same level, or higher in the hierarchy
        health = GetComponent<Health>();
        if (!health)
        {
            health = GetComponentInParent<Health>();
        }
    }

    public void InflictDamage(float damage, GameObject damageSource)
    {

        if (health)
        {
            var totalDamage = damage;

            health.TakeDamage(totalDamage, damageSource);
        }
    }
}
