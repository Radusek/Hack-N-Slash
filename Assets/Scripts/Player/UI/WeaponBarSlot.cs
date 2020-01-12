using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBarSlot : MonoBehaviour
{
    public Image weaponImage;

    [SerializeField]
    private GameObject highlightBorder;

    [SerializeField]
    private Slider reloadBar;

    public void DeselectWeapon()
    {
        highlightBorder.SetActive(false);
    }

    public void SelectWeapon()
    {
        highlightBorder.SetActive(true);
    }

    public void SetReloadBarValue(float value)
    {
        reloadBar.value = value;
    }
}
