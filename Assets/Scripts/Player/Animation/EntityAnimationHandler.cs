using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EntityAnimationHandler : MonoBehaviour
{
    private Animator animator;

    private Rigidbody rb;

    [SerializeField]
    private bool usesNavMeshAgent = true;

    private Direction lastDirection = Direction.None;

    private NavMeshAgent agent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody>();
        if (usesNavMeshAgent)
            agent = transform.parent.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (usesNavMeshAgent)
            animator.SetFloat("Velocity", agent.velocity.magnitude);
        else
            animator.SetFloat("Velocity", rb.velocity.magnitude);
    }

    public void SetHurtTrigger()
    {
        animator.SetTrigger("Hurt");
    }

    public void SetDeathTrigger()
    {
        animator.SetTrigger("Death");
    }

    public void SetAttackTrigger()
    {
        animator.SetTrigger("Attack");
    }

    public void SetShieldingBool(bool value)
    {
        animator.SetBool("Shielding", value);
    }

    public void SetWalkDirection(Direction direction)
    {
        if (direction == lastDirection)
            return;

        if (lastDirection != Direction.None)
            animator.SetBool(DirectionToAnimatorParameterName(lastDirection), false);
        if (direction != Direction.None)
            animator.SetBool(DirectionToAnimatorParameterName(direction), true);

        lastDirection = direction;
    }

    private string DirectionToAnimatorParameterName(Direction direction)
    {
        switch (direction)
        {
            case Direction.Forward:
                return "WalkForward";
            case Direction.Back:
                return "WalkBack";
            case Direction.Left:
                return "WalkLeft";
            case Direction.Right:
                return "WalkRight";
            default:
                return string.Empty;
        }
    }
}

public enum Direction
{
    None,
    Forward,
    Back,
    Left,
    Right
}

[System.Serializable]
public class DirectionEvent : UnityEvent<Direction> { }
