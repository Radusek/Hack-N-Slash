﻿using System.Collections;
using UnityEngine;

public class StatsRegen : MonoBehaviour
{
    private EntityStats playerStats;

    [SerializeField]
    private float baseHpRegenerationRate;
    [SerializeField]
    private float baseManaRegenerationRate;

    private float baseHpRegenRateWithStats;
    private float baseManaRegenRateWithStats;

    private float accumulatedHp;
    private float accumulatedMana;

    private float hpRegenerationRate;
    private float manaRegenerationRate;

    private bool healingEnabled = true;

    private void Awake()
    {
        playerStats = GetComponent<EntityStats>();
        SetNewBaseRegeneration();
    }

    private void Update()
    {
        if (!healingEnabled)
            return;

        accumulatedHp +=  (baseHpRegenRateWithStats + hpRegenerationRate) * Time.deltaTime;
        accumulatedMana += (baseManaRegenRateWithStats + manaRegenerationRate) * Time.deltaTime;

        if (Mathf.Abs(accumulatedHp) < 1f && Mathf.Abs(accumulatedMana) < 1f)
            return;

        RestoreEntityStats();
    }

    private void RestoreEntityStats()
    {
        int hpToAdd = (int)accumulatedHp;
        accumulatedHp = accumulatedHp - (int)accumulatedHp;

        int manaToAdd = (int)accumulatedMana;
        accumulatedMana = accumulatedMana - (int)accumulatedMana;

        playerStats.Regenerate(hpToAdd, manaToAdd);
    }

    public void IncreaseRegeneration(float hpRegenRate, float manaRegenRate, float duration)
    {
        StartCoroutine(RegenEffectCoroutine(hpRegenRate, manaRegenRate, duration));
    }

    private IEnumerator RegenEffectCoroutine(float hpRegenRate, float manaRegenRate, float duration)
    {
        hpRegenerationRate += hpRegenRate;
        manaRegenerationRate += manaRegenRate;
        yield return new WaitForSeconds(duration + 0.01f);

        hpRegenerationRate -= hpRegenRate;
        manaRegenerationRate -= manaRegenRate;
    }

    public void OnDeath()
    {
        StopAllCoroutines();
        hpRegenerationRate = 0f;
        manaRegenerationRate = 0f;

        healingEnabled = false;
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(3f);
        healingEnabled = true;
    }

    public void SetNewBaseRegeneration()
    {
        baseHpRegenRateWithStats = baseHpRegenerationRate + playerStats.GetVitality() * 0.01f;
        baseManaRegenRateWithStats = baseManaRegenerationRate + playerStats.GetEnergy()  * 0.02f;
    }
}
