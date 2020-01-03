using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private GameObject projectileCaster;

    private int damage;
    private LayerMask targetLayers;
    private AttackType attackType;

    [SerializeField]
    private bool gravityAffected;

    private int CasterToProjectileLayer(int casterLayer)
    {
        return casterLayer + 2;
    }

    public void Initialize(GameObject caster, int dmg, LayerMask layers, AttackType type)
    {
        projectileCaster = caster;
        gameObject.layer = CasterToProjectileLayer(projectileCaster.layer);
        GetComponent<Rigidbody>().useGravity = gravityAffected;
        damage = dmg;
        targetLayers = layers;
        attackType = type;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == projectileCaster)
            return;

        if (other.gameObject.layer.IsInLayerMask(targetLayers))
            InteractWithTarget(other);

        Destroy(gameObject);
    }

    protected virtual void InteractWithTarget(Collider other)
    {
        other.GetComponent<EntityStats>().TakeDamage(damage, attackType);
    }
}
