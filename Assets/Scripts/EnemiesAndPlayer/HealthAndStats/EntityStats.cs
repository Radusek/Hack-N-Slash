using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EntityStats : MonoBehaviour
{
    [SerializeField]
    protected int maxHealth = 50;
    protected int currentHealth;

    // some resistances stats and other cool things to add

    [SerializeField]
    private float fadingDepth = 1.2f;

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

        return currentHealth <= 0;
    }

    protected void Die()
    {
        PrepareDisabling();

        if (gameObject.GetComponent<PlayerController>() != null)
            DisablePlayer();
        else
        {
            DisableEnemy();
            StartCoroutine(FadeAndDestroyCoroutine());
        }
    }

    private void PrepareDisabling()
    {
        gameObject.layer = (int)Layer.Dead;
        Attack[] attacks = GetComponents<Attack>();
        foreach (var attack in attacks)
            attack.enabled = false;
        WeaponManager wm = GetComponent<WeaponManager>();
        if (wm != null)
            wm.enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    private void DisableEnemy()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        Destroy(GetComponent<EnemyController>());
    }

    private void DisablePlayer()
    {
        GetComponent<PlayerController>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private IEnumerator FadeAndDestroyCoroutine()
    {
        yield return new WaitForSeconds(5f);

        float fadingTime = 4f * fadingDepth;
        float startTime = Time.time;
        while (Time.time < startTime + fadingTime)
        {
            transform.position = transform.position + fadingDepth * Vector3.down / fadingTime * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}