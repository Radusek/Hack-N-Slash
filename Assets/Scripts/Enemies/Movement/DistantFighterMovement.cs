using UnityEngine;

public class DistantFighterMovement : EnemyController
{
    [SerializeField]
    private float rangedAttackRange;

    private bool isOutOfRangedAttackRange = true;

    private bool targetIsVisible;

    protected override void InteractWithTarget()
    {
        isOutOfRangedAttackRange = isInCloseCombatRange || targetDistanceSquared > rangedAttackRange * rangedAttackRange;

        if (!isOutOfRangedAttackRange && targetIsVisible)
            agent.destination = transform.position;
        else
            agent.destination = target.position;
    }

    public void SetTargetVisibility(bool visibility)
    {
        targetIsVisible = visibility;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            if ((!isOutOfRangedAttackRange && targetIsVisible) || isInCloseCombatRange)
                RotateTowardsTarget();
        }     
    }
}
