﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New weapon", menuName = "HackNSlash/Items/Weapon item")]
public class WeaponItem : Item
{
    [Space]

    public GameObject weaponPrefab;

    [Space]
    [Space]

    public AudioClip firingSound;

    public AttackType attackType;
    public int damage;
    public float recastInterval;

    [Space]
    [Space]

    public PoolType projectileType;
    public float projectileSpeed;


    public override bool Use()
    {
        WeaponManager wm = owner.GetComponent<WeaponManager>();
        EntitySoundIndex soundIndex;
        int slotNumber;
        if (attackType == AttackType.Melee)
        {
            slotNumber = 0;
            soundIndex = EntitySoundIndex.MeleeHit;
        }
        else
        {
            slotNumber = 1;
            soundIndex = EntitySoundIndex.RangedWeaponUse;
        }

        owner.GetComponent<SoundPlayer>().SetAudioClip(firingSound, soundIndex);

        WeaponItem previouslyEquippedWeapon = wm.GetCurrentWeaponItem(slotNumber);
        previouslyEquippedWeapon.owner = owner;

        Inventory ownerInventory = owner.GetComponent<Inventory>();
        ownerInventory.RemoveItem(this);
        ownerInventory.AddItem(previouslyEquippedWeapon);
        
        wm.SetNewWeapon(this, slotNumber);
        return false;
    }

    public override string GetDescription()
    {
        StringBuilder sb = new StringBuilder(base.GetDescription());

        sb.Append($"\nDamage type: {attackType.ToString()}");
        sb.Append($"\nDamage value: {damage.ToString()}");
        sb.Append($"\nReuse time: {recastInterval.ToString("n2")} s");

        return sb.ToString();
    }
}
