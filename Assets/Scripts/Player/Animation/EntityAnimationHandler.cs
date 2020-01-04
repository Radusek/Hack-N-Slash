using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityAnimationHandler : MonoBehaviour
{
    private Animator animator;

    private Rigidbody rb;

    [SerializeField]
    private bool usesNavMeshAgent = true;

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
}
