using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "HackNSlash/Edible item")]
public class EdibleItem : Item
{
    public int healingValue;
    public int manaRestorationValue;

    [Space]
    [Space]

    public float hpRegenerationRate;
    public float manaRegenerationRate;

    public float effectDuration;


    public override bool Use()
    {
        owner.GetComponent<EntityStats>().GetHealed(healingValue);
        if (hpRegenerationRate != 0f || manaRegenerationRate != 0f)
            owner.GetComponent<StatsRegen>().IncreaseRegeneration(hpRegenerationRate, manaRegenerationRate, effectDuration);
        return true;
    }
}
