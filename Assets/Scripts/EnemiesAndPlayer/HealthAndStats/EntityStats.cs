using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityStats : MonoBehaviour
{
    [SerializeField]
    protected int maxHealth = 50;
    protected int currentHealth;

    [SerializeField]
    private bool destroyOnDeath = true;

    // some resistances stats and other cool things to add

    public UnityEvent OnHpChanged;
    public UnityEvent OnDeath;

    private void Awake()
    {
        Initialize();
    }

    protected void Initialize()
    {
        currentHealth = maxHealth;
    }

    public float GetHpFraction()
    {
        return (float)currentHealth / maxHealth;
    }

    public virtual void TakeDamage(int amount, AttackType attackType)
    {
        bool shouldDie = SubtractHpAndCheckIfShouldDie(amount, attackType);
        if (shouldDie)
            Die();
    }

    protected bool SubtractHpAndCheckIfShouldDie(int amount, AttackType attackType)
    {
        // take different amount of damage based on the attackType to add
        currentHealth -= amount;
        OnHpChanged?.Invoke();
        if (currentHealth <= 0)
            OnDeath?.Invoke();
        Debug.Log($"{gameObject.name}'s health is now {currentHealth}.");

        return currentHealth <= 0;
    }

    protected void Die()
    {
        if (destroyOnDeath)
            Destroy(gameObject);
        else
        {
            if (gameObject.layer == (int)Layer.Player)
                DisablePlayer();
        }
    }

    private void DisablePlayer()
    {
        Attack[] attacks = GetComponents<Attack>();
        foreach (var attack in attacks)
            attack.enabled = false;
        GetComponent<PlayerController>().enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }
}