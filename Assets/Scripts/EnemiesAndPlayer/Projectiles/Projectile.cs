using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private GameObject projectileCaster; //change to ranged attack and check for targetLayer to be destroyed, enable enemyProjectile and enemy collisions

    private int damage;
    private LayerMask targetLayers;
    private AttackType attackType;

    [SerializeField]
    private bool gravityAffected;

    [SerializeField]
    [Range(0f, 1f)]
    private float gravityScale = 0.5f;

    private Rigidbody rb;


    public void Initialize(GameObject caster, int dmg, LayerMask layers, AttackType type)
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = gravityAffected;
        projectileCaster = caster;
        damage = dmg;
        targetLayers = layers;
        attackType = type;
    }

    private void FixedUpdate()
    {
        if (gravityAffected)
        {
            rb.AddForce(-(1f - gravityScale) * Physics.gravity, ForceMode.Acceleration);
            transform.LookAt(transform.position + rb.velocity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == projectileCaster)
            return;

        if (other.gameObject.layer.IsInLayerMask(targetLayers))
        {
            InteractWithTarget(other);
            Destroy(gameObject);
        }

        if (other.gameObject.layer == (int)Layer.Default || other.gameObject.layer == (int)Layer.Environment)
            Destroy(gameObject);
    }

    protected virtual void InteractWithTarget(Collider other)
    {
        other.GetComponent<EntityStats>().TakeDamage(damage, attackType, projectileCaster);
    }
}
