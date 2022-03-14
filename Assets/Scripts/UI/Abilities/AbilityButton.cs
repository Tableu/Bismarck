using Ships.Components;
using Ships.Fleets;
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
        private DamageableComponent _target;

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

        protected override void Update()
        {
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
            Ship = player;
            _ability = ability;
            if (ability != null)
            {
                ButtonData = ability.Data.ButtonData;
                _ability.CooldownEvent += UpdateTimer;
            }

            Refresh();
        }

        public void Refresh()
        {
            if (_ability != null)
            {
                _target = _ability.Target;
                if (_target == null && _ability.Data.ValidTargets.HasFlag(FleetAgroStatus.Self))
                {
                    EnableButton();
                }
                else if (_target != null &&
                         Player.Fleet.AgroStatusMap.TryGetValue(_target.ShipStats.Fleet,
                             out FleetAgroStatus fleetAgroStatus) &&
                         _ability.Data.ValidTargets.HasFlag(fleetAgroStatus))
                {
                    EnableButton();
                }
                else
                {
                    DisableButton();
                }
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