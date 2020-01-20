using System.Text;
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

    public override string GetDescription()
    {
        StringBuilder sb = new StringBuilder(base.GetDescription());

        if (healingValue != 0)
            sb.Append($"\nHealth: {healingValue.ToString()}");
        if (manaRestorationValue != 0)
            sb.Append($"Mana: {manaRestorationValue.ToString()}");

        if ((hpRegenerationRate != 0 || manaRegenerationRate != 0))
        {
            sb.Append($"\nBoosts regeneration for {effectDuration.ToString("n0")} s:");
            if (hpRegenerationRate != 0)
                sb.Append($"\nHp/s: {hpRegenerationRate.ToString("n1")}");
            if (manaRegenerationRate != 0)
                sb.Append($"\nMana/s: {manaRegenerationRate.ToString("n1")}");
        }

        return sb.ToString();
    }
}
