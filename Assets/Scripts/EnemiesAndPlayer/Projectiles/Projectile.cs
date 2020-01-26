using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    protected GameObject projectileCaster; //change to ranged attack and check for targetLayer to be destroyed, enable enemyProjectile and enemy collisions

    private int damage;
    protected LayerMask targetLayers;

    [SerializeField]
    protected AttackType attackType;

    [SerializeField]
    private bool gravityAffected;

    [SerializeField]
    [Range(0f, 1f)]
    private float gravityScale = 0.5f;

    private float firstDistanceTreshold = 1.75f;
    private float secondDistanceTreshold = 5.5f;

    private float projectileDestructionSqrDistance = 15f * 15f;

    private Vector3 startPosition;

    private Rigidbody rb;

    public UnityEvent OnHit;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = gravityAffected;
    }

    public void Initialize(int dmg, LayerMask layers, GameObject caster)
    {
        damage = dmg;
        targetLayers = layers;
        projectileCaster = caster;
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (gravityAffected)
        {
            rb.AddForce(-(1f - gravityScale) * Physics.gravity, ForceMode.Acceleration);
            transform.LookAt(transform.position + rb.velocity);
        }

        if ((startPosition - transform.position).sqrMagnitude > projectileDestructionSqrDistance)
            Hit(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == projectileCaster || other.isTrigger || other.gameObject.layer == projectileCaster.layer)
            return;

        if (other.gameObject.layer.IsInLayerMask(targetLayers))
            InteractWithTarget(other);

        Hit();
    }

    protected virtual void InteractWithTarget(Collider other)
    {
        other.GetComponent<EntityStats>().TakeDamage(GetCurrentDamage(), attackType, projectileCaster, transform.position);
    }

    protected virtual void Hit(bool forceDestroy = false)
    {
        OnHit?.Invoke();
    }

    protected int GetCurrentDamage()
    {
        float minDamageFraction = 0.5f;
        float distance = (startPosition - transform.position).magnitude;

        if (distance >= secondDistanceTreshold)
            return damage;

        if (distance <= firstDistanceTreshold)
            return (int)(minDamageFraction * damage);

        float damageMultiplier = minDamageFraction + (1f - minDamageFraction) * (distance - firstDistanceTreshold) / (secondDistanceTreshold - firstDistanceTreshold);
        return (int)(damageMultiplier * damage);
    }
}
