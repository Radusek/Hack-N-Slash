using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Projectile))]
public class ProjectileRecycler : Recycler
{
    private Projectile projectile;


    private void Awake()
    {
        base.Initialize();
        projectile = GetComponent<Projectile>();
    }

    public override void SetInitialProjectileValues(Vector3 spawnPosition, Vector3 velocity, int dmg, LayerMask layers, GameObject caster)
    {
        SetInitialValues(spawnPosition);
        rb.velocity = velocity;
        projectile.transform.LookAt(spawnPosition + velocity);
        projectile.Initialize(dmg, layers, caster);
    }
}
