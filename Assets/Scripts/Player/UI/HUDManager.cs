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

        if (Input.GetKeyDown(KeyCode.I))
            inventoryUI.SetActive(!inventoryUI.activeSelf);

        if (Input.GetKeyDown(KeyCode.C))
        {
            statsUI.SetActive(!statsUI.activeSelf);
            openStatsButton.SetActive(!statsUI.activeSelf && playerStats.GetAvailableStatsPoints() > 0);
        }
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
    }

    public void OnLevelUp()
    {
        availablePointsText.text = playerStats.GetAvailableStatsPoints().ToString();
        openStatsButton.SetActive(!statsUI.activeSelf);
    }

    public void OpenStats()
    {
        openStatsButton.SetActive(false);
        statsUI.SetActive(true);
    }

    public void CloseStats()
    {
        statsUI.SetActive(false);
        openStatsButton.SetActive(playerStats.GetAvailableStatsPoints() > 0);
    }
}
