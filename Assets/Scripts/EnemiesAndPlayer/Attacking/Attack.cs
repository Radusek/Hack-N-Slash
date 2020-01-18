using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    [SerializeField]
    protected WeaponItem weapon;

    protected AttackType attackType;

    protected int damage;

    protected float recastInterval;

    private float lastAttackTime;

    protected LayerMask targetLayers;

    [SerializeField]
    protected Transform firePoint;

    [SerializeField]
    protected float attackRange = 0.5f;

    protected bool isPlayer;

    private float equippingTime = 0.2f;

    public UnityEvent OnAttacked;

    private void Awake()
    {
        Initialize();
    }

    protected void Initialize()
    {
        UpdateWeaponStats();
        lastAttackTime = -recastInterval;
    }

    public void SetNewWeapon(WeaponItem newWeapon)
    {
        weapon = newWeapon;
        UpdateWeaponStats();
    }

    public virtual void UpdateWeaponStats()
    {
        attackType = weapon.attackType;
        damage = weapon.damage;
        recastInterval = weapon.recastInterval;
        EntitySoundIndex soundIndex = attackType == AttackType.Melee ? EntitySoundIndex.MeleeHit : EntitySoundIndex.RangedWeaponUse;
        GetComponent<SoundPlayer>().SetAudioClip(weapon.firingSound, soundIndex);
    }

    public WeaponItem GetWeaponItem()
    {
        return weapon;
    }

    private void OnEnable()
    {
        if (lastAttackTime == -recastInterval)
            return;

        lastAttackTime = Mathf.Max(Time.time - recastInterval + equippingTime, lastAttackTime);
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

    public float GetReloadingBarValue()
    {
        return 1f - (Time.time - lastAttackTime)/(recastInterval + 0.1f);
    }

    public float GetLastAttackTime()
    {
        return lastAttackTime;
    }

    public void ReduceCooldown(float time, float multiplier)
    {
        lastAttackTime += (1f - multiplier) * time;
    }

    public Sprite GetSprite()
    {
        return weapon.icon;
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