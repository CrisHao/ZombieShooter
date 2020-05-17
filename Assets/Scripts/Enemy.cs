using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float deathDuration = 1f;

    Health m_Health;
    float m_LastTimeDamaged = float.NegativeInfinity;



    // Start is called before the first frame update
    void Start()
    {
        m_Health = GetComponent<Health>();
        m_Health.onDamaged += OnDamaged;
        m_Health.onDie += OnDie;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Damaged Event 
    /// </summary>
    /// <param name="damage">damager number</param>
    /// <param name="damageSource">damage Source</param>
    void OnDamaged(float damage, GameObject damageSource)
    {
        // test if the damage source is the player
        if (damageSource && damageSource.GetComponent<PlayerCharacterControl>())
        {

            //animator.SetTrigger(k_AnimOnDamagedParameter);

        }
    }



    void OnDie()
    {

        // tells the game flow manager to handle the enemy destuction
        //m_EnemyManager.UnregisterEnemy(this);

        // loot an object
        //if (TryDropItem())
        //{
        //    Instantiate(lootPrefab, transform.position, Quaternion.identity);
        //}

        // this will call the OnDestroy function
        Destroy(gameObject, deathDuration);
    }
}
