using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private EntityStats playerStats;

    [SerializeField]
    private Slider hpBar;

    [SerializeField]
    private Slider expBar;


    public void UpdateHpBar()
    {
        hpBar.value = playerStats.GetHpFraction();
    }

    public void UpdateExpBar()
    {
        expBar.value = playerStats.GetExpFraction();
    }

}
