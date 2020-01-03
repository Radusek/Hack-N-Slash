﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private Attack[] weapons;

    [SerializeField]
    private float[] weaponDistanceTreshold;

    private int currentWeapon;

    private Transform target;

    private void Start()
    {
        foreach (var weapon in weapons)
            weapon.enabled = false;

        currentWeapon = 0;
        weapons[currentWeapon].enabled = true;
    }

    void Update()
    {
        if (gameObject.layer == (int)Layer.Player)
        {
            if (Input.GetKeyDown(KeyCode.Q))
                PickWeapon(currentWeapon + 1);
            return;
        }

        if (target == null)
            return;

        PickWeaponForEnemy();
    }

    private void PickWeaponForEnemy()
    {
        float targetDistanceSquared = (target.position - transform.position).sqrMagnitude;
        for (int i = 0; i < weaponDistanceTreshold.Length; i++)
        {
            float treshold = weaponDistanceTreshold[i];
            if (targetDistanceSquared < treshold * treshold)
            {
                if (currentWeapon == i)
                    return;

                PickWeapon(i);
                return;
            }
        }
    }

    public void SetTargetTransform(Transform newTarget)
    {
        target = newTarget;
    }

    private void PickWeapon(int id)
    {
        weapons[currentWeapon].enabled = false;

        if (id < 0)
            id = weapons.Length;
        else
            id %= weapons.Length;

        Debug.Log($"New weapon used: {id}");
        currentWeapon = id;
        weapons[currentWeapon].enabled = true;
    }
}

public enum Layer
{
    Player = 8
}