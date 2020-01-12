using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(WeaponManager))]
public class ReloadBarHandler : MonoBehaviour
{
    [SerializeField]
    private Slider reloadBar;

    private WeaponManager wm;

    private void Awake()
    {
        wm = GetComponent<WeaponManager>();
    }

    void Update()
    {
        reloadBar.value = wm.GetReloadingBarValue();
    }
}
