using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    protected NavMeshAgent agent;

    protected Transform target;

    [SerializeField]
    private LayerMask layersToTarget;

    [SerializeField]
    private float sightRange = 3f;

    [SerializeField]
    private float closeCombatRange = 2f;

    private float playerCheckInterval = 0.5f;

    protected float targetDistanceSquared;
    protected bool isInCloseCombatRange = false;

    public TransformEvent onTargetChanged;

    private Coroutine idleCoroutine;

    private void Awake()
    {
        idleCoroutine = null;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(LookForTargetCoroutine());
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            if (idleCoroutine != null)
            {
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
            }

            targetDistanceSquared = (target.position - transform.position).sqrMagnitude;
            isInCloseCombatRange = targetDistanceSquared <= closeCombatRange * closeCombatRange;

            InteractWithTarget();
        }
        else
        {
            if (idleCoroutine != null)
                return;

            Idle();
        }
    }

    private void LateUpdate()
    {
        if (target != null && isInCloseCombatRange)
            RotateTowardsTarget();
    }

    private IEnumerator LookForTargetCoroutine()
    {
        var delay = new WaitForSeconds(playerCheckInterval);
        while(true)
        {
            CheckForTarget();
            yield return delay;
        }
    }

    private void CheckForTarget()
    {
        Transform oldTarget = target;
        if (target != null)
        {
            if (target.gameObject.layer == (int)Layer.Dead || (target.position - transform.position).sqrMagnitude > sightRange * sightRange)
                target = null;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange, layersToTarget).Where(col => col.gameObject != gameObject).ToArray();
        if (colliders.Length > 0)
        {
            float distance = float.MaxValue;
            Collider finalTarget = null;
            foreach (var col in colliders)
            {
                float currentDistance = (col.transform.position - transform.position).sqrMagnitude;
                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    finalTarget = col;
                }
            }

            target = finalTarget.transform;
        }

        if (target != oldTarget)
            onTargetChanged?.Invoke(target);
    }

    protected virtual void InteractWithTarget(){}

    protected virtual void Idle()
    {
        idleCoroutine = StartCoroutine(IdleCoroutine());
    }

    private IEnumerator IdleCoroutine()
    {
        float waitingTime = Random.value + 0.75f;
        var delay = new WaitForSeconds(waitingTime);
        float travellingRange = 4f;
        while(true)
        {
            agent.destination = transform.position;
            yield return delay;

            Vector2 newDestination = travellingRange * Random.insideUnitCircle;
            agent.destination = transform.position + new Vector3(newDestination.x, 0f, newDestination.y);
            yield return delay;
        }
    }

    protected void RotateTowardsTarget()
    {
        Vector3 targetDirection = target.position - transform.position;
        targetDirection.y = 0f;
        targetDirection.Normalize();

        float rotationSpeed = 5f;
        float rotation = rotationSpeed * Vector3.Cross(transform.forward, targetDirection).y * Time.deltaTime;
        transform.RotateAround(Vector3.up, rotation);
    }
}

[System.Serializable]
public class TransformEvent: UnityEvent<Transform> { }
