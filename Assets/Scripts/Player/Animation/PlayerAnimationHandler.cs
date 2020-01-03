using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    private Animator animator;

    private Rigidbody rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = transform.parent.transform.parent.GetComponent<Rigidbody>();
    }

    void Update()
    {
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
