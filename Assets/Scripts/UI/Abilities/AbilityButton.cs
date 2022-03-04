using Ships.Components;
using Systems.Abilities;
using Systems.Modifiers;
using UI.InfoWindow;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : SubsystemButton
{
    [SerializeField] private Slider slider;
    private Ability _ability;

    private void Awake()
    {
        slider.minValue = 0;
        slider.maxValue = 1;

        if (_ability != null)
        {
            _ability.CooldownEvent += UpdateTimer;
        }
    }

    protected override void Start()
    {
        base.Start();
        if (_ability != null)
        {
            StartCoroutine(_ability.CooldownTimer());
        }
    }

    protected override void OnClick()
    {
        if (Player != null && !_ability.OnCooldown)
        {
            foreach (ModifierData modifier in _ability.AbilityData.Modifiers)
            {
                modifier.AttachNewModifer(Player);
            }

            StartCoroutine(_ability.CooldownTimer());
        }
    }

    private void FixedUpdate()
    {
        if (Player != null && Target is Weapon weapon)
        {
            slider.value = weapon.FireTimePercent;
        }
    }

    private void UpdateTimer(float percentage)
    {
        slider.value = percentage;
    }
#if UNITY_EDITOR
    [Header("Debug")] [SerializeField] private AbilityData testData;

    [ContextMenu("Test Ability")]
    private void TestAbility()
    {
        _ability = new Ability(testData);
        _ability.CooldownEvent += UpdateTimer;
        OnClick();
    }
#endif
}