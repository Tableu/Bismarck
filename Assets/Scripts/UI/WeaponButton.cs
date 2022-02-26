using System;
using Ships.Components;
using UI.InfoWindow;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : SubsystemButton
{
    [SerializeField] private Slider slider;

    private void Awake()
    {
        slider.minValue = 0;
        slider.maxValue = 1;
    }

    protected override void OnClick()
    {
        if (Player != null && Target is Weapon weapon)
        {
            weapon.Fire();
        }
    }

    private void FixedUpdate()
    {
        if (Player != null && Target is Weapon weapon)
        {
            slider.value = weapon.FireTimePercent;
        }
    }
}