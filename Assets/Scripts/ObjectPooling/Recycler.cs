using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Recycler : MonoBehaviour, IRecyclable
{
    protected Rigidbody rb;

    [SerializeField]
    private PoolType poolType;

    private void Awake()
    {
        Initialize();
    }

    protected void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    protected void SetInitialValues(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;
        gameObject.SetActive(true);
    }

    public virtual void SetInitialEnemyValues(Vector3 spawnPosition, Vector3 areaPosition, float areaRadius){}

    public virtual void SetInitialProjectileValues(Vector3 spawnPosition, Vector3 velocity, int dmg, LayerMask layers, GameObject caster) {}

    public virtual void StartDying() {}

    public virtual void EndDying()
    {
        gameObject.SetActive(false);
        ObjectPoolManager.Instance.EnqueueObject(this, poolType);
    }
}
