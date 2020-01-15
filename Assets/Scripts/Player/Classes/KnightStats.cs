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

    protected override void SubtractHp(int amount, AttackType attackType, Vector3 attackPosition)
    {
        Vector3 knightToAttackDirection = attackPosition - transform.position;
        bool attackedFromFront = knightToAttackDirection.AngleDegreesBetween(transform.forward) < 60f;
        if (attackedFromFront)
        {
            if (usesShield && Random.Range(0f, 1f) < 0.8f)
                return;
        }

        base.SubtractHp(amount, attackType, attackPosition);
    }

    public override void EnablePlayer(bool enabled)
    {
        base.EnablePlayer(enabled);
        this.enabled = enabled;
    }
}

public static class MyVectorExtensions
{
    public static float AngleDegreesBetween(this Vector3 lhs, Vector3 rhs)
    {
        float cosine = Vector3.Dot(lhs.normalized, rhs.normalized);
        return Mathf.Acos(cosine) * Mathf.Rad2Deg;
    }
}