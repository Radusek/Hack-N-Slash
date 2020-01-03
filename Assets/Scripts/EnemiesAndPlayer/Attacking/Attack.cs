using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField]
    protected AttackType attackType;

    [SerializeField]
    protected int damage = 5;

    [SerializeField]
    protected float recastInterval = 0.25f;

    private float lastAttackTime;

    [SerializeField]
    protected LayerMask targetLayers;

    [SerializeField]
    protected Transform firePoint;

    [SerializeField]
    private float attackRange = 0.5f;

    [SerializeField]
    protected bool isPlayer = false;

    private void Awake()
    {
        Initialize();
    }

    protected void Initialize()
    {
        lastAttackTime = Time.time;
    }

    private void Update()
    {
        if (EntityShouldAttack())
            TryToAttackTarget();
    }

    protected virtual bool EntityShouldAttack()
    {
        if (isPlayer)
            return Input.GetButtonDown("Fire1");

        return true;
    }

    private void TryToAttackTarget()
    {
        if (Time.time < lastAttackTime + recastInterval)
            return;

        Collider[] colliders = Physics.OverlapSphere(firePoint.position, attackRange, targetLayers);
        if (AttackTarget(colliders))
            lastAttackTime = Time.time;
    }

    // returns true if the attack could be successfully done
    protected virtual bool AttackTarget(Collider[] colliders)
    {
        Debug.Log("Override me");
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null)
            Gizmos.DrawWireSphere(firePoint.position, attackRange);
    }
}

public static class MyExtensions
{
    public static bool IsInLayerMask(this int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }
}

public enum AttackType
{
    Melee,
    Ranged,
    Fire
    // other to add later
}