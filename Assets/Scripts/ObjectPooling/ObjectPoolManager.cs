using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    Warrior,
    Archer,
    Arrow,
    Rock,
    Spike,
    Bat,
    Count
}


public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    private Dictionary<PoolType, Queue<IRecyclable>> objectPools;

    [SerializeField]
    private GameObject[] poolParents;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this);
            return;
        }
        #endregion

        objectPools = new Dictionary<PoolType, Queue<IRecyclable>>
        {
            { PoolType.Warrior, new Queue<IRecyclable>() },
            { PoolType.Archer, new Queue<IRecyclable>() },
            { PoolType.Arrow, new Queue<IRecyclable>() },
            { PoolType.Rock, new Queue<IRecyclable>() },
            { PoolType.Spike, new Queue<IRecyclable>() },
            { PoolType.Bat, new Queue<IRecyclable>() },
        };

        for (int i = 0; i < poolParents.Length; i++)
        {
            IRecyclable[] entities = poolParents[i].GetComponentsInChildren<IRecyclable>(true);
            foreach (var entity in entities)
            {
                objectPools[(PoolType)i].Enqueue(entity);
            }
        }
    }

    public void EnqueueObject(IRecyclable recyclable, PoolType poolType)
    {
        objectPools[poolType].Enqueue(recyclable);
    }

    public int PoolCount(PoolType poolType)
    {
        return objectPools[poolType].Count;
    }

    public IRecyclable DequeueObject(PoolType poolType)
    {
        return objectPools[poolType].Dequeue();
    }
}
