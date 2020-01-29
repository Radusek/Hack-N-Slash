using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Loot Info", menuName = "HackNSlash/Enemies/LootInfo")]
[Serializable]
public class LootInfo : ScriptableObject
{
    [Serializable]
    public struct Loot
    {
        public GameObject item;
        public int maxAmount;
        [Range(0f, 1f)]
        public float dropChance;
    }

    public Loot[] possibleLoot;
}
