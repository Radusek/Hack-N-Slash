using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerController))]
public class KnightStats : EntityStats
{
    private WeaponManager wm;

    private bool usesShield;

    private float shieldUsageBlockTime = 0.35f;

    private PlayerController playerController;

    public BoolEvent OnShieldingStateChanged;

    public UnityEvent OnAttackBlocked;

    [SerializeField]
    private float shieldingMovementSpeedDecrease = 1.25f;


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

    protected override void SubtractHp(int amount, AttackType attackType, Vector3 attackPositionOrVelocity)
    {
        Vector3 knightToAttackDirection;

        if (attackType == AttackType.Melee)
            knightToAttackDirection = attackPositionOrVelocity - transform.position;
        else
            knightToAttackDirection = -attackPositionOrVelocity;

        bool attackedFromFront = knightToAttackDirection.AngleDegreesBetween(transform.forward) < 60f;

        if (attackedFromFront)
        {
            if (usesShield && Random.Range(0f, 1f) < 0.85f)
            {
                OnAttackBlocked?.Invoke();
                return;
            }
        }

        base.SubtractHp(amount, attackType, attackPositionOrVelocity);
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

    public static float AngleRadsBetween(this Vector3 lhs, Vector3 rhs)
    {
        float cosine = Vector3.Dot(lhs.normalized, rhs.normalized);
        return Mathf.Acos(cosine);
    }
}