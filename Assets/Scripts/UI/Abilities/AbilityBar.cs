using System.Collections;
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
        public ShipInfo Player;

        [SerializeField] private GameObject _weaponButtonPrefab;
        [SerializeField] private SubsystemButtonData _abilityButtonData;
        private List<Ability> _abilities;

        public void SetAbilities(List<Ability> abilities)
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
                foreach (Ability ability in _abilities)
                {
                    GameObject abilityButton = Instantiate(_weaponButtonPrefab, transform, false);
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
            _abilities = new List<Ability>();
            foreach (AbilityData abilityData in _abilityDatas)
            {
                Ability ability = new Ability(abilityData);
                _abilities.Add(ability);
            }

            Refresh();
        }
#endif
    }
}