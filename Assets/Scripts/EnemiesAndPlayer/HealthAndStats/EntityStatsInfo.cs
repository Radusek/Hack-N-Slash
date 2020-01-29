using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New stats info", menuName = "HackNSlash/Entity/Entity stats info")]
public class EntityStatsInfo : ScriptableObject
{
    public int maxHealth;
    public int maxMana;

    public int level;
    public int experienceReward;
}
