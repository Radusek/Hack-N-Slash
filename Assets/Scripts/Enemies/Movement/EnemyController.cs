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
    private float playerCheckInterval = 0.5f;

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

            InteractWithTarget();
        }
        else
        {
            if (idleCoroutine != null)
                return;

            Idle();
        }
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

        if (target == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange, layersToTarget);
            Collider[] collidersFiltered = colliders.Where(col => col.enabled && col.gameObject != gameObject).ToArray();
            if (collidersFiltered.Length > 0)
                target = collidersFiltered[0].transform;
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
}

[System.Serializable]
public class TransformEvent: UnityEvent<Transform> { }
