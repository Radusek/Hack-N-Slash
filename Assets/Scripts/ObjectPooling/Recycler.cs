using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycler : MonoBehaviour, IRecyclable
{
    [SerializeField]
    private PoolType poolType;

    public virtual bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public virtual void SetInitialValues(Vector3 spawnPosition, Vector3 areaPosition, float areaRadius)
    {
        transform.position = spawnPosition;
        gameObject.SetActive(true);
    }

    public virtual void StartDying() {}

    public virtual void EndDying()
    {
        gameObject.SetActive(false);
        ObjectPoolManager.Instance.EnqueueObject(this, poolType);
    }
}
