using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class RangedProjectileAttack : Attack
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float projectileSpeed = 40f;

    [SerializeField]
    [Range(10f, 180f)]
    private float degreesTolerance = 15f;

    private bool targetIsVisible;
    public BoolEvent OnTargetVisibilityChange;

    private Rigidbody rb;

    private void Awake()
    {
        base.Initialize();
        rb = GetComponent<Rigidbody>();
    }

    protected override bool AttackTarget(Collider[] colliders)
    {
        if (isPlayer)
        {
            LaunchProjectile(transform.forward);
            return true;
        }

        if (colliders.Length == 0)
            return false;

        foreach(var target in colliders)
        {
            if (target.gameObject == gameObject)
                continue;
            Vector3 relativeTargetPosition = target.transform.position - transform.position;
            relativeTargetPosition.y = 0f;

            RaycastHit hit;
            int layerMask = ~(1 << gameObject.layer);
            if (Physics.Raycast(firePoint.position, relativeTargetPosition, out hit, attackRange, layerMask))
            {
                bool oldVisibilty = targetIsVisible;
                targetIsVisible = hit.collider.gameObject.layer.IsInLayerMask(targetLayers);
                if (targetIsVisible != oldVisibilty)
                    OnTargetVisibilityChange?.Invoke(targetIsVisible);
            }

            if (!targetIsVisible)
                return false;

            float cosine = Vector3.Dot(transform.forward, relativeTargetPosition.normalized);
            float deltaDegrees = Mathf.Acos(cosine) * Mathf.Rad2Deg;
            if (deltaDegrees <= degreesTolerance)
            {
                LaunchProjectile(relativeTargetPosition.normalized);
                return true;
            }
        }
        return false;
    }

    private void LaunchProjectile(Vector3 direction)
    {
        direction.y = 0f;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Initialize(gameObject, damage, targetLayers, attackType);
        Vector3 resultVelocity = (projectileSpeed + Vector3.Dot(transform.forward, rb.velocity)) * direction;
        projectile.GetComponent<Rigidbody>().velocity = resultVelocity;
        projectile.transform.LookAt(firePoint.position + direction);
    }
}

[System.Serializable]
public class BoolEvent : UnityEvent<bool> { }
