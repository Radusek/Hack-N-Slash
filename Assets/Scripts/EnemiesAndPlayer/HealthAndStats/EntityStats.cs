using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EntityStats : MonoBehaviour
{
    public readonly static int expPerLevel = 1000;

    [SerializeField]
    private int hpAtLevelOne = 50;

    protected int maxHealth;
    protected int currentHealth;

    [SerializeField]
    private int manaAtLevelOne = 10;

    protected int maxMana;
    protected int currentMana;

    [Space]

    [SerializeField]
    protected int vitality;
    [SerializeField]
    protected int energy;
    [SerializeField]
    protected int strength;
    [SerializeField]
    protected int dexterity;

    [Space]

    private int statPointsToDistribute;


    private int experiencePoints = 0;
    private int level = 1;

    [SerializeField]
    private int baseExp = 105;

    [SerializeField]
    private int startingLevel = 1;

    // some resistances stats and other cool things to add

    [SerializeField]
    private float fadingDepth = 1.2f;

    [SerializeField]
    private GameObject minimapMark;

    public UnityEvent OnHpChanged;
    public UnityEvent OnManaChanged;
    public IntEvent OnDamageAmountTaken;
    public UnityEvent OnDeath;
    public UnityEvent OnDyingAnimationEnd;

    public UnityEvent OnExpGained;
    public UnityEvent OnLevelUp;

    public UnityEvent OnStatPointSpent;


    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        InitializeStart();
    }

    protected void Initialize()
    {
        maxHealth = GetMaxHp();
        currentHealth = maxHealth;

        maxMana = GetMaxMana();
        currentMana = maxMana;
    }

    private int GetMaxHp()
    {
        return hpAtLevelOne + 5 * vitality;
    }

    private int GetMaxMana()
    {
        return manaAtLevelOne + 3 * energy;
    }

    protected void InitializeStart()
    {
        experiencePoints = (startingLevel - 1) * expPerLevel;
        level = startingLevel;
    }

    public void SetStartingLevel(int number)
    {
        startingLevel = number;
    }

    public int GetLevel()
    {
        return (experiencePoints / expPerLevel) + 1;
    }

    public void AddExperience(int amount)
    {
        if (amount <= 0)
            return;

        experiencePoints += amount;

        int updatedLevel = GetLevel();
        if (updatedLevel > level)
        {
            statPointsToDistribute += 5 * (updatedLevel - level);
            level = updatedLevel;
            OnLevelUp?.Invoke();
        }

        OnExpGained?.Invoke();
    }

    public float GetHpFraction()
    {
        return (float)currentHealth / maxHealth;
    }

    public float GetManaFraction()
    {
        return (float)currentMana / maxMana;
    }

    public Vector2Int GetHpValues()
    {
        return new Vector2Int(currentHealth, maxHealth);
    }

    public Vector2Int GetManaValues()
    {
        return new Vector2Int(currentMana, maxMana);
    }

    public float GetExpFraction()
    {
        return (float)(experiencePoints - (level - 1) * expPerLevel) / expPerLevel;
    }

    public void TakeDamage(int amount, AttackType attackType, GameObject attacker, Vector3 attackPosition)
    {
        int oldCurrentHp = currentHealth;

        SubtractHp(amount, attackType, attackPosition);

        if (currentHealth != oldCurrentHp)
        {
            OnHpChanged?.Invoke();
            OnDamageAmountTaken?.Invoke(oldCurrentHp - currentHealth);
        }

        if (currentHealth <= 0)
        {
            if (attacker != null)
                RewardAttacker(attacker);
            Die();
        }
    }

    public void GetHealed(int amount)
    {
        int oldCurrentHp = currentHealth;
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        if (currentHealth != oldCurrentHp)
            OnHpChanged?.Invoke();

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void SubtractHp(int amount, AttackType attackType, Vector3 attackPosition)
    {
        // take different amount of damage based on the attackType to add
        currentHealth -= amount;
    }

    private void RewardAttacker(GameObject attacker)
    {
        EntityStats attackerStats = attacker.GetComponent<EntityStats>();
        int attackerLevel = attackerStats.GetLevel();
        float nerfValue = 0.16f;
        int levelsDifference = Mathf.Abs(attackerLevel - level);
        int levelDiffMultiplier = levelsDifference <= 2 ? 0 : levelsDifference - 2;
        int finalExp = (int)(baseExp * Mathf.Clamp01(1f - levelDiffMultiplier * nerfValue));
        attackerStats.AddExperience(finalExp);
    }

    private void Die()
    {
        OnDeath?.Invoke();

        if (gameObject.GetComponent<PlayerController>() != null)
            EnablePlayer(false);
        else
            StartCoroutine(FadeCoroutine());
    }

    public virtual void EnablePlayer(bool enabled)
    {
        GetComponent<WeaponManager>().enabled = enabled;
        GetComponent<Collider>().enabled = enabled;
        GetComponent<PlayerController>().enabled = enabled;
        GetComponent<Rigidbody>().isKinematic = !enabled;
    }

    private IEnumerator FadeCoroutine()
    {
        yield return new WaitForSeconds(5f);

        float fadingTime = 4f * fadingDepth;
        float startTime = Time.time;
        while (Time.time < startTime + fadingTime)
        {
            transform.position = transform.position + fadingDepth * Vector3.down / fadingTime * Time.deltaTime;
            yield return null;
        }

        OnDyingAnimationEnd?.Invoke();
    }

    public void SetFullHealth()
    {
        currentHealth = maxHealth;
        OnHpChanged?.Invoke();
    }

    public void Regenerate(int hpToRegen, int manaToRegen)
    {
        GetHealed(hpToRegen);

        int oldMana = currentMana;

        currentMana += manaToRegen;
        if (currentMana > maxMana)
            currentMana = maxMana;

        if (currentMana < 0)
            currentMana = 0;

        if (currentMana != oldMana)
            OnManaChanged?.Invoke();
    }

    public int GetVitality()
    {
        return vitality;
    }

    public int GetEnergy()
    {
        return energy;
    }

    public int GetStrength()
    {
        return strength;
    }

    public int GetDexterity()
    {
        return dexterity;
    }

    public void SpendStatPoint(int index)
    {
        if (statPointsToDistribute <= 0)
            return;

        switch (index)
        {
            case 0:
                vitality++;
                break;
            case 1:
                if (strength == 200)
                    return;
                strength++;
                break;
            case 2:
                if (dexterity == 200)
                    return;
                dexterity++;
                break;
            case 3:
                energy++;
                break;
            default:
                return;
        }

        statPointsToDistribute--;
        UpdateStatsEffects();
    }

    public int GetAvailableStatsPoints()
    {
        return statPointsToDistribute;
    }

    public void UpdateStatsEffects()
    {
        int oldMaxHp = maxHealth;
        maxHealth = GetMaxHp();
        currentHealth += maxHealth - oldMaxHp;
        OnHpChanged?.Invoke();


        int oldMaxMana = maxMana;
        maxMana = GetMaxMana();
        currentMana += maxMana - oldMaxMana;
        OnManaChanged?.Invoke();

        OnStatPointSpent?.Invoke();
    }
}

[System.Serializable]
public class IntEvent : UnityEvent<int> { }