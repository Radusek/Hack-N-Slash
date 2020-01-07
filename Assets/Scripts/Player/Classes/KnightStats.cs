using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerController))]
public class KnightStats : EntityStats
{
    private WeaponManager wm;

    private bool usesShield;

    private float shieldUsageBlockTime = 0.6f;

    private PlayerController playerController;

    private float shieldingMovementSpeedDecrease = 2f;

    public BoolEvent OnShieldingStateChanged;

    private void Awake()
    {
        base.Initialize();
        wm = GetComponent<WeaponManager>();
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        InitializeStart();
    }

    void Update()
    {
        if (Input.GetButton("Fire2") != usesShield)
        {
            if (usesShield || Time.time >= wm.GetLastWeaponUsageTime() + shieldUsageBlockTime)
            {
                usesShield = !usesShield;
                OnShieldingStateChanged?.Invoke(usesShield);
                wm.SetCanAttack(!usesShield);
                playerController.AddMovementSpeed(usesShield ? -shieldingMovementSpeedDecrease : shieldingMovementSpeedDecrease);
            }
        }
    }

    protected override void SubtractHp(int amount, AttackType attackType)
    {
        if (usesShield && Random.Range(0f, 1f) < 0.8f)
            return;

        base.SubtractHp(amount, attackType);
    }

    protected override void DisablePlayer()
    {
        base.DisablePlayer();
        this.enabled = false;
    }
}
