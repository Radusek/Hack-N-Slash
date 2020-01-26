using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : Projectile
{
    private float damageFractionOnExplosion = 0.45f;
    private float explosionRadius = 2.5f;

    [SerializeField]
    private GameObject explosionObject;


    protected override void Hit(bool forceDestroy = false)
    {
        int explosionDamage = (int)(GetCurrentDamage() * damageFractionOnExplosion);

        Collider[] targets = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var target in targets)
        {
            if (target.gameObject.layer.IsInLayerMask(targetLayers))
                target.GetComponent<EntityStats>().TakeDamage(explosionDamage, attackType, projectileCaster, target.transform.position - transform.position);
        }

        Instantiate(explosionObject, transform.position, Quaternion.identity);
        OnHit?.Invoke();
    }
}
