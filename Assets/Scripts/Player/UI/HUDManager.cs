using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject inventoryUI;

    [SerializeField]
    private GameObject statsUI;

    [SerializeField]
    private EntityStats playerStats;

    [SerializeField]
    private Slider hpBar;

    [SerializeField]
    private TextMeshProUGUI hpText;

    [SerializeField]
    private Slider manaBar;

    [SerializeField]
    private TextMeshProUGUI manaText;

    [SerializeField]
    private Slider expBar;

    [SerializeField]
    private TextMeshProUGUI expText;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI[] statsTexts;

    [SerializeField]
    private TextMeshProUGUI availablePointsText;

    [SerializeField]
    private GameObject openStatsButton;

    [SerializeField]
    private GameObject[] statButtons;

    [SerializeField]
    private GameObject statsDescriptionObject;
    [SerializeField]
    private TextMeshProUGUI statsDescription;

    private PlayerSpawn playerSpawn;

    private bool inputEnabled = true;


    private void Start()
    {
        UpdateHpBar();
        UpdateManaBar();
        UpdateExpBar();
        UpdateLevelText();
        UpdateStatsTexts();
        statsUI.SetActive(false);

        playerSpawn = FindObjectOfType<PlayerSpawn>();
    }

    private void Update()
    {
        if (!inputEnabled)
            return;

        if (Input.GetKeyDown(KeyCode.E))
            SetIntentoryUIActiveState(!inventoryUI.activeSelf);

        if (Input.GetKeyDown(KeyCode.C))
            SetStatsUIActiveState(!statsUI.activeSelf);

        if (Input.GetKeyDown(KeyCode.Escape))
            HandleEscPress();
    }

    public void SetIntentoryUIActiveState(bool newState)
    {
        inventoryUI.SetActive(newState);
    }

    public void SetStatsUIActiveState(bool newState)
    {
        SetNoStatDescription();
        statsUI.SetActive(newState);
        openStatsButton.SetActive(!statsUI.activeSelf && playerStats.GetAvailableStatsPoints() > 0);
    }

    private void HandleEscPress()
    {
        if (statsUI.activeSelf)
        {
            SetStatsUIActiveState(false);
            return;
        }

        if (inventoryUI.activeSelf)
        {
            SetIntentoryUIActiveState(false);
            return;
        }

        //pause screen later
    }

    public GameObject GetPlayer()
    {
        return playerStats.gameObject;
    }

    public void UpdateHpBar()
    {
        hpBar.value = playerStats.GetHpFraction();
        Vector2Int hpValues = playerStats.GetHpValues();
        if (hpValues.x < 0)
            hpValues.x = 0;

        hpText.text = hpValues.x.ToString() + "/" + hpValues.y.ToString();
    }

    public void UpdateManaBar()
    {
        manaBar.value = playerStats.GetManaFraction();
        Vector2Int manaValues = playerStats.GetManaValues();
        if (manaValues.x < 0)
            manaValues.x = 0;

        manaText.text = manaValues.x.ToString() + "/" + manaValues.y.ToString();
    }

    public void UpdateExpBar()
    {
        float xpFraction = playerStats.GetExpFraction();
        expBar.value = xpFraction;
        expText.text = ((int)(xpFraction * 100)).ToString() + "%";
    }

    public void UpdateLevelText()
    {
        levelText.text = playerStats.GetLevel().ToString();
    }

    public void SetPlayer(EntityStats player)
    {
        playerStats = player;
    }

    public void RunRespawnAnimation()
    {
        inputEnabled = false;

        animator.SetTrigger("PlayerDeath");

        openStatsButton.SetActive(false);
        statsUI.SetActive(false);
        inventoryUI.SetActive(false);
    }

    public void RespawnPlayer()
    {
        playerSpawn.PreparePlayer();
        openStatsButton.SetActive(playerStats.GetAvailableStatsPoints() > 0);
        inputEnabled = true;
    }

    public void SpendStatPoint(int index)
    {
        playerStats.SpendStatPoint(index);
        UpdateStatsTexts();
    }

    private void UpdateStatsTexts()
    {
        statsTexts[0].text = playerStats.GetVitality().ToString();
        statsTexts[1].text = playerStats.GetStrength().ToString();
        statsTexts[2].text = playerStats.GetDexterity().ToString();
        statsTexts[3].text = playerStats.GetEnergy().ToString();

        availablePointsText.text = playerStats.GetAvailableStatsPoints().ToString();
        UpdateStatsButtons();
    }

    public void OnLevelUp()
    {
        availablePointsText.text = playerStats.GetAvailableStatsPoints().ToString();
        UpdateStatsButtons();
        openStatsButton.SetActive(!statsUI.activeSelf);
    }

    private void UpdateStatsButtons()
    {
        int availableStatPoints = playerStats.GetAvailableStatsPoints();

        statButtons[0].SetActive(availableStatPoints > 0);
        statButtons[1].SetActive(availableStatPoints > 0 && playerStats.GetStrength() < 200);
        statButtons[2].SetActive(availableStatPoints > 0 && playerStats.GetDexterity() < 200);
        statButtons[3].SetActive(availableStatPoints > 0);
    }

    public void SetNoStatDescription()
    {
        statsDescriptionObject.SetActive(false);
        statsDescription.text = string.Empty;
    }

    public void SetVitalityDescription()
    {
        statsDescriptionObject.SetActive(true);
        StringBuilder sb = new StringBuilder();
        sb.Append("<b>Vitality</b>");
        sb.Append("\nIncreases total health and health regeneration");
        sb.Append("\n\nCurrent bonuses:");
        sb.Append($"\n{playerStats.GetVitality()*5} HP");
        sb.Append($"\n{playerStats.GetVitality()*0.04f:n2} HP/s");
        statsDescription.text = sb.ToString();
    }

    public void SetStrengthDescription()
    {
        statsDescriptionObject.SetActive(true);
        StringBuilder sb = new StringBuilder();
        sb.Append("<b>Strength</b>");
        sb.Append("\nIncreases melee damage");
        sb.Append("\n\nCurrent bonus:");
        sb.Append($"\n+{playerStats.GetStrength() * 1.25f:n2}% ");
        statsDescription.text = sb.ToString();
    }

    public void SetDexterityDescription()
    {
        statsDescriptionObject.SetActive(true);
        StringBuilder sb = new StringBuilder();
        sb.Append("<b>Dexterity</b>");
        sb.Append("\nIncreases ranged weapons' fire rate");
        sb.Append("\n\nCurrent fire rate:");
        sb.Append($"\n{100f/Attack.GetRecastIntervalTimeMultiplier(playerStats.GetDexterity()):n2}% ");
        statsDescription.text = sb.ToString();
    }

    public void SetEnergyDescription()
    {
        statsDescriptionObject.SetActive(true);
        StringBuilder sb = new StringBuilder();
        sb.Append("<b>Energy</b>");
        sb.Append("\nIncreases total mana and mana regeneration");
        sb.Append("\n\nCurrent bonuses:");
        sb.Append($"\n{playerStats.GetEnergy() * 3} Mana");
        sb.Append($"\n{playerStats.GetEnergy() * 0.03f:n2} MP/s");
        statsDescription.text = sb.ToString();
    }


    public bool GetInputEnabled()
    {
        return inputEnabled;
    }
}
