using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Regen Info", menuName = "HackNSlash/Entity/Regen Info")]
public class RegenInfo : ScriptableObject
{
    public float baseHealthRegen;
    public float baseManaRegen;
    [Space]
    public float hpRegenPerVitality;
    public float manaRegenPerEnergy;
}
