using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private EntityStats playerStats;

    [SerializeField]
    private Slider hpBar;

    [SerializeField]
    private TextMeshProUGUI hpText;

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
        UpdateExpBar();
        UpdateLevelText();

        playerSpawn = FindObjectOfType<PlayerSpawn>();
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
