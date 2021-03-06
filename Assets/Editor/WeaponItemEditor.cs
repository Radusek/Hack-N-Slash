﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponItem))]
public class WeaponItemEditor : ItemEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WeaponItem weaponItem = (WeaponItem)target;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Weapon Prefab");
        weaponItem.weaponPrefab = (GameObject)EditorGUILayout.ObjectField(weaponItem.weaponPrefab, typeof(GameObject), allowSceneObjects: true);
        EditorGUILayout.EndHorizontal();

        weaponItem.attackType = (AttackType)EditorGUILayout.EnumPopup("Attack Type", weaponItem.attackType);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Firing Sound");
        weaponItem.firingSound = (AudioClip)EditorGUILayout.ObjectField(weaponItem.firingSound, typeof(AudioClip), allowSceneObjects: true);
        EditorGUILayout.EndHorizontal();

        weaponItem.damage = EditorGUILayout.IntField("Damage", weaponItem.damage);
        weaponItem.recastInterval = EditorGUILayout.FloatField("Recast Interval", weaponItem.recastInterval);

        if (weaponItem.attackType != AttackType.Melee)
        {
            weaponItem.projectileType = (PoolType)EditorGUILayout.EnumPopup("Projectile Type", weaponItem.projectileType);
            weaponItem.projectileSpeed = EditorGUILayout.FloatField("Projectile Speed", weaponItem.projectileSpeed);
        }

        if (GUI.changed)
        {
            Undo.RecordObject(weaponItem, "save");
            EditorUtility.SetDirty(weaponItem);
            AssetDatabase.SaveAssets();
            Repaint();
            Debug.Log("SAVED weapon");
        }
    }
}
