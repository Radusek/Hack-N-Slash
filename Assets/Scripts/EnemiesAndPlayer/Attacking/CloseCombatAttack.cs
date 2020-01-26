using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCombatAttack : Attack
{
    protected override bool AttackTarget(Collider[] colliders)
    {
        bool attacked = false;
        foreach(var collider in colliders)
        {
            if (collider.gameObject == gameObject)
                continue;

            int finalDamage = (int)(damage * Random.Range(0.9f, 1.1f));
            collider.GetComponent<EntityStats>().TakeDamage(finalDamage, attackType, gameObject, transform.position);
            attacked = true;
        }
        return attacked;
    }
}
