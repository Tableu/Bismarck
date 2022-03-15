using System.Collections.Generic;
using Ships.Components;
using Systems.Abilities;
using UI.InfoWindow;
using UnityEngine;

namespace UI.Abilities
{
    public class AbilityBar : MonoBehaviour
    {
        /// <summary>
        ///     Takes a list of Abilities and spawns buttons for them in a toolbar
        /// </summary>
        public ShipStats Player;

        [SerializeField] private GameObject _buttonPrefab;
        [SerializeField] private SubsystemButtonData _buttonData;
        [SerializeField] private bool _weapons;
        private List<Ability> _abilities;
        private List<AbilityButton> _abilityButtons;

        public void Start()
        {
            if (_weapons)
            {
                _abilities = new List<Ability>();
                _abilityButtons = new List<AbilityButton>();
                foreach (Weapon weapon in Player.AbilityManager.Weapons)
                {
                    if (weapon != null && weapon.Attack != null)
                    {
                        _abilities.Add(weapon.Attack);
                        GameObject abilityButton = Instantiate(_buttonPrefab, transform, false);
                        AbilityButton button = abilityButton.GetComponent<AbilityButton>();
                        button.Initialize(Player, weapon.Attack);
                        button.SubsystemComponent = weapon;
                        _abilityButtons.Add(button);
                    }
                }
            }
            else
            {
                _abilities = Player.AbilityManager.Abilities;
                _abilityButtons = new List<AbilityButton>();
                foreach (Ability ability in _abilities)
                {
                    GameObject abilityButton = Instantiate(_buttonPrefab, transform, false);
                    AbilityButton button = abilityButton.GetComponent<AbilityButton>();
                    button.Initialize(Player, ability);
                    _abilityButtons.Add(button);
                }
            }

            Player.AbilityManager.OnTargetChanged += Refresh;
        }

        public void Refresh()
        {
            foreach (AbilityButton button in _abilityButtons)
            {
                button.Refresh();
            }
        }
#if UNITY_EDITOR
        [Header("Debug")] [SerializeField] private List<AbilityData> _abilityDatas;
        [SerializeField] private DamageableComponent _target;
        [ContextMenu("Test Refresh")]
        private void TestRefresh()
        {
            _abilities = new List<Ability>();
            foreach (AbilityData abilityData in _abilityDatas)
            {
                Ability ability = new Ability(abilityData, Player);
                _abilities.Add(ability);
            }

            Refresh();
        }
#endif
    }
}