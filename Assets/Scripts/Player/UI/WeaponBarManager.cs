﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBarManager : MonoBehaviour
{
    private WeaponManager playerWeaponManager;

    [SerializeField]
    private WeaponBarSlot[] weaponSlots;

    int currentWeapon;

    private void Awake()
    {
        playerWeaponManager = transform.parent.parent.GetComponent<HUDManager>().GetPlayer().GetComponent<WeaponManager>();

        Attack[] playerAttacks = playerWeaponManager.GetAttacks();

        for (int i = 0; i < playerAttacks.Length; i++)
        {
            weaponSlots[i].gameObject.SetActive(true);
            weaponSlots[i].weaponImage.sprite = playerAttacks[i].weaponImage;
        }

        currentWeapon = 0;
        if (weaponSlots.Length > 0)
        {
            weaponSlots[0].SelectWeapon();
        }
    }

    private void OnEnable()
    {
        playerWeaponManager.OnWeaponChanged += ChooseWeapon;
    }

    private void OnDisable()
    {
        playerWeaponManager.OnWeaponChanged -= ChooseWeapon;
    }

    public void ChooseWeapon(int newWeapon)
    {
        weaponSlots[currentWeapon].DeselectWeapon();
        weaponSlots[newWeapon].SelectWeapon();
        currentWeapon = newWeapon;
    }
}
