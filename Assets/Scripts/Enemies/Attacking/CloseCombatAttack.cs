using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCombatAttack : Attack
{
    protected override void AttackTarget(Collider[] colliders)
    {
        foreach(var collider in colliders)
        {
            collider.GetComponent<EntityStats>().TakeDamage(damage, attackType);
        }
    }
}
