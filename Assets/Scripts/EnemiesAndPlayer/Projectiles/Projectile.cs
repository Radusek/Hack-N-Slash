using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private GameObject projectileCaster; //change to ranged attack and check for targetLayer to be destroyed, enable enemyProjectile and enemy collisions

    private int damage;
    private LayerMask targetLayers;

    [SerializeField]
    private AttackType attackType;

    [SerializeField]
    private bool gravityAffected;

    [SerializeField]
    [Range(0f, 1f)]
    private float gravityScale = 0.5f;

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
            OnHit?.Invoke();
        }

        if (other.gameObject.layer == (int)Layer.Default || other.gameObject.layer == (int)Layer.Environment)
            OnHit?.Invoke();
    }

    protected virtual void InteractWithTarget(Collider other)
    {
        other.GetComponent<EntityStats>().TakeDamage(damage, attackType, projectileCaster, transform.position);
    }
}
