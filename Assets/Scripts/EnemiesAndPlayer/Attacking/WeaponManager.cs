using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private Attack[] weapons;

    [SerializeField]
    private float[] weaponDistanceTreshold;

    private int currentWeapon;

    private Transform target;

    [SerializeField]
    private LayerMask targetLayers;

    [SerializeField]
    private bool isPlayer;

    private bool canAttack = true;

    [SerializeField]
    private Transform holdingHand;
    private GameObject currentWeaponObject;

    public event Action<int> OnWeaponChanged;


    private void Awake()
    {
        foreach (var weapon in weapons)
        {
            weapon.SetIsPlayer(isPlayer);
            weapon.SetTargetLayers(targetLayers);
        }

        currentWeapon = 0;

        if (isPlayer)
            currentWeaponObject = holdingHand.GetChild(0).gameObject;
    }

    public WeaponItem GetCurrentWeaponItem(int slotNumber)
    {
        return weapons[slotNumber].GetWeaponItem();
    }

    public void SetNewWeapon(WeaponItem newWeapon, int slotNumber)
    {
        weapons[slotNumber].SetNewWeapon(newWeapon);
        PickWeapon(slotNumber, true);

        Destroy(currentWeaponObject);
        currentWeaponObject = Instantiate(newWeapon.weaponPrefab, holdingHand.position, holdingHand.rotation, holdingHand);
    }

    public void UpdateWeapons()
    {
        foreach (var weapon in weapons)
            weapon.UpdateWeaponStats();
    }

    void Update()
    {
        if (EntityShouldAttack())
            weapons[currentWeapon].TryToAttackTarget();


        if (isPlayer)
        {
            SlowlyReloadOtherWeapons();

            int oldCurrentWeapon = currentWeapon;
            if (Input.GetKeyDown(KeyCode.Q))
                PickWeapon(currentWeapon + 1);
            else
            {
                KeyCode keyCode;
                for (int i = 0; i < weapons.Length; i++)
                {
                    keyCode = (KeyCode)((int)KeyCode.Alpha1 + i);
                    if (Input.GetKeyDown(keyCode))
                    {
                        PickWeapon(i);
                    }
                }
            }

            if (currentWeapon != oldCurrentWeapon)
                OnWeaponChanged?.Invoke(currentWeapon);

            return;
        }

        if (target == null)
            return;

        PickWeaponForEnemy();
    }

    private void SlowlyReloadOtherWeapons()
    {
        foreach (var weapon in weapons)
        {
            if (weapon == weapons[currentWeapon])
                continue;

            weapon.ReduceCooldown(Time.deltaTime, 0.4f);
        }
    }

    protected virtual bool EntityShouldAttack()
    {
        if (!canAttack)
            return false;

        if (isPlayer)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return false;

            return Input.GetButtonDown("Fire1");
        }

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

    private void PickWeapon(int id, bool forceReloadTime = false)
    {
        if (currentWeapon == id && !forceReloadTime)
            return;

        if (id < 0)
            id = weapons.Length;
        else
            id %= weapons.Length;

        currentWeapon = id;
        OnWeaponChanged?.Invoke(currentWeapon);
        weapons[currentWeapon].SetLastAttackTime();
    }

    public float GetReloadingBarValue()
    {
        return weapons[currentWeapon].GetReloadingBarValue();
    }

    public float GetLastWeaponUsageTime()
    {
        return weapons[currentWeapon].GetLastAttackTime();
    }

    public void SetCanAttack(bool itCanAttack)
    {
        canAttack = itCanAttack;
    }

    public Attack[] GetAttacks()
    {
        return weapons;
    }
}

public enum Layer
{
    Default = 0,
    Player = 8,
    Enemy = 9,
    Enemy2 = 10,
    Interactable = 27,
    Environment = 28,
    Dead = 30
}