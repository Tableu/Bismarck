using System.Collections.Generic;
using System.Linq;
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
        private List<CooldownAbility> _abilities;

        private void Start()
        {
            if (_weapons)
            {
                _abilities = new List<CooldownAbility>();
                foreach (Weapon weapon in Player.AbilityManager.Weapons)
                {
                    if (weapon != null && weapon.Attack != null)
                    {
                        _abilities.Add(weapon.Attack);
                    }
                }

                Refresh();
            }
            else
            {
                SetAbilities(Player.AbilityManager.Abilities.Cast<CooldownAbility>().ToList());
            }
        }

        public void SetAbilities(List<CooldownAbility> abilities)
        {
            _abilities = abilities;
            Refresh();
        }

        [ContextMenu("Refresh")]
        public void Refresh()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            if (Player != null)
            {
                foreach (CooldownAbility ability in _abilities)
                {
                    GameObject abilityButton = Instantiate(_buttonPrefab, transform, false);
                    AbilityButton button = abilityButton.GetComponent<AbilityButton>();
                    button.Initialize(Player, ability);
                }
            }
        }
#if UNITY_EDITOR
        [Header("Debug")] [SerializeField] private List<AbilityData> _abilityDatas;
        [ContextMenu("Test Refresh")]
        private void TestRefresh()
        {
            _abilities = new List<CooldownAbility>();
            foreach (AbilityData abilityData in _abilityDatas)
            {
                CooldownAbility ability = new Ability(abilityData);
                _abilities.Add(ability);
            }

            Refresh();
        }
#endif
    }
}