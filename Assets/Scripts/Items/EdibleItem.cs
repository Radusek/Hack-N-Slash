using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "HackNSlash/Edible item")]
public class EdibleItem : Item
{
    public int healingValue;
    public int manaRestorationValue;


    public override bool Use()
    {
        owner.GetComponent<EntityStats>().GetHealed(healingValue);
        return true;
    }
}
