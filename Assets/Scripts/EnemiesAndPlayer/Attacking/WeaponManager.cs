using System.Collections;
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

    [SerializeField]
    private bool isPlayer;

    private bool canAttack = true;


    private void Start()
    {
        foreach (var weapon in weapons)
        {
            weapon.SetIsPlayer(isPlayer);
            weapon.enabled = false;
        }

        currentWeapon = 0;
        weapons[currentWeapon].enabled = true;
    }

    void Update()
    {
        if (EntityShouldAttack())
            weapons[currentWeapon].TryToAttackTarget();

        if (isPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Q))
                PickWeapon(currentWeapon + 1);
            return;
        }

        if (target == null)
            return;

        PickWeaponForEnemy();
    }

    protected virtual bool EntityShouldAttack()
    {
        if (!canAttack)
            return false;

        if (isPlayer)
            return Input.GetButtonDown("Fire1");

        return true;
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

        currentWeapon = id;
        weapons[currentWeapon].enabled = true;
    }

    public float GetReloadingTimeLeftFraction()
    {
        return weapons[currentWeapon].GetReloadingTimeLeftFraction();
    }

    public float GetLastWeaponUsageTime()
    {
        return weapons[currentWeapon].GetLastAttackTime();
    }

    public void SetCanAttack(bool itCanAttack)
    {
        canAttack = itCanAttack;
    }
}

public enum Layer
{
    Default = 0,
    Player = 8,
    Environment = 28,
    Dead = 30
}