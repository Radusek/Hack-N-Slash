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
            if (collider.gameObject != gameObject)
            {
                collider.GetComponent<EntityStats>().TakeDamage(damage, attackType);
                attacked = true;
            }
        }
        return attacked || isPlayer;
    }
}
