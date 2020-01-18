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

    [SerializeField]
    [Range(10f, 180f)]
    private float degreesTolerance = 15f;

    private bool targetIsVisible;
    public BoolEvent OnTargetVisibilityChange;

    private Rigidbody rb;

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

            Vector3 relativeTargetPosition = target.transform.position - firePoint.position;
            relativeTargetPosition.y = 0f;

            RaycastHit hit;
            int visibilityMask = ~(1 << gameObject.layer);
            if (Physics.Raycast(firePoint.position, relativeTargetPosition, out hit, attackRange, visibilityMask))
            {
                bool oldVisibilty = targetIsVisible;
                targetIsVisible = hit.collider.gameObject.layer.IsInLayerMask(targetLayers);
                if (targetIsVisible != oldVisibilty)
                    OnTargetVisibilityChange?.Invoke(targetIsVisible);
            }

            if (!targetIsVisible)
                return false;

            float deltaDegrees = transform.forward.AngleDegreesBetween(relativeTargetPosition.normalized);
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
        if (ObjectPoolManager.Instance.PoolCount(projectileType) <= 0)
            return;

        IRecyclable projectile = ObjectPoolManager.Instance.DequeueObject(projectileType);
        Vector3 resultVelocity = (projectileSpeed + Vector3.Dot(transform.forward, rb.velocity)) * direction;
        //modify damage basing on velocity for archer later
        projectile.SetInitialProjectileValues(firePoint.position, resultVelocity, damage, targetLayers, gameObject);
    }
}

[System.Serializable]
public class BoolEvent : UnityEvent<bool> { }
