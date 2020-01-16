using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRecycler : Recycler
{
    [SerializeField]
    private GameObject minimapMark;

    private WeaponManager wm;
    private Collider col;
    private NavMeshAgent agent;
    private EnemyController controller;

    private EntityStats stats;


    private void Awake()
    {
        base.Initialize();

        wm = GetComponent<WeaponManager>();
        col = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<EnemyController>();
        stats = GetComponent<EntityStats>();
    }

    public override void SetInitialEnemyValues(Vector3 spawnPosition, Vector3 areaPosition, float areaRadius)
    {
        SetInitialValues(spawnPosition);
        minimapMark.SetActive(true);
        stats.SetFullHealth();
        controller.SetMobArea(areaPosition, areaRadius);

        wm.enabled = true;
        col.enabled = true;
        agent.enabled = true;
        controller.enabled = true;
    }

    public override void StartDying()
    {
        wm.enabled = false;
        col.enabled = false;
        agent.enabled = false;
        controller.enabled = false;
        minimapMark.SetActive(false);
    }
}
