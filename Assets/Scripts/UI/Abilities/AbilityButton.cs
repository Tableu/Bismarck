using Ships.Components;
using Systems.Abilities;
using UI.InfoWindow;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Abilities
{
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
            if (!_ability.OnCooldown)
            {
                if (_ability.Fire())
                {
                    StartCoroutine(_ability.CooldownTimer());
                }
            }
        }

        public void Initialize(ShipStats player, Ability ability)
        {
            Player = player;
            shipStats = player;
            _ability = ability;
            ButtonData = ability.Data.ButtonData;
            if (_ability != null)
            {
                _ability.CooldownEvent += UpdateTimer;
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
            _ability = new Ability(testData, Player);
            _ability.CooldownEvent += UpdateTimer;
            OnClick();
        }
#endif
    }
}