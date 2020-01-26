using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class RangedProjectileAttack : Attack
{
    private PoolType projectileType;

    private float projectileSpeed;

    private bool targetIsVisible;
    public BoolEvent OnTargetVisibilityChange;

    private Rigidbody rb;
    private Rigidbody targetRb;

    private PlayerController playerController;

    private void Awake()
    {
        base.Initialize();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (isPlayer)
            playerController = GetComponent<PlayerController>();
    }

    public override void UpdateWeaponStats()
    {
        base.UpdateWeaponStats();
        projectileType = weapon.projectileType;
        projectileSpeed = weapon.projectileSpeed;
    }

    protected override bool AttackTarget(Collider[] colliders)
    {
        if (isPlayer)
        {
            Vector3 direction = playerController.GetLookingPosition() - firePoint.position;
            direction.y = 0;
            LaunchProjectile(direction.normalized);
            return true;
        }

        if (colliders.Length == 0)
            return false;

        foreach(var target in colliders)
        {
            if (target.gameObject == gameObject)
                continue;

            if ((targetRb == null) || targetRb.gameObject != target.gameObject)
                targetRb = target.GetComponent<Rigidbody>();

            Vector3 shootingDirection = GetInterceptCourseDirection(target.transform.position, targetRb.velocity, firePoint.position, projectileSpeed);
            if (shootingDirection == Vector3.zero)
                continue;

            RaycastHit hit;
            int visibilityMask = ~(1 << gameObject.layer | 1 << (int)Layer.Interactable);
            if (Physics.Raycast(firePoint.position - firePoint.forward, shootingDirection, out hit, attackRange + 1f, visibilityMask))
            {
                bool oldVisibilty = targetIsVisible;
                targetIsVisible = hit.collider.gameObject.layer.IsInLayerMask(targetLayers);
                if (targetIsVisible != oldVisibilty)
                    OnTargetVisibilityChange?.Invoke(targetIsVisible);
            }

            if (!targetIsVisible)
                continue;

            LaunchProjectile(shootingDirection.normalized);
            return true;
            
        }
        return false;
    }

    private void LaunchProjectile(Vector3 direction)
    {
        if (ObjectPoolManager.Instance.PoolCount(projectileType) <= 0)
            return;

        IRecyclable projectile = ObjectPoolManager.Instance.DequeueObject(projectileType);
        Vector3 resultVelocity = (projectileSpeed + Vector3.Dot(transform.forward, rb.velocity)) * direction;

        int finalDamage = damage;
        finalDamage = (int)(finalDamage * Random.Range(0.9f, 1.1f));

        if (isPlayer)
            finalDamage = (int)(finalDamage * resultVelocity.magnitude / projectileSpeed);

        projectile.SetInitialProjectileValues(firePoint.position, resultVelocity, finalDamage, targetLayers, gameObject);
    }

    private Vector3 GetInterceptCourseDirection(Vector3 targetPos, Vector3 targetVelocity, Vector3 firePoint, float projectileSpeed)
    {
        Vector3 targetDir = targetPos - firePoint;
        targetDir.y = 0f;
        float iSpeed2 = projectileSpeed * projectileSpeed;
        float tSpeed2 = targetVelocity.sqrMagnitude;
        float fDot1 = Vector3.Dot(targetDir, targetVelocity);
        float targetDist2 = targetDir.sqrMagnitude;
        float d = (fDot1 * fDot1) - targetDist2 * (tSpeed2 - iSpeed2);
        if (d < 0.1f)  // negative == no possible course because the interceptor isn't fast enough
            return Vector3.zero;
        float sqrt = Mathf.Sqrt(d);
        float S1 = (-fDot1 - sqrt) / targetDist2;
        float S2 = (-fDot1 + sqrt) / targetDist2;
        if (S1 < 0.0001f)
        {
            if (S2 < 0.0001f)
                return Vector3.zero;
            else
                return (S2) * targetDir + targetVelocity;
        }
        else if (S2 < 0.0001f)
            return (S1) * targetDir + targetVelocity;
        else if (S1 < S2)
            return (S2) * targetDir + targetVelocity;
        else
            return (S1) * targetDir + targetVelocity;
    }
}


[System.Serializable]
public class BoolEvent : UnityEvent<bool> { }
