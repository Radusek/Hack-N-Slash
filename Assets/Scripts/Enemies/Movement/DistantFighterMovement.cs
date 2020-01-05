using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistantFighterMovement : EnemyController
{
    [SerializeField]
    private float rangedAttackRange;

    private bool isOutOfRangedAttackRange = true;

    protected override void InteractWithTarget()
    {
        isOutOfRangedAttackRange = isInCloseCombatRange || targetDistanceSquared > rangedAttackRange * rangedAttackRange;

        if (isOutOfRangedAttackRange)
            agent.destination = target.position;
        else
            agent.destination = transform.position;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            if (!isOutOfRangedAttackRange)
                agent.gameObject.transform.LookAt(target.position);
            else if (isInCloseCombatRange)
                RotateTowardsTarget();
        }     
    }
}
