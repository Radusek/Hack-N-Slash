using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    [SerializeField]
    protected AttackType attackType;

    [SerializeField]
    protected int damage = 5;

    [SerializeField]
    protected float recastInterval = 0.25f;

    private float lastAttackTime;

    protected LayerMask targetLayers;

    [SerializeField]
    protected Transform firePoint;

    [SerializeField]
    protected float attackRange = 0.5f;

    protected bool isPlayer;

    private float disabledTime;
    private float equippingTime = 0.2f;

    public UnityEvent OnAttacked;

    private void Awake()
    {
        Initialize();
    }

    protected void Initialize()
    {
        lastAttackTime = -recastInterval;
    }

    private void OnDisable()
    {
        disabledTime = Time.time;
    }

    private void OnEnable()
    {
        if (lastAttackTime == -recastInterval)
            return;

        lastAttackTime = Mathf.Max(Time.time - recastInterval + equippingTime, Time.time - (disabledTime - lastAttackTime));
    }

    public void SetIsPlayer(bool itIsPlayer)
    {
        isPlayer = itIsPlayer;
    }

    public void SetTargetLayers(LayerMask layers)
    {
        targetLayers = layers;
    }

    public void TryToAttackTarget()
    {
        if (Time.time < lastAttackTime + recastInterval)
            return;

        Collider[] colliders = Physics.OverlapSphere(firePoint.position, attackRange, targetLayers);
        if (AttackTarget(colliders))
        {
            lastAttackTime = Time.time;
            OnAttacked?.Invoke();
        }
    }

    // returns true if the attack could be successfully done
    protected virtual bool AttackTarget(Collider[] colliders)
    {
        OnAttacked?.Invoke();
        Debug.Log("Override me");
        return false;
    }

    public float GetReloadingTimeLeftFraction()
    {
        return 1f - (Time.time - lastAttackTime)/(recastInterval + 0.1f);
    }

    public float GetLastAttackTime()
    {
        return lastAttackTime;
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