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

    private int experiencePoints = 0;
    private int level = 1;

    [SerializeField]
    private int baseExp = 105;

    [SerializeField]
    private int startingLevel = 1;

    // some resistances stats and other cool things to add

    [SerializeField]
    private float fadingDepth = 1.2f;

    [SerializeField]
    private GameObject minimapMark;

    public UnityEvent OnHpChanged;
    public IntEvent OnDamageAmountTaken;
    public UnityEvent OnDeath;

    public UnityEvent OnExpGained;
    public UnityEvent OnLevelUp;

    private int expPerLevel = 1000;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        InitializeStart();
    }

    protected void Initialize()
    {
        currentHealth = maxHealth;
    }

    protected void InitializeStart()
    {
        experiencePoints = (startingLevel - 1) * expPerLevel;
        level = startingLevel;
    }

    public void SetStartingLevel(int number)
    {
        startingLevel = number;
    }

    public int GetLevel()
    {
        return (experiencePoints / expPerLevel) + 1;
    }

    public void AddExperience(int amount)
    {
        if (amount <= 0)
            return;

        experiencePoints += amount;

        int updatedLevel = GetLevel();
        if (updatedLevel > level)
        {
            level = updatedLevel;
            OnLevelUp?.Invoke();
        }

        OnExpGained?.Invoke();
    }

    public float GetHpFraction()
    {
        return (float)currentHealth / maxHealth;
    }

    public Vector2Int GetHpValues()
    {
        return new Vector2Int(currentHealth, maxHealth);
    }

    public float GetExpFraction()
    {
        return (float)(experiencePoints - (level - 1) * expPerLevel) / expPerLevel;
    }

    public void TakeDamage(int amount, AttackType attackType, GameObject attacker, Vector3 attackPosition)
    {
        int oldCurrentHp = currentHealth;

        SubtractHp(amount, attackType, attackPosition);

        if (currentHealth != oldCurrentHp)
        {
            OnHpChanged?.Invoke();
            OnDamageAmountTaken?.Invoke(oldCurrentHp - currentHealth);
        }

        if (currentHealth <= 0)
        {
            if (attacker != null)
                RewardAttacker(attacker);
            Die();
        }
    }

    protected virtual void SubtractHp(int amount, AttackType attackType, Vector3 attackPosition)
    {
        // take different amount of damage based on the attackType to add
        currentHealth -= amount;
    }

    private void RewardAttacker(GameObject attacker)
    {
        EntityStats attackerStats = attacker.GetComponent<EntityStats>();
        int attackerLevel = attackerStats.GetLevel();
        float nerfValue = 0.16f;
        int levelsDifference = Mathf.Abs(attackerLevel - level);
        int levelDiffMultiplier = levelsDifference <= 2 ? 0 : levelsDifference - 2;
        int finalExp = (int)(baseExp * Mathf.Clamp01(1f - levelDiffMultiplier * nerfValue));
        attackerStats.AddExperience(finalExp);
    }

    protected void Die()
    {
        OnDeath?.Invoke();

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
        GetComponent<WeaponManager>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    private void DisableEnemy()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        Destroy(GetComponent<EnemyController>());
        minimapMark.SetActive(false);
    }

    protected virtual void DisablePlayer()
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

[System.Serializable]
public class IntEvent : UnityEvent<int> { }