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

    private PlayerSpawn playerSpawn;


    private void Start()
    {
        UpdateHpBar();
        UpdateManaBar();
        UpdateExpBar();
        UpdateLevelText();

        playerSpawn = FindObjectOfType<PlayerSpawn>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            inventoryUI.SetActive(!inventoryUI.activeSelf);
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
        animator.SetTrigger("PlayerDeath");
    }

    public void RespawnPlayer()
    {
        playerSpawn.PreparePlayer();
    }
}
