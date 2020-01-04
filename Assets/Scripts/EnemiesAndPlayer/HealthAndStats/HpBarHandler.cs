using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarHandler : MonoBehaviour
{
    [SerializeField]
    private Slider hpBar;

    private EntityStats entityStats;

    [SerializeField]
    private bool showWhenFull;

    private void Awake()
    {
        entityStats = GetComponent<EntityStats>();
    }

    private void Start()
    {
        UpdateHpBar();
    }

    public void UpdateHpBar()
    {
        hpBar.value = entityStats.GetHpFraction();
        bool sliderIsVisible = hpBar.value > 0f && (showWhenFull || hpBar.value < 1f); 
        hpBar.gameObject.SetActive(sliderIsVisible);
    }
}
