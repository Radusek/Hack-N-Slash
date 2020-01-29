using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New characteristics", menuName = "HackNSlash/Entity/Characteristics")]
public class EntityCharacteristicsInfo : ScriptableObject
{
    public int vitality;
    public int strength;
    public int dexterity;
    public int energy;
}
